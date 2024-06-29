using System.Collections;
using FlowerShop.Achievements;
using FlowerShop.Fertilizers;
using FlowerShop.Flowers;
using FlowerShop.Help;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Tables.Abstract;
using FlowerShop.Tables.Helpers;
using FlowerShop.Weeds;
using PlayerControl;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    [RequireComponent(typeof(FlowersGrowingTableEffects))]
    public class FlowersGrowingTable : UpgradableBreakableTable, ISavableObject
    {
        [Inject] private readonly PlantGrowingPlant plantGrowingPlant;
        [Inject] private readonly PlayerAnimationEvents playerAnimationEvents;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly FertilizersSetting fertilizersSetting;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly HelpCanvasLiaison helpCanvasLiaison;
        [Inject] private readonly HelpTexts helpTexts;
        [Inject] private readonly ReferencesForLoad referencesForLoad;
    
        [SerializeField] private Transform tablePotTransform;
        [SerializeField] private WeedPlanter weedPlanter;

        [HideInInspector, SerializeField] private FlowersGrowingTableEffects flowersGrowingTableEffects;

        private Fertilizer fertilizer;
        private WateringCan wateringCan;
        private WeedingHoe weedingHoe;
        private Pot potOnTable;
        private bool isPotOnTable;

        [field: SerializeField] public string UniqueKey { get; private set; }

        private protected override void OnValidate()
        {
            base.OnValidate();
            
            flowersGrowingTableEffects = GetComponent<FlowersGrowingTableEffects>();
        }

        private protected override void Awake()
        {
            base.Awake();
            
            Load();
        }

        private void Start()
        {
            if (isPotOnTable && potOnTable &&
                potOnTable.FlowerGrowingLvl < flowersSettings.MaxFlowerGrowingLvl)
            {
                flowersGrowingTableEffects.EnableEffects();
            }
            
            breakableTableBaseComponent.CheckIfTableBroken();
        }

        private protected override void TryInteractWithTable()
        {
            base.TryInteractWithTable();

            if (playerBusyness.IsPlayerFree)
            {
                if (CanPlayerPutPotOnTable())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(PutPotOnTable);
                }
                else if (CanPlayerPourPotOnTable())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(PourPotOnTable);
                }
                else if (CanPlayerDeleteWeedInPot())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(DeleteWeedInPot);
                }
                else if (CanPlayerUseFertilizer())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(UseFertilizer);
                }
                else if (CanPlayerTakePotInHands())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(TakePotInPlayerHands);
                }
                else if (CanPlayerFixTable())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(FixFlowerGrowingTable);
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
            else if (playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                if (currentPot.GrowingRoom != growingRoom)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.MismatchGrowingRoom);
                }
                else if (isPotOnTable)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.TableAlreadyHasPot);
                }
                else if (potOnTable.PlantedFlowerInfo.FlowerName == flowersSettings.FlowerNameEmpty)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.NoFlowerPlanted);
                }
                else if (potOnTable.FlowerGrowingLvl >= flowersSettings.MaxFlowerGrowingLvl)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.FlowerAlreadyGrown);
                }
                else if (currentPot.PlantedFlowerInfo.FlowerLvl == flowersSettings.MediumFlowerLvlForTableLvl &&
                    tableLvl < flowersSettings.MediumTableLvlForFlowerLvl)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.InconsistencyFlowerAndGrowingTableLvl);
                }
                else if (currentPot.PlantedFlowerInfo.FlowerLvl == flowersSettings.MaxFlowerLvlForTableLvl &&
                    tableLvl < flowersSettings.MaxTableLvlForFlowerLvl)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.InconsistencyFlowerAndGrowingTableLvl);
                }
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is WateringCan currentWateringCan)
            {
                if (currentWateringCan.GrowingRoom != growingRoom)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.MismatchGrowingRoom);
                }
                else if (!isPotOnTable)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.EmptyTable);
                }
                else if (!potOnTable.IsFlowerNeedWater)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.FlowerDoesNotNeedWatering);
                }
                else if (currentWateringCan.CurrentWateringsNumber <= 0)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.EmptyWateringCan);
                }
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is WeedingHoe currentWeedingHoe)
            {
                if (currentWeedingHoe.GrowingRoom != growingRoom)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.MismatchGrowingRoom);
                }
                else if (!isPotOnTable)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.EmptyTable);
                }
                else if (!potOnTable.IsWeedInPot)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.NoWeed);
                }
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is Fertilizer currentFertilizer)
            {
                if (!isPotOnTable)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.EmptyTable);
                }
                else if (currentFertilizer.AvailableUsesNumber <= 0)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.FertilizersAreOut);
                }
                else if (potOnTable.FlowerGrowingLvl >= flowersSettings.MaxFlowerGrowingLvl)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.FlowerAlreadyGrown);
                }
                else if (potOnTable.IsPotTreatedByGrothAccelerator)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.FlowerAlreadyProcessed);
                }
            }
            else if (playerPickableObjectHandler.IsPickableObjectNull)
            {
                if (!isPotOnTable)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.EmptyTable);
                }
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer)
            {
                if (tableLvl == repairsAndUpgradesSettings.MaxUpgradableTableLvl)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.TableHasMaxLvl);
                }
            }
        }

        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerPutPotOnTable() || CanPlayerPourPotOnTable() || 
                   CanPlayerDeleteWeedInPot() || CanPlayerUseFertilizer() || 
                   CanPlayerFixTable() || CanPlayerUpgradeTableForSelectableEffect() ||
                   CanPlayerTakePotInHandsForSelectedEffect() || CanPlayerUseTableInfoCanvas();
        }

        public override void UpgradeTableFinish()
        {
            base.UpgradeTableFinish();
            flowersGrowingTableEffects.SetFlowersGrowingTableLvlForEffects(tableLvl);

            if (isPotOnTable)
            {
                potOnTable.CalculateUpGrowingLvlTimeOnTableUpgrade(tableLvl);
                if (potOnTable.FlowerGrowingLvl >= flowersSettings.MaxFlowerGrowingLvl)
                {
                    flowersGrowingTableEffects.DisableEffects();
                }
            }
            
            SetActionsBeforeBrokenQuantity(
                repairsAndUpgradesSettings.FlowerGrowingTableMinQuantity * (tableLvl + 1),
                repairsAndUpgradesSettings.FlowerGrowingTableMaxQuantity * (tableLvl + 1));
            
            Save();
        }

        public void Load()
        {
            FlowersGrowingTableForSaving flowersGrowingTableForLoading =
                SavesHandler.Load<FlowersGrowingTableForSaving>(UniqueKey);

            if (flowersGrowingTableForLoading.IsValuesSaved)
            {
                tableLvl = flowersGrowingTableForLoading.TableLvl;
                if (tableLvl > 0)
                {
                    LoadLvlMesh();
                    flowersGrowingTableEffects.SetFlowersGrowingTableLvlForEffects(tableLvl);
                }
                
                potOnTable = referencesForLoad.GetReference<Pot>(flowersGrowingTableForLoading.PotUniqueKey);
                
                if (potOnTable != null)
                {
                    potOnTable.LoadOnGrowingTable(tablePotTransform, tableLvl);
                    PutPotOnTableBase();
                }
                
                breakableTableBaseComponent.LoadActionsBeforeBrokenQuantity(
                    flowersGrowingTableForLoading.ActionsBeforeBrokenQuantity);
            }
            else
            {
                SetActionsBeforeBrokenQuantity(
                    repairsAndUpgradesSettings.FlowerGrowingTableMinQuantity * (tableLvl + 1),
                    repairsAndUpgradesSettings.FlowerGrowingTableMaxQuantity * (tableLvl + 1));
            }
        }

        public void Save()
        {
            string potOnTableUniqueKey = "Empty";

            if (isPotOnTable && potOnTable)
            {
                potOnTableUniqueKey = potOnTable.UniqueKey;
            }
            
            FlowersGrowingTableForSaving flowersGrowingTableForSaving =
                new(potOnTableUniqueKey, tableLvl, breakableTableBaseComponent.ActionsBeforeBrokenQuantity);
            
            SavesHandler.Save(UniqueKey, flowersGrowingTableForSaving);
        }
        
        private bool CanPlayerPutPotOnTable()
        {
            if (!IsTableBroken && !isPotOnTable &&
                playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                potOnTable = currentPot;

                if (potOnTable.PlantedFlowerInfo.FlowerName == flowersSettings.FlowerNameEmpty)
                {
                    return false;
                }
                else
                {
                    if (potOnTable.PlantedFlowerInfo.FlowerLvl == flowersSettings.MediumFlowerLvlForTableLvl &&
                        tableLvl < flowersSettings.MediumTableLvlForFlowerLvl)
                    {
                        return false;
                    }
                    else if (potOnTable.PlantedFlowerInfo.FlowerLvl == flowersSettings.MaxFlowerLvlForTableLvl &&
                        tableLvl < flowersSettings.MaxTableLvlForFlowerLvl)
                    {
                        return false;
                    }
                    else
                    {
                        return potOnTable.GrowingRoom == growingRoom &&
                               potOnTable.FlowerGrowingLvl < flowersSettings.MaxFlowerGrowingLvl;
                    }
                }
            }

            return false;
        }

        private void PutPotOnTable()
        {
            potOnTable.PutOnGrowingTableAndSetPlayerFree(tablePotTransform, tableLvl);
            playerPickableObjectHandler.ResetPickableObject();
            flowersGrowingTableEffects.EnableEffects();
            PutPotOnTableBase();
            plantGrowingPlant.IncreaseProgress();
            
            Save();
        }

        private bool CanPlayerUseTableInfoCanvas()
        {
            return !IsTableBroken && playerPickableObjectHandler.CurrentPickableObject is InfoBook;
        }

        private void UseTableInfoCanvas()
        {
            tableInfoCanvasLiaison.ShowCanvas(tableInfo, growingRoom);
        }

        private void PutPotOnTableBase()
        {
            isPotOnTable = true;

            if (!potOnTable.IsWeedInPot)
            {
                weedPlanter.AddPotInPlantingWeedList(potOnTable);
            }
        }

        private bool CanPlayerPourPotOnTable()
        {
            if (!IsTableBroken && isPotOnTable && potOnTable.IsFlowerNeedWater &&
                playerPickableObjectHandler.CurrentPickableObject is WateringCan currentWateringCan)
            {
                wateringCan = currentWateringCan;

                return wateringCan.GrowingRoom == growingRoom && wateringCan.CurrentWateringsNumber > 0;
            }

            return false;
        }

        private void PourPotOnTable()
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.PourTrigger);
            potOnTable.HideWaterIndicator();
            playerAnimationEvents.SetCurrentAnimationEvents(potOnTable.PourFlower, TryDisableTableEffects);
        }

        private bool CanPlayerDeleteWeedInPot()
        {
            if (isPotOnTable && potOnTable.IsWeedInPot &&
                playerPickableObjectHandler.CurrentPickableObject is WeedingHoe currentWeedingHoe)
            {
                weedingHoe = currentWeedingHoe;

                return weedingHoe.GrowingRoom == growingRoom;
            }

            return false;
        }

        private void DeleteWeedInPot()
        {
            StartCoroutine(weedingHoe.DeleteWeed(potOnTable, weedPlanter));
        }

        private bool CanPlayerUseFertilizer()
        {
            if (!IsTableBroken && isPotOnTable &&
                playerPickableObjectHandler.CurrentPickableObject is Fertilizer currentFertilizer)
            {
                fertilizer = currentFertilizer;
                return fertilizer.AvailableUsesNumber > 0 &&
                       !potOnTable.IsPotTreatedByGrothAccelerator &&
                       potOnTable.FlowerGrowingLvl < flowersSettings.MaxFlowerGrowingLvl;
            }

            return false;
        }

        private void UseFertilizer()
        {
            StartCoroutine(fertilizer.PotTreating(potOnTable));
            StartCoroutine(TryDisableEffectsAfterTreating());
        }

        private IEnumerator TryDisableEffectsAfterTreating()
        {
            yield return new WaitForSeconds(fertilizersSetting.FertilizerTreatingTime + 0.1f);
            TryDisableTableEffects();
        }

        private bool CanPlayerTakePotInHandsForSelectedEffect()
        {
            return CanPlayerTakePotInHands() && 
                   potOnTable.FlowerGrowingLvl >= flowersSettings.MaxFlowerGrowingLvl;
        }

        private bool CanPlayerTakePotInHands()
        {
            return !IsTableBroken && isPotOnTable && playerPickableObjectHandler.IsPickableObjectNull;
        }

        private void TakePotInPlayerHands()
        {
            weedPlanter.RemovePotFormPlantingWeedList(potOnTable);
            potOnTable.TakeInPlayerHandsFromGrowingTableAndSetPlayerFree();
            potOnTable = null;
            isPotOnTable = false;
            UseBreakableTable();
            flowersGrowingTableEffects.DisableEffects();
            plantGrowingPlant.DecreaseAchievementProgress();
            
            Save();
        }

        private void FixFlowerGrowingTable()
        {
            FixBreakableFlowerTable(
                repairsAndUpgradesSettings.FlowerGrowingTableMinQuantity * (tableLvl + 1),
                repairsAndUpgradesSettings.FlowerGrowingTableMaxQuantity * (tableLvl + 1));
        }

        private void TryDisableTableEffects()
        {
            if (potOnTable.FlowerGrowingLvl >= flowersSettings.MaxFlowerGrowingLvl)
            {
                flowersGrowingTableEffects.DisableEffects();
            }
        }



        public void Load(FlowersGrowingTableForSaving flowersGrowingTableForLoading) // CutScene
        {
            if (flowersGrowingTableForLoading.IsValuesSaved)
            {
                tableLvl = flowersGrowingTableForLoading.TableLvl;
                if (tableLvl > 0)
                {
                    LoadLvlMesh();
                    flowersGrowingTableEffects.SetFlowersGrowingTableLvlForEffects(tableLvl);
                }

                potOnTable = referencesForLoad.GetReference<Pot>(flowersGrowingTableForLoading.PotUniqueKey);

                if (potOnTable != null)
                {
                    potOnTable.LoadOnGrowingTable(tablePotTransform, tableLvl);
                    PutPotOnTableBase();
                }

                breakableTableBaseComponent.LoadActionsBeforeBrokenQuantity(
                    flowersGrowingTableForLoading.ActionsBeforeBrokenQuantity);
            }
            else
            {
                SetActionsBeforeBrokenQuantity(
                    repairsAndUpgradesSettings.FlowerGrowingTableMinQuantity * (tableLvl + 1),
                    repairsAndUpgradesSettings.FlowerGrowingTableMaxQuantity * (tableLvl + 1));
            }
        }
    }
}
