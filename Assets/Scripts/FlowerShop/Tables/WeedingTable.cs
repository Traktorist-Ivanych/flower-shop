using FlowerShop.PickableObjects;
using FlowerShop.Tables.Abstract;
using UnityEngine;
using UnityEngine.Serialization;

public class WeedingTable : UpgradableTable
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
                WeedingTableEvent = null;
                WeedingTableEvent += TakeHoeInPlayerHands;
            }
            else if ((playerPickableObjectHandler.CurrentPickableObject as WeedingHoe)?.Equals(weedingHoe) == nullableTrue)
            {
                WeedingTableEvent = null;
                WeedingTableEvent += PutHoeOnWeedingTable;
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer && tableLvl < 2)
            {
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
        weedingHoe.Improve();
    }

    private void TakeHoeInPlayerHands()
    {
        weedingHoe.TakeInPlayerHandsAndSetPlayerFree();
    }

    private void PutHoeOnWeedingTable()
    {
        playerPickableObjectHandler.ResetPickableObject();
        weedingHoe.PutOnTableAndSetPlayerFree(hoeOnTableTransform);
    }
}
