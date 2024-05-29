using FlowerShop.Help;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Tables.Abstract;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class WeedingTable : UpgradableTable, ISavableObject
    {
        [Inject] private readonly HelpCanvasLiaison helpCanvasLiaison;
        [Inject] private readonly HelpTexts helpTexts;

        [SerializeField] private Transform hoeOnTableTransform;
        [SerializeField] private WeedingHoe weedingHoe;
        
        [field: SerializeField] public string UniqueKey { get; private set; }

        private bool isWeedingHoeInPlayerHands;
        private int currentFlowersThatNeedWeedingQuantity;

        private protected override void Awake()
        {
            base.Awake();
            
            Load();
        }

        private protected override void TryInteractWithTable()
        {
            base.TryInteractWithTable();

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
            if (playerPickableObjectHandler.CurrentPickableObject is WeedingHoe currentWeedingHoe)
            {
                if (!currentWeedingHoe.Equals(weedingHoe))
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.MismatchGrowingRoom);
                }
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer)
            {
                if (tableLvl >= repairsAndUpgradesSettings.MaxUpgradableTableLvl)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.TableHasMaxLvl);
                }
            }
            else
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.WrongPickableObject);
            }
        }

        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerTakeHoeInHandsForSelectableEffect() || CanPlayerUseTableInfoCanvas() ||
                   CanPlayerUpgradeTableForSelectableEffect() || CanPlayerPutHoeOnTable();
        }

        public override void UpgradeTableFinish()
        {
            base.UpgradeTableFinish();
            
            weedingHoe.Upgrade(tableLvl);
            
            Save();
        }

        public void IncreaseFlowersThatNeedWeedingQuantity()
        {
            currentFlowersThatNeedWeedingQuantity++;
        }

        public void DecreaseFlowersThatNeedWeedingQuantity()
        {
            currentFlowersThatNeedWeedingQuantity--;
        }

        public void Load()
        {
            WeedingTableForSaving weedingTableForLoading = SavesHandler.Load<WeedingTableForSaving>(UniqueKey);

            if (weedingTableForLoading.IsValuesSaved)
            {
                tableLvl = weedingTableForLoading.TableLvl;
                if (tableLvl > 0)
                {
                    LoadLvlMesh();
                    weedingHoe.Upgrade(tableLvl);
                }

                if (weedingTableForLoading.IsWeedingHoeInPlayerHands)
                {
                    isWeedingHoeInPlayerHands = true;
                    weedingHoe.LoadInPlayerHands();
                }
            }
        }

        public void Save()
        {
            WeedingTableForSaving weedingTableForSaving = new(tableLvl, isWeedingHoeInPlayerHands);
            
            SavesHandler.Save(UniqueKey, weedingTableForSaving);
        }

        private bool CanPlayerTakeHoeInHandsForSelectableEffect()
        {
            return currentFlowersThatNeedWeedingQuantity > 0 && CanPlayerTakeHoeInHands();
        }

        private bool CanPlayerTakeHoeInHands()
        {
            return playerPickableObjectHandler.IsPickableObjectNull;
        }

        private void TakeHoeInPlayerHands()
        {
            isWeedingHoeInPlayerHands = true;
            weedingHoe.TakeInPlayerHandsAndSetPlayerFree();
            
            Save();
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
            isWeedingHoeInPlayerHands = false;
            playerPickableObjectHandler.ResetPickableObject();
            weedingHoe.PutOnTableAndSetPlayerFree(hoeOnTableTransform);
            
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
    }
}
