using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Tables.Abstract;
using Saves;
using UnityEngine;

namespace FlowerShop.Tables
{
    public class WeedingTable : UpgradableTable, ISavableObject
    {
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

        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerTakeHoeInHandsForSelectableEffect() ||
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
    }
}
