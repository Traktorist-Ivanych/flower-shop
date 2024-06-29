using System.Collections;
using FlowerShop.Effects;
using FlowerShop.Help;
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
        [Inject] private readonly HelpCanvasLiaison helpCanvasLiaison;
        [Inject] private readonly HelpTexts helpTexts;
        [Inject] private readonly SoundsHandler soundsHandler;
        
        [SerializeField] private Transform wateringCanTableTransform;
        [SerializeField] private WateringCan wateringCan;
        [SerializeField] private ParticleSystem waterPS;

        [HideInInspector, SerializeField] private ActionProgressbar actionProgressbar;

        private bool isWateringCanInPlayerHands;
        private int currentFlowersThatNeedWaterQuantity;
        
        [field: SerializeField] public string UniqueKey { get; private set; }
        private protected override void OnValidate()
        {
            base.OnValidate();

            actionProgressbar = GetComponentInChildren<ActionProgressbar>();
        }

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

        private protected override void TryInteractWithTable()
        {
            base.TryInteractWithTable();

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
                else if (CanPlayerFixTable())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(FixWateringTable);
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
            if (IsTableBroken)
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.BrokenTable);
            }
            else if (playerPickableObjectHandler.IsPickableObjectNull)
            {
                if (wateringCan.IsWateringCanNeedForReplenish())
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.WateringCanReplenish);
                }
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is WateringCan currentWateringCan)
            {
                if (!currentWateringCan.Equals(wateringCan))
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
            return CanPlayerTakeWateringCanInHandsForSelectableEffect() || 
                   CanPlayerFixTable() || CanPlayerUpgradeTableForSelectableEffect() || 
                   CanPlayerPutWateringCanOnTable() || CanPlayerUseTableInfoCanvas();
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

        public void IncreaseFlowersThatNeedWaterQuantity()
        {
            currentFlowersThatNeedWaterQuantity++;
        }

        public void DecreaseFlowersThatNeedWaterQuantity()
        {
            currentFlowersThatNeedWaterQuantity--;
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

        private bool CanPlayerTakeWateringCanInHandsForSelectableEffect()
        {
            return currentFlowersThatNeedWaterQuantity > 0 && CanPlayerTakeWateringCanInHands();
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

            float currentReplenishTime = wateringCan.ReplenishWateringCanTime();

            actionProgressbar.EnableActionProgressbar(currentReplenishTime);
            yield return new WaitForSeconds(wateringCan.ReplenishWateringCanTime());
            waterPS.Stop();
            soundsHandler.StopPlayingReplenishWateringCanAudio();
            wateringCan.ReplenishWateringCan();
            UseBreakableTable();
            selectedTableEffect.TryToRecalculateEffect();
            
            Save();
        }

        private void FixWateringTable()
        {
            FixBreakableFlowerTable(
                repairsAndUpgradesSettings.WateringTableMinQuantity * (tableLvl + 1),
                repairsAndUpgradesSettings.WateringTableMaxQuantity * (tableLvl + 1));
        }

        private bool CanPlayerUseTableInfoCanvas()
        {
            return !IsTableBroken && playerPickableObjectHandler.CurrentPickableObject is InfoBook;
        }

        private void UseTableInfoCanvas()
        {
            tableInfoCanvasLiaison.ShowCanvas(tableInfo, growingRoom);
        }

        

        public void Load(WateringTableForSaving wateringTableForLoading) // CutScene
        {
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
    }
}
