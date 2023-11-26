using UnityEngine;

public class WeedingTable : ImprovableFlowerTable
{
    [SerializeField] private Transform hoeOnTableTransform;
    [SerializeField] private Hoe hoe;

    private delegate void WeedingTableAction();
    private event WeedingTableAction WeedingTableEvent;
    // redo in all places to something like that using either HelperCalss or try to create extensionMethod for bool? with implicit operator from bool? to bool
    private readonly bool? nullableTrue = true;

    public override void ExecuteClickableAbility()
    {
        if (playerBusyness.IsPlayerFree)
        {
            if (playerPickableObjectHandler.IsPickableObjectNull)
            {
                SetPlayerDestination();
                WeedingTableEvent = null;
                WeedingTableEvent += TakeHoeInPlayerHands;
            }
            else if ((playerPickableObjectHandler.CurrentPickableObject as Hoe)?.Equals(hoe) == nullableTrue)
            {
                SetPlayerDestination();
                WeedingTableEvent = null;
                WeedingTableEvent += PutHoeOnWeedingTable;
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is Hammer && tableLvl < 2)
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
        playerPickableObjectHandler.ClearPickableObject();
        hoe.GiveHoe(hoeOnTableTransform);
    }
}
