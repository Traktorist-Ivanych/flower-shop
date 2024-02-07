using System.Collections;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Settings;
using FlowerShop.Sounds;
using FlowerShop.Tables.Abstract;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class WateringTable : UpgradableBreakableTable, ISavableObject
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly SoundsHandler soundsHandler;
        
        [SerializeField] private Transform wateringCanTableTransform;
        [SerializeField] private WateringCan wateringCan;
        [SerializeField] private ParticleSystem waterPS;

        private bool isWateringCanInPlayerHands;
        
        [field: SerializeField] public string UniqueKey { get; private set; }

        private protected override void Awake()
        {
            base.Awake();
            
            Load();
        }

        private void Start()
        {
            if (isWateringCanInPlayerHands)
            {
                wateringCan.LoadInPlayerHands();
            }
            else if (wateringCan.IsWateringCanNeedForReplenish())
            {
                StartCoroutine(ReplenishWateringCan());
            }
            
            breakableTableBaseComponent.CheckIfTableBroken();
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

        public override void UpgradeTableFinish()
        {
            base.UpgradeTableFinish();
            
            wateringCan.UpgradeWateringCan(tableLvl);
            
            SetActionsBeforeBrokenQuantity(
                repairsAndUpgradesSettings.WateringTableMinQuantity * (tableLvl + 1),
                repairsAndUpgradesSettings.WateringTableMaxQuantity * (tableLvl + 1));
            
            Save();
        }

        public void Load()
        {
            WateringTableForSaving wateringTableForLoading = SavesHandler.Load<WateringTableForSaving>(UniqueKey);

            if (wateringTableForLoading.IsValuesSaved)
            {
                if (wateringTableForLoading.TableLvl > 0)
                {
                    tableLvl = wateringTableForLoading.TableLvl;
                    LoadLvlMesh();
                }
                
                isWateringCanInPlayerHands = wateringTableForLoading.IsWateringCanInPlayerHands;
                
                breakableTableBaseComponent.LoadActionsBeforeBrokenQuantity(
                    wateringTableForLoading.ActionsBeforeBrokenQuantity);

            }
            else
            {
                SetActionsBeforeBrokenQuantity(
                    repairsAndUpgradesSettings.WateringTableMinQuantity * (tableLvl + 1),
                    repairsAndUpgradesSettings.WateringTableMaxQuantity * (tableLvl + 1));
            }
        }

        public void Save()
        {
            WateringTableForSaving wateringTableForSaving = new(
                tableLvl: tableLvl, 
                actionsBeforeBrokenQuantity: breakableTableBaseComponent.ActionsBeforeBrokenQuantity, 
                currentWateringsNumber: wateringCan.CurrentWateringsNumber,
                isWateringCanInPlayerHands: isWateringCanInPlayerHands);
            
            SavesHandler.Save(UniqueKey, wateringTableForSaving);
        }

        private bool CanPlayerTakeWateringCanInHands()
        {
            return playerPickableObjectHandler.IsPickableObjectNull &&
                   !wateringCan.IsWateringCanNeedForReplenish() && !IsTableBroken;
        }

        private void TakeWateringCanInPlayerHands()
        {
            wateringCan.TakeInPlayerHandsAndSetPlayerFree();

            isWateringCanInPlayerHands = true;
            
            Save();
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
            isWateringCanInPlayerHands = false;

            if (wateringCan.IsWateringCanNeedForReplenish())
            {
                StartCoroutine(ReplenishWateringCan());
            }
            
            Save();
        }

        private IEnumerator ReplenishWateringCan()
        {
            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            waterPS.Play();
            soundsHandler.StartPlayingReplenishWateringCanAudio();
            yield return new WaitForSeconds(wateringCan.ReplenishWateringCanTime());
            waterPS.Stop();
            soundsHandler.StopPlayingReplenishWateringCanAudio();
            wateringCan.ReplenishWateringCan();
            UseBreakableTable();
            
            Save();
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
            
            Save();
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
