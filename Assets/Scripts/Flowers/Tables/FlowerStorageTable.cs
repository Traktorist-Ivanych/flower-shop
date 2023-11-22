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
        // kill me (please)
        // if branches should be separate methods
        if (playerBusyness.IsPlayerFree)
        {
            if (isFlowerOnStorageTable)
            {
                if (PlayerPickableObjectHandler.IsPlayerPickableObjectNull())
                {
                    FlowerStorageTableEvent = null;
                    FlowerStorageTableEvent += TakePot;
                    SetPlayerDestination();
                }
                // playerDinamicObject.GetCurrentPlayerDinamicObject() to some local variable (repeated method access)
                else if (PlayerPickableObjectHandler.GetCurrentPlayerPickableObject() is WateringCan)
                {
                    wateringCan = PlayerPickableObjectHandler.GetCurrentPlayerPickableObject() as WateringCan;
                    if (wateringCan.GetGroweringRoom() == groweringRoom && wateringCan.CurrentWateringsNumber > 0 &&
                        potOnTable.IsFlowerNeedWater)
                    {
                        FlowerStorageTableEvent = null;
                        FlowerStorageTableEvent += PourPotOnStorageTable;
                        SetPlayerDestination();
                    }
                }
                else if (PlayerPickableObjectHandler.GetCurrentPlayerPickableObject() is Hoe)
                {
                    hoe = PlayerPickableObjectHandler.GetCurrentPlayerPickableObject() as Hoe;
                    if (potOnTable.IsWeedInPot && hoe.GetGroweringRoom() == groweringRoom)
                    {
                        SetPlayerDestination();
                        FlowerStorageTableEvent = null;
                        FlowerStorageTableEvent += DeleteWeed;
                    }
                }
            }
            else if (PlayerPickableObjectHandler.GetCurrentPlayerPickableObject() is Pot)
            {
                potOnTable = PlayerPickableObjectHandler.GetCurrentPlayerPickableObject() as Pot;
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
        PlayerPickableObjectHandler.ClearPlayerPickableObject();
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
