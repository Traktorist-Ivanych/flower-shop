using UnityEngine;

// rename to FlowersCrossingTable
public class FlowersCrossingTable : FlowerTable
{
    [SerializeField] private Transform tablePotTransform;
    [SerializeField] private CrossingTableProcess crossingTableProcess;

    private delegate void CrossingTableAction();
    // MAYBE rename to ExecutePlayerAbility or etc ('CrossingTable'Event and we already in class with such name)
    private event CrossingTableAction CrossingTableEvent;

    private Pot potOnTable;
    private bool isPotOnCrossingTable;

    public Pot GetPotOnTable() 
    {
        return potOnTable;
    }
    public bool IsPotOnCrossingTable
    {
        get => isPotOnCrossingTable;
    }

    public override void ExecuteClickableAbility()
    {
        if (playerBusyness.IsPlayerFree)
        {
            if (isPotOnCrossingTable && !crossingTableProcess.IsSeedCrossing && PlayerPickableObjectHandler.IsPlayerPickableObjectNull())
            {
                SetPlayerDestination();
                CrossingTableEvent = null;
                CrossingTableEvent += GetPotFromCrossingTable;
            }
            else if (PlayerPickableObjectHandler.GetCurrentPlayerPickableObject() is Pot)
            {
                potOnTable = PlayerPickableObjectHandler.GetCurrentPlayerPickableObject() as Pot;
                if (potOnTable.GetGroweringRoom() == groweringRoom && potOnTable.FlowerGrowingLvl >= 3 &&
                    !potOnTable.IsWeedInPot && !crossingTableProcess.IsTableBroken)
                {
                    SetPlayerDestination();
                    CrossingTableEvent = null;
                    CrossingTableEvent += PutPotOnCrossingTable;
                }
            }
        }
    }

    public override void ExecutePlayerAbility()
    {
        CrossingTableEvent?.Invoke();
        crossingTableProcess.CheckCrossingAbility();
    }

    private void PutPotOnCrossingTable()
    {
        potOnTable.GivePotAndSetPlayerFree(tablePotTransform);
        PlayerPickableObjectHandler.ClearPlayerPickableObject();
        isPotOnCrossingTable = true;
    }

    private void GetPotFromCrossingTable()
    {
        potOnTable.TakePotInPlayerHands();
        isPotOnCrossingTable = false;
    }
}
