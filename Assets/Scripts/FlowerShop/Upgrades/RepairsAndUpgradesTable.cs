using System.Collections;
using System.Collections.Generic;
using FlowerShop.PickableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace FlowerShop.Upgrades
{
    public class RepairsAndUpgradesTable : FlowerTable
    {
        [Inject] private readonly GameConfiguration gameConfiguration;

        [SerializeField] private Transform hammerOnTableTransform;
        [SerializeField] private UpgradingAndRepairingHammer upgradingAndRepairingHammer;

        private delegate void OnPlayerArriveAction();
        private event OnPlayerArriveAction OnPlayerArriveEvent;

        private readonly List<IUpgradableTable> upgradableTables = new();

        public override void ExecuteClickableAbility()
        {
            if (playerBusyness.IsPlayerFree)
            {
                if (playerPickableObjectHandler.IsPickableObjectNull)
                {
                    SetPlayerDestination();
                    OnPlayerArriveEvent = null;
                    OnPlayerArriveEvent += TakeHammerInPlayerHands;
                }
                else if (playerPickableObjectHandler.CurrentPickableObject is UpgradingAndRepairingHammer)
                {
                    SetPlayerDestination();
                    OnPlayerArriveEvent = null;
                    OnPlayerArriveEvent += PutHammerOnTable;
                }
            }
        }

        public override void ExecutePlayerAbility()
        {
            OnPlayerArriveEvent?.Invoke();
        }

        public void AddUpgradableTableToList(IUpgradableTable upgradableTable)
        {
            upgradableTables.Add(upgradableTable);
        }

        private void TakeHammerInPlayerHands()
        {
            upgradingAndRepairingHammer.TakeInPlayerHandsAndSetPlayerFree();
            StartCoroutine(ShowAllUpgradeIndicators());
        }

        private void PutHammerOnTable()
        {
            playerPickableObjectHandler.ClearPickableObject();
            upgradingAndRepairingHammer.PutOnTableAndSetPlayerFree(hammerOnTableTransform);

            foreach (IUpgradableTable upgradableTable in upgradableTables)
            {
                upgradableTable.HideUpgradeIndicator();
            }
        }

        private IEnumerator ShowAllUpgradeIndicators()
        {
            yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay);
            foreach (IUpgradableTable upgradableTable in upgradableTables)
            {
                upgradableTable.ShowUpgradeIndicator();
            }
        }
    }
}
