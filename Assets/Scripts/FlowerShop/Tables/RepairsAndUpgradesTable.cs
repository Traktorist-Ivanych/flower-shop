using System.Collections;
using System.Collections.Generic;
using FlowerShop.PickableObjects;
using FlowerShop.RepairsAndUpgrades;
using FlowerShop.Settings;
using FlowerShop.Tables.Abstract;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class RepairsAndUpgradesTable : Table
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;

        [SerializeField] private Transform hammerOnTableTransform;
        [SerializeField] private RepairingAndUpgradingHammer repairingAndUpgradingHammer;

        private readonly List<IUpgradableTable> upgradableTables = new();

        public override void ExecuteClickableAbility()
        {
            if (playerBusyness.IsPlayerFree)
            {
                if (playerPickableObjectHandler.IsPickableObjectNull)
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(TakeHammerInPlayerHands);
                }
                else if (playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer)
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(PutHammerOnTable);
                }
            }
        }

        public void AddUpgradableTableToList(IUpgradableTable upgradableTable)
        {
            upgradableTables.Add(upgradableTable);
        }

        private void TakeHammerInPlayerHands()
        {
            repairingAndUpgradingHammer.TakeInPlayerHandsAndSetPlayerFree();
            StartCoroutine(ShowAllUpgradeIndicators());
        }

        private void PutHammerOnTable()
        {
            playerPickableObjectHandler.ResetPickableObject();
            repairingAndUpgradingHammer.PutOnTableAndSetPlayerFree(hammerOnTableTransform);

            foreach (IUpgradableTable upgradableTable in upgradableTables)
            {
                upgradableTable.HideUpgradeIndicator();
            }
        }

        private IEnumerator ShowAllUpgradeIndicators()
        {
            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            
            foreach (IUpgradableTable upgradableTable in upgradableTables)
            {
                upgradableTable.ShowUpgradeIndicator();
            }
        }
    }
}
