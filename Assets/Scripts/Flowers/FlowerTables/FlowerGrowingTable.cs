using UnityEngine;
using Zenject;

public class FlowerGrowingTable : ImprovableBreakableFlowerTable
{
    [SerializeField] private Transform tablePotTransform;
    [SerializeField] private MeshRenderer growingLightMeshRenderer;
    [SerializeField] private MeshRenderer growingTableFanMeshRenderer;
    [SerializeField] private Mesh[] growingLightLvlMeshes = new Mesh[2];
    [SerializeField] private WeedPlanter weedPlanter;

    private delegate void FlowerGrowingTableAction();
    private event FlowerGrowingTableAction FlowerGrowingTableEvent;

    private WateringCan wateringCan;
    private Hoe hoe;
    private Pot potOnTable;
    private Transform growingTableFanTransform;
    private MeshFilter growingLightMeshFilter;
    private bool isPotOnTable;

    private protected override void Start()
    {
        base.Start();
        growingLightMeshFilter = growingLightMeshRenderer.GetComponent<MeshFilter>();
        growingTableFanTransform = growingTableFanMeshRenderer.GetComponent<Transform>();

        SetActionsBeforeBrokenQuantity(
            gameConfiguration.FlowerGrowingTableMinQuantity * (tableLvl + 1),
            gameConfiguration.FlowerGrowingTableMaxQuantity * (tableLvl + 1));
    }

    private void Update()
    {
        if (isPotOnTable)
        {
            growingTableFanTransform.Rotate(Vector3.up, gameConfiguration.ObjectsRotateDegreesPerSecond * Time.deltaTime);
        }
    }

    public override void ExecuteClickableAbility()
    {
        if (playerBusyness.IsPlayerFree)
        {
            if (!IsTableBroken)
            {
                if (isPotOnTable)
                {
                    if (playerDinamicObject.IsPlayerDynamicObjectNull())
                    {
                        SetPlayerDestination();
                        FlowerGrowingTableEvent = null;
                        FlowerGrowingTableEvent += TakePotFromGrowingTable;
                    }
                    if (playerDinamicObject.GetCurrentPlayerDinamicObject() is WateringCan)
                    {
                        wateringCan = playerDinamicObject.GetCurrentPlayerDinamicObject() as WateringCan;
                        if (wateringCan.GetGroweringRoom() == groweringRoom && wateringCan.CurrentWateringsNumber > 0 &&
                            potOnTable.IsFlowerNeedWater)
                        {
                            SetPlayerDestination();
                            FlowerGrowingTableEvent = null;
                            FlowerGrowingTableEvent += PourPotOnGrowingTable;
                        }
                    }
                    else if (playerDinamicObject.GetCurrentPlayerDinamicObject() is Hoe)
                    {
                        hoe = playerDinamicObject.GetCurrentPlayerDinamicObject() as Hoe;
                        if (potOnTable.IsWeedInPot && hoe.GetGroweringRoom() == groweringRoom)
                        {
                            SetPlayerDestination();
                            FlowerGrowingTableEvent = null;
                            FlowerGrowingTableEvent += DeleteWeed;
                        }

                    }
                }
                else if (playerDinamicObject.GetCurrentPlayerDinamicObject() is Pot)
                {
                    potOnTable = playerDinamicObject.GetCurrentPlayerDinamicObject() as Pot;
                    if (potOnTable.GetGroweringRoom() == groweringRoom &&
                        potOnTable.PlantedFlower.FlowerEnum != IFlower.Flower.Empty && potOnTable.FlowerGrowingLvl < 3)
                    {
                        SetPlayerDestination();
                        FlowerGrowingTableEvent = null;
                        FlowerGrowingTableEvent += PutPotOnGrowingTable;
                    }
                }
            }

            if (playerDinamicObject.GetCurrentPlayerDinamicObject() is Hammer)
            {
                if (IsTableBroken)
                {
                    SetPlayerDestination();
                    FlowerGrowingTableEvent = null;
                    FlowerGrowingTableEvent += FixFlowerGrowingTable;
                }
                else if (tableLvl < 2)
                {
                    SetPlayerDestination();
                    FlowerGrowingTableEvent = null;
                    FlowerGrowingTableEvent += ShowImprovableCanvas;
                }
            }
        }
    }

    public override void ExecutePlayerAbility()
    {
        FlowerGrowingTableEvent?.Invoke();
    }

    public override void ImproveTable()
    {
        base.ImproveTable();
        growingLightMeshFilter.mesh = growingLightLvlMeshes[tableLvl - 1];
        growingTableFanMeshRenderer.enabled = true;
    }

    private void TakePotFromGrowingTable()
    {
        weedPlanter.RemovePotFormPlantingWeedList(potOnTable);
        potOnTable.TakePotInPlayerHandsFromGrowingTable();
        potOnTable = null;
        isPotOnTable = false;
        growingLightMeshRenderer.enabled = false;
        UseBreakableFlowerTable();
    }

    private void PourPotOnGrowingTable()
    {
        potOnTable.PourFlower();
    }

    private void DeleteWeed()
    {
        StartCoroutine(hoe.DeleteWeedWithHoe(potOnTable, weedPlanter));
    }

    private void PutPotOnGrowingTable()
    {
        potOnTable.GivePotOnGrowingTableAndSetPlayerFree(tablePotTransform, tableLvl);
        isPotOnTable = true;
        growingLightMeshRenderer.enabled = true;
        playerDinamicObject.ClearPlayerDinamicObject();
        weedPlanter.AddPotInPlantingWeedList(potOnTable);
    }

    private void FixFlowerGrowingTable()
    {
        FixBreakableFlowerTable(
            gameConfiguration.FlowerGrowingTableMinQuantity * (tableLvl + 1),
            gameConfiguration.FlowerGrowingTableMaxQuantity * (tableLvl + 1));
    }
}
