using System.Collections;
using FlowerShop.PickableObjects;
using FlowerShop.Tables.Abstract;
using UnityEngine;
using Zenject;

public class WateringTable : UpgradableBreakableTable
{
    [Inject] private readonly GameConfiguration gameConfiguration;
    
    [SerializeField] private Transform wateringCanTableTransform;
    [SerializeField] private WateringCan wateringCan;
    [SerializeField] private ParticleSystem waterPS;

    private delegate void WateringTableAction();
    private event WateringTableAction WateringTableEvent;

    private protected override void Start()
    {
        base.Start();

        SetActionsBeforeBrokenQuantity(
            repairsAndUpgradesSettings.WateringTableMinQuantity * (tableLvl + 1),
            repairsAndUpgradesSettings.WateringTableMaxQuantity * (tableLvl + 1));
    }

    public override void ExecuteClickableAbility()
    {
        if (playerBusyness.IsPlayerFree)
        {
            if (playerPickableObjectHandler.IsPickableObjectNull && !wateringCan.IsWateringCanNeedForReplenish() && !IsTableBroken)
            {
                WateringTableEvent = null;
                WateringTableEvent += TakeWateringCanFromTable;
            }
            // try to do with one check/cast
            else if ((playerPickableObjectHandler.CurrentPickableObject is WateringCan) &&
                     (playerPickableObjectHandler.CurrentPickableObject as WateringCan).Equals(wateringCan))
            {
                WateringTableEvent = null;
                WateringTableEvent += PutWateringCanOnTable;
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer)
            {
                if (IsTableBroken)
                {
                    WateringTableEvent = null;
                    WateringTableEvent += FixWateringTable;
                }
                else if (tableLvl < 2)
                {
                    WateringTableEvent = null;
                    WateringTableEvent += ShowUpgradeCanvas;
                }
            }
        }
    }

    public override void ExecutePlayerAbility()
    {
        WateringTableEvent?.Invoke();
    }

    public override void UpgradeTable()
    {
        base.UpgradeTable();
        wateringCan.ImproveWateringCan();
    }

    private void TakeWateringCanFromTable()
    {
        wateringCan.TakeInPlayerHandsAndSetPlayerFree();
    }

    private void PutWateringCanOnTable()
    {
        wateringCan.PutOnTableAndSetPlayerFree(wateringCanTableTransform);
        playerPickableObjectHandler.ResetPickableObject();

        if (wateringCan.IsWateringCanNeedForReplenish())
        {
            StartCoroutine(ReplenishWateringCan());
        }
    }

    private IEnumerator ReplenishWateringCan()
    {
        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay);
        waterPS.Play();
        yield return new WaitForSeconds(wateringCan.ReplenishWateringCanTime());
        waterPS.Stop();
        wateringCan.ReplenishWateringCan();
        UseBreakableTable();
    }

    private void FixWateringTable()
    {
        FixBreakableFlowerTable(
            repairsAndUpgradesSettings.WateringTableMinQuantity * (tableLvl + 1),
            repairsAndUpgradesSettings.WateringTableMaxQuantity * (tableLvl + 1));
    }
}
