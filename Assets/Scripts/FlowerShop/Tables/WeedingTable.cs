using FlowerShop.PickableObjects;
using FlowerShop.Tables.Abstract;
using UnityEngine;

namespace FlowerShop.Tables
{
    public class WeedingTable : UpgradableTable
    {
        [SerializeField] private Transform hoeOnTableTransform;
        [SerializeField] private WeedingHoe weedingHoe;

        public override void ExecuteClickableAbility()
        {
            if (playerBusyness.IsPlayerFree)
            {
                if (CanPlayerTakeHoeInHands())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(TakeHoeInPlayerHands);
                }
                else if (CanPlayerPutHoeOnTable())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(PutHoeOnTable);
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
            
            weedingHoe.Upgrade();
        }

        private bool CanPlayerTakeHoeInHands()
        {
            return playerPickableObjectHandler.IsPickableObjectNull;
        }

        private void TakeHoeInPlayerHands()
        {
            weedingHoe.TakeInPlayerHandsAndSetPlayerFree();
        }

        private bool CanPlayerPutHoeOnTable()
        {
            if (playerPickableObjectHandler.CurrentPickableObject is WeedingHoe currentWeedingHoe)
            {
                return currentWeedingHoe.Equals(weedingHoe);
            }

            return false;
        }

        private void PutHoeOnTable()
        {
            playerPickableObjectHandler.ResetPickableObject();
            weedingHoe.PutOnTableAndSetPlayerFree(hoeOnTableTransform);
        }

        private bool CanPlayerUpgradeTable()
        {
            return playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer &&
                   tableLvl < repairsAndUpgradesSettings.MaxUpgradableTableLvl;
        }
    }
}
