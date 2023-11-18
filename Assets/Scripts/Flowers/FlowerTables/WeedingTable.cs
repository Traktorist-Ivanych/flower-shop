using UnityEngine;

public class WeedingTable : ImprovableFlowerTable
{
    [SerializeField] private Transform hoeOnTableTransform;
    [SerializeField] private Hoe hoe;

    private delegate void WeedingTableAction();
    private event WeedingTableAction WeedingTableEvent;

    public override void ExecuteClickableAbility()
    {
        if (playerBusyness.IsPlayerFree)
        {
            if (playerDinamicObject.IsPlayerDinamicObjectNull())
            {
                SetPlayerDestination();
                WeedingTableEvent = null;
                WeedingTableEvent += TakeHoeInPlayerHands;
            }
            else if ((playerDinamicObject.GetCurrentPlayerDinamicObject() is Hoe) &&
                     (playerDinamicObject.GetCurrentPlayerDinamicObject() as Hoe).Equals(hoe))
            {
                SetPlayerDestination();
                WeedingTableEvent = null;
                WeedingTableEvent += PutHoeOnWeedingTable;
            }
            else if (playerDinamicObject.GetCurrentPlayerDinamicObject() is Hammer && tableLvl < 2)
            {
                SetPlayerDestination();
                WeedingTableEvent = null;
                WeedingTableEvent += ShowImprovableCanvas;
            }
        }
    }

    public override void ExecutePlayerAbility()
    {
        WeedingTableEvent?.Invoke();
    }

    public override void ImproveTable()
    {
        base.ImproveTable();
        hoe.ImproveHoe();
    }

    private void TakeHoeInPlayerHands()
    {
        hoe.TakeHoe();
    }

    private void PutHoeOnWeedingTable()
    {
        playerDinamicObject.ClearPlayerDinamicObject();
        hoe.GiveHoe(hoeOnTableTransform);
    }
}
