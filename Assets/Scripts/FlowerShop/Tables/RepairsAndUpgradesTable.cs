using System;
using System.Collections;
using System.Collections.Generic;
using FlowerShop.PickableObjects;
using FlowerShop.RepairsAndUpgrades;
using FlowerShop.Saves.SaveData;
using FlowerShop.Settings;
using FlowerShop.Tables.Abstract;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class RepairsAndUpgradesTable : Table, ISavableObject
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;

        [SerializeField] private Transform hammerOnTableTransform;
        [SerializeField] private RepairingAndUpgradingHammer repairingAndUpgradingHammer;

        private readonly List<IUpgradableTable> upgradableTables = new();
        private bool isRepairingAndUpgradingHammerInPlayerHands;
        
        [field: SerializeField] public string UniqueKey { get; private set; }

        private void Awake()
        {
            Load();
        }

        private void Start()
        {
            if (isRepairingAndUpgradingHammerInPlayerHands)
            {
                repairingAndUpgradingHammer.LoadInPlayerHands();
                ShowAllUpgradeIndicators();
            }
        }

        public override void ExecuteClickableAbility()
        {
            if (playerBusyness.IsPlayerFree)
            {
                if (CanPlayerTakeHammerInHands())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(TakeHammerInPlayerHands);
                }
                else if (CanPlayerPutHammerOnTable())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(PutHammerOnTable);
                }
            }
        }

        public void AddUpgradableTableToList(IUpgradableTable upgradableTable)
        {
            upgradableTables.Add(upgradableTable);
        }

        public void Load()
        {
            RepairsAndUpgradesTableForSaving repairsAndUpgradesTableForLoading =
                SavesHandler.Load<RepairsAndUpgradesTableForSaving>(UniqueKey);

            if (repairsAndUpgradesTableForLoading.IsValuesSaved &&
                repairsAndUpgradesTableForLoading.IsRepairingAndUpgradingHammerInPlayerHands)
            {
                isRepairingAndUpgradingHammerInPlayerHands = true;
            }
        }

        public void Save()
        {
            RepairsAndUpgradesTableForSaving repairsAndUpgradesTableForSaving = 
                new(isRepairingAndUpgradingHammerInPlayerHands);
            
            SavesHandler.Save(UniqueKey, repairsAndUpgradesTableForSaving);
        }
        
        private bool CanPlayerTakeHammerInHands()
        {
            return playerPickableObjectHandler.IsPickableObjectNull;
        }

        private void TakeHammerInPlayerHands()
        {
            isRepairingAndUpgradingHammerInPlayerHands = true;
            repairingAndUpgradingHammer.TakeInPlayerHandsAndSetPlayerFree();
            StartCoroutine(ShowAllUpgradeIndicatorsWithDelay());
            
            Save();
        }

        private bool CanPlayerPutHammerOnTable()
        {
            return playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer;
        }

        private void PutHammerOnTable()
        {
            isRepairingAndUpgradingHammerInPlayerHands = false;
            playerPickableObjectHandler.ResetPickableObject();
            repairingAndUpgradingHammer.PutOnTableAndSetPlayerFree(hammerOnTableTransform);

            foreach (IUpgradableTable upgradableTable in upgradableTables)
            {
                upgradableTable.HideUpgradeIndicator();
            }
            
            Save();
        }

        private IEnumerator ShowAllUpgradeIndicatorsWithDelay()
        {
            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);

            ShowAllUpgradeIndicators();
        }

        private void ShowAllUpgradeIndicators()
        {
            foreach (IUpgradableTable upgradableTable in upgradableTables)
            {
                upgradableTable.ShowUpgradeIndicator();
            }
        }
            
    }
}
