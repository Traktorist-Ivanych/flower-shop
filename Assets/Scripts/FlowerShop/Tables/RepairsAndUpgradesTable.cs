using System.Collections;
using System.Collections.Generic;
using FlowerShop.Help;
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
        [Inject] private readonly HelpCanvasLiaison helpCanvasLiaison;
        [Inject] private readonly HelpTexts helpTexts;

        [SerializeField] private Transform hammerOnTableTransform;
        [SerializeField] private RepairingAndUpgradingHammer repairingAndUpgradingHammer;

        private readonly List<IUpgradableTable> upgradableTables = new();
        private bool isRepairingAndUpgradingHammerInPlayerHands;
        private int currentTablesThatNeedRepairQuantity;
        
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

        private protected override void TryInteractWithTable()
        {
            base.TryInteractWithTable();

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
                else if (CanPlayerUseTableInfoCanvas())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(UseTableInfoCanvas);
                }
                else
                {
                    TryToShowHelpCanvas();
                }
            }
            else
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.PlayerBusy);
            }
        }

        private void TryToShowHelpCanvas()
        {
            helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.WrongPickableObject);
        }

        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerTakeHammerInHandsForSelectableEffect() || CanPlayerUseTableInfoCanvas() ||
                   CanPlayerPutHammerOnTable();
        }

        public void AddUpgradableTableToList(IUpgradableTable upgradableTable)
        {
            upgradableTables.Add(upgradableTable);
        }

        public void IncreaseTablesThatNeedRepairQuantity()
        {
            currentTablesThatNeedRepairQuantity++;
        }

        public void DecreaseTablesThatNeedRepairQuantity()
        {
            currentTablesThatNeedRepairQuantity--;
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

        private bool CanPlayerTakeHammerInHandsForSelectableEffect()
        {
            return currentTablesThatNeedRepairQuantity > 0 && CanPlayerTakeHammerInHands();
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

        private bool CanPlayerUseTableInfoCanvas()
        {
            return playerPickableObjectHandler.CurrentPickableObject is InfoBook;
        }

        private void UseTableInfoCanvas()
        {
            tableInfoCanvasLiaison.ShowCanvas(tableInfo, growingRoom);
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
                upgradableTable.HideIndicator();
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
                upgradableTable.ShowIndicator();
            }
        }
            
    }
}
