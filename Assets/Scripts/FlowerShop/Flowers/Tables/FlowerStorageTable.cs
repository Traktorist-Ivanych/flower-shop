using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using UnityEngine;

public class FlowerStorageTable : FlowerTable
{
    [SerializeField] private Transform tablePotTransform;
    [SerializeField] private WeedPlanter weedPlanter;
    [SerializeField] private GrowingRoom growingRoomAny;
    
    private delegate void FlowerStorageTableAction();
    private event FlowerStorageTableAction FlowerStorageTableEvent;

    private WeedingHoe weedingHoe;
    private WateringCan wateringCan;
    private Pot potOnTable;
    private bool isFlowerOnStorageTable;

    public override void ExecuteClickableAbility()
    {
        // kill me (please)
        // if branches should be separate methods
        if (playerBusyness.IsPlayerFree)
        {
            if (isFlowerOnStorageTable)
            {
                if (playerPickableObjectHandler.IsPickableObjectNull)
                {
                    FlowerStorageTableEvent = null;
                    FlowerStorageTableEvent += TakePot;
                    SetPlayerDestination();
                }
                // playerDinamicObject.GetCurrentPlayerDinamicObject() to some local variable (repeated method access)
                else if (playerPickableObjectHandler.CurrentPickableObject is WateringCan)
                {
                    wateringCan = playerPickableObjectHandler.CurrentPickableObject as WateringCan;
                    if (wateringCan.GrowingRoom == growingRoom && wateringCan.CurrentWateringsNumber > 0 &&
                        potOnTable.IsFlowerNeedWater)
                    {
                        FlowerStorageTableEvent = null;
                        FlowerStorageTableEvent += PourPotOnStorageTable;
                        SetPlayerDestination();
                    }
                }
                else if (playerPickableObjectHandler.CurrentPickableObject is WeedingHoe)
                {
                    weedingHoe = playerPickableObjectHandler.CurrentPickableObject as WeedingHoe;
                    if (potOnTable.IsWeedInPot && weedingHoe.GrowingRoom == growingRoom)
                    {
                        SetPlayerDestination();
                        FlowerStorageTableEvent = null;
                        FlowerStorageTableEvent += DeleteWeed;
                    }
                }
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is Pot)
            {
                potOnTable = playerPickableObjectHandler.CurrentPickableObject as Pot;
                if (potOnTable.GrowingRoom == growingRoom || growingRoom == growingRoomAny)
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
        playerPickableObjectHandler.ClearPickableObject();
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
        StartCoroutine(weedingHoe.DeleteWeed(potOnTable, weedPlanter));
    }

    private void TakePot()
    {
        potOnTable.TakeInPlayerHands();
        isFlowerOnStorageTable = false;
        weedPlanter.RemovePotFormPlantingWeedList(potOnTable);
    }
}
