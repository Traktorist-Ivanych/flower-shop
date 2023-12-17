using System.Collections;
using FlowerShop.PickableObjects;
using FlowerShop.Settings;
using FlowerShop.Tables.Abstract;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class WateringTable : UpgradableBreakableTable
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        
        [SerializeField] private Transform wateringCanTableTransform;
        [SerializeField] private WateringCan wateringCan;
        [SerializeField] private ParticleSystem waterPS;

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
                if (CanPlayerTakeWateringCanInHands())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(TakeWateringCanInPlayerHands);
                }
                else if (CanPlayerPutWateringCanOnTable())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(PutWateringCanOnTable);
                }
                else if (CanPlayerFixWateringTable())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(FixWateringTable);
                }
                else if (CanPlayerUpgradeTable())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(ShowUpgradeCanvas); 
                }
            }
        }

        public override void UpgradeTable()
        {
            base.UpgradeTable();
            
            wateringCan.UpgradeWateringCan();
        }

        private bool CanPlayerTakeWateringCanInHands()
        {
            return playerPickableObjectHandler.IsPickableObjectNull &&
                   !wateringCan.IsWateringCanNeedForReplenish() && !IsTableBroken;
        }

        private void TakeWateringCanInPlayerHands()
        {
            wateringCan.TakeInPlayerHandsAndSetPlayerFree();
        }

        private bool CanPlayerPutWateringCanOnTable()
        {
            if (playerPickableObjectHandler.CurrentPickableObject is WateringCan currentWateringCan)
            {
                return currentWateringCan.Equals(wateringCan);
            }

            return false;
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
            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            waterPS.Play();
            yield return new WaitForSeconds(wateringCan.ReplenishWateringCanTime());
            waterPS.Stop();
            wateringCan.ReplenishWateringCan();
            UseBreakableTable();
        }

        private bool CanPlayerFixWateringTable()
        {
            if (playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer)
            {
                return IsTableBroken;
            }

            return false;
        }

        private void FixWateringTable()
        {
            FixBreakableFlowerTable(
                repairsAndUpgradesSettings.WateringTableMinQuantity * (tableLvl + 1),
                repairsAndUpgradesSettings.WateringTableMaxQuantity * (tableLvl + 1));
        }

        private bool CanPlayerUpgradeTable()
        {
            if (playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer)
            {
                return tableLvl < repairsAndUpgradesSettings.MaxUpgradableTableLvl;
            }

            return false;
        }
    }
}
