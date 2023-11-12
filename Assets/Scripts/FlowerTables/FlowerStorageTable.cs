using UnityEngine;

public class FlowerStorageTable : FlowerTable
{
    [SerializeField] private Transform tablePotTransform;
    [SerializeField] private WeedPlanter weedPlanter;

    private delegate void FlowerStorageTableAction();
    private event FlowerStorageTableAction FlowerStorageTableEvent;

    private Hoe hoe;
    private WateringCan wateringCan;
    private Pot potOnTable;
    private bool isFlowerOnStorageTable;

    public override void ExecuteClickableAbility()
    {
        if (playerBusyness.IsPlayerFree)
        {
            if (isFlowerOnStorageTable)
            {
                if (playerDinamicObject.IsPlayerDinamicObjectNull())
                {
                    FlowerStorageTableEvent = null;
                    FlowerStorageTableEvent += TakePot;
                    SetPlayerDestination();
                }
                else if (playerDinamicObject.GetCurrentPlayerDinamicObject() is WateringCan)
                {
                    wateringCan = playerDinamicObject.GetCurrentPlayerDinamicObject() as WateringCan;
                    if (wateringCan.GetGroweringRoom() == groweringRoom && wateringCan.CurrentWateringsNumber > 0 &&
                        potOnTable.IsFlowerNeedWater)
                    {
                        FlowerStorageTableEvent = null;
                        FlowerStorageTableEvent += PourPotOnStorageTable;
                        SetPlayerDestination();
                    }
                }
                else if (playerDinamicObject.GetCurrentPlayerDinamicObject() is Hoe)
                {
                    hoe = playerDinamicObject.GetCurrentPlayerDinamicObject() as Hoe;
                    if (potOnTable.IsWeedInPot && hoe.GetGroweringRoom() == groweringRoom)
                    {
                        SetPlayerDestination();
                        FlowerStorageTableEvent = null;
                        FlowerStorageTableEvent += DeleteWeed;
                    }
                }
            }
            else if (playerDinamicObject.GetCurrentPlayerDinamicObject() is Pot)
            {
                potOnTable = playerDinamicObject.GetCurrentPlayerDinamicObject() as Pot;
                if (potOnTable.GetGroweringRoom() == groweringRoom || groweringRoom == IGrowingRoom.GroweringRoom.Any)
                {
                    FlowerStorageTableEvent = null;
                    FlowerStorageTableEvent += GivePot;
                    SetPlayerDestination();
                }
            }
        }
    }

    public override void ExecutePlayerAbility()
    {
        FlowerStorageTableEvent?.Invoke();
    }

    private void GivePot()
    {
        potOnTable.GivePotAndSetPlayerFree(tablePotTransform);
        playerDinamicObject.ClearPlayerDinamicObject();
        isFlowerOnStorageTable = true;
        if (potOnTable.IsSoilInsidePot)
        {
            weedPlanter.AddPotInPlantingWeedList(potOnTable);
        }
    }

    private void PourPotOnStorageTable()
    {
        potOnTable.PourFlower();
    }

    private void DeleteWeed()
    {
        StartCoroutine(hoe.DeleteWeedWithHoe(potOnTable, weedPlanter));
    }

    private void TakePot()
    {
        potOnTable.TakePotInPlayerHands();
        isFlowerOnStorageTable = false;
        weedPlanter.RemovePotFormPlantingWeedList(potOnTable);
    }
}
