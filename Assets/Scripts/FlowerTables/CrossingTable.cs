using UnityEngine;

public class CrossingTable : FlowerTable
{
    [SerializeField] private Transform tablePotTransform;
    [SerializeField] private CrossingTableProcess crossingTableProcess;

    private delegate void CrossingTableAction();
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
            if (isPotOnCrossingTable)
            {
                if (playerDinamicObject.IsPlayerDinamicObjectNull())
                {
                    SetPlayerDestination();
                    CrossingTableEvent = null;
                    CrossingTableEvent += GetPotFromCrossingTable;
                }
            }
            else if (playerDinamicObject.GetCurrentPlayerDinamicObject() is Pot)
            {
                potOnTable = playerDinamicObject.GetCurrentPlayerDinamicObject() as Pot;
                if (potOnTable.GetGroweringRoom() == groweringRoom && potOnTable.FlowerGrowingLvl > 2 &&
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
        playerDinamicObject.ClearPlayerDinamicObject();
        isPotOnCrossingTable = true;
    }

    private void GetPotFromCrossingTable()
    {
        potOnTable.TakePotInPlayerHands();
        isPotOnCrossingTable = false;
    }
}
