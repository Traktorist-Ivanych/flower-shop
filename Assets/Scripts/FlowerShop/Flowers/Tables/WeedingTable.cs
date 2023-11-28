using FlowerShop.PickableObjects;
using UnityEngine;
using UnityEngine.Serialization;

public class WeedingTable : UpgradableFlowerTable
{
    [SerializeField] private Transform hoeOnTableTransform;
    [FormerlySerializedAs("hoe")] [SerializeField] private WeedingHoe weedingHoe;

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
            else if ((playerPickableObjectHandler.CurrentPickableObject as WeedingHoe)?.Equals(weedingHoe) == nullableTrue)
            {
                SetPlayerDestination();
                WeedingTableEvent = null;
                WeedingTableEvent += PutHoeOnWeedingTable;
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is UpgradingAndRepairingHammer && tableLvl < 2)
            {
                SetPlayerDestination();
                WeedingTableEvent = null;
                WeedingTableEvent += ShowUpgradeCanvas;
            }
        }
    }

    public override void ExecutePlayerAbility()
    {
        WeedingTableEvent?.Invoke();
    }

    public override void UpgradeTable()
    {
        base.UpgradeTable();
        weedingHoe.ImproveHoe();
    }

    private void TakeHoeInPlayerHands()
    {
        weedingHoe.TakeInPlayerHands();
    }

    private void PutHoeOnWeedingTable()
    {
        playerPickableObjectHandler.ClearPickableObject();
        weedingHoe.PutOnTable(hoeOnTableTransform);
    }
}
