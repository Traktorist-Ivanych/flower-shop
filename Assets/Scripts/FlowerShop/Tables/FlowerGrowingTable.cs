using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using UnityEngine;
using UnityEngine.Serialization;

public class FlowerGrowingTable : UpgradableBreakableFlowerTable
{
    [SerializeField] private Transform tablePotTransform;
    [SerializeField] private MeshRenderer growingLightMeshRenderer;
    [SerializeField] private MeshRenderer growingTableFanMeshRenderer;
    [SerializeField] private Mesh[] growingLightLvlMeshes = new Mesh[2];
    [SerializeField] private WeedPlanter weedPlanter;
    [FormerlySerializedAs("flowerEmpty")] [SerializeField] private FlowerName flowerNameEmpty;

    private delegate void FlowerGrowingTableAction();
    private event FlowerGrowingTableAction FlowerGrowingTableEvent;

    private WateringCan wateringCan;
    private WeedingHoe weedingHoe;
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
                    if (playerPickableObjectHandler.IsPickableObjectNull)
                    {
                        SetPlayerDestination();
                        FlowerGrowingTableEvent = null;
                        FlowerGrowingTableEvent += TakePotFromGrowingTable;
                    }
                    if (playerPickableObjectHandler.CurrentPickableObject is WateringCan)
                    {
                        wateringCan = playerPickableObjectHandler.CurrentPickableObject as WateringCan;
                        if (wateringCan.GrowingRoom == growingRoom && wateringCan.CurrentWateringsNumber > 0 &&
                            potOnTable.IsFlowerNeedWater)
                        {
                            SetPlayerDestination();
                            FlowerGrowingTableEvent = null;
                            FlowerGrowingTableEvent += PourPotOnGrowingTable;
                        }
                    }
                    else if (playerPickableObjectHandler.CurrentPickableObject is WeedingHoe)
                    {
                        weedingHoe = playerPickableObjectHandler.CurrentPickableObject as WeedingHoe;
                        if (potOnTable.IsWeedInPot && weedingHoe.GrowingRoom == growingRoom)
                        {
                            SetPlayerDestination();
                            FlowerGrowingTableEvent = null;
                            FlowerGrowingTableEvent += DeleteWeed;
                        }

                    }
                }
                else if (playerPickableObjectHandler.CurrentPickableObject is Pot)
                {
                    potOnTable = playerPickableObjectHandler.CurrentPickableObject as Pot;
                    if (potOnTable.GrowingRoom == growingRoom &&
                        potOnTable.PlantedFlowerInfo.FlowerName != flowerNameEmpty && potOnTable.FlowerGrowingLvl < 3)
                    {
                        SetPlayerDestination();
                        FlowerGrowingTableEvent = null;
                        FlowerGrowingTableEvent += PutPotOnGrowingTable;
                    }
                }
            }

            if (playerPickableObjectHandler.CurrentPickableObject is UpgradingAndRepairingHammer)
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
                    FlowerGrowingTableEvent += ShowUpgradeCanvas;
                }
            }
        }
    }

    public override void ExecutePlayerAbility()
    {
        FlowerGrowingTableEvent?.Invoke();
    }

    public override void UpgradeTable()
    {
        base.UpgradeTable();
        growingLightMeshFilter.mesh = growingLightLvlMeshes[tableLvl - 1];
        growingTableFanMeshRenderer.enabled = true;
    }

    private void TakePotFromGrowingTable()
    {
        weedPlanter.RemovePotFormPlantingWeedList(potOnTable);
        potOnTable.TakeInPlayerHandsFromGrowingTableAndSetPlayerFree();
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
        StartCoroutine(weedingHoe.DeleteWeed(potOnTable, weedPlanter));
    }

    private void PutPotOnGrowingTable()
    {
        potOnTable.PutOnGrowingTableAndSetPlayerFree(tablePotTransform, tableLvl);
        isPotOnTable = true;
        growingLightMeshRenderer.enabled = true;
        playerPickableObjectHandler.ClearPickableObject();
        weedPlanter.AddPotInPlantingWeedList(potOnTable);
    }

    private void FixFlowerGrowingTable()
    {
        FixBreakableFlowerTable(
            gameConfiguration.FlowerGrowingTableMinQuantity * (tableLvl + 1),
            gameConfiguration.FlowerGrowingTableMaxQuantity * (tableLvl + 1));
    }
}
