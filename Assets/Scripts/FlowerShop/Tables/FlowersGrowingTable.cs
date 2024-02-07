using System.Runtime.InteropServices;
using FlowerShop.Fertilizers;
using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Sounds;
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
        [Inject] private readonly PlayerAnimationEvents playerAnimationEvents;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly ReferencesForLoad referencesForLoad;
        [Inject] private readonly SoundsHandler soundsHandler;
        [Inject] private readonly TablesSettings tablesSettings;
    
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
            if (potOnTable && tableLvl > 0)
            {
                flowersGrowingTableEffects.StartFansRotation();
            }
            
            breakableTableBaseComponent.CheckIfTableBroken();
        }

        public override void ExecuteClickableAbility()
        {
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
            }
        }

        public override void UpgradeTableFinish()
        {
            base.UpgradeTableFinish();
            flowersGrowingTableEffects.SetFlowersGrowingTableLvlForEffects(tableLvl);

            if (isPotOnTable)
            {
                if (tableLvl == tablesSettings.FansEnableLvl)
                {
                    soundsHandler.StartPlayingGrowingTableFansAudio();
                }
                flowersGrowingTableEffects.EnableEffects();
                potOnTable.CalculateUpGrowingLvlTimeOnTableUpgrade(tableLvl);
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

            if (potOnTable)
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

                return potOnTable.GrowingRoom == growingRoom &&
                       potOnTable.PlantedFlowerInfo.FlowerName != flowersSettings.FlowerNameEmpty &&
                       potOnTable.FlowerGrowingLvl < flowersSettings.MaxFlowerGrowingLvl;
            }

            return false;
        }

        private void PutPotOnTable()
        {
            potOnTable.PutOnGrowingTableAndSetPlayerFree(tablePotTransform, tableLvl);
            playerPickableObjectHandler.ResetPickableObject();
            flowersGrowingTableEffects.StartFansRotation();
            PutPotOnTableBase();
            
            Save();
        }

        private void PutPotOnTableBase()
        {
            isPotOnTable = true;
            flowersGrowingTableEffects.EnableEffects();

            if (!potOnTable.IsWeedInPot)
            {
                weedPlanter.AddPotInPlantingWeedList(potOnTable);
            }

            if (tableLvl > 0)
            {
                soundsHandler.StartPlayingGrowingTableFansAudio();
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
            playerAnimationEvents.SetCurrentAnimationEvent(potOnTable.PourFlower);
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
            flowersGrowingTableEffects.StopFansRotation();
            flowersGrowingTableEffects.DisableEffects();
            
            if (tableLvl > 0)
            {
                soundsHandler.StopPlayingGrowingTableFansAudio();
            }
            
            Save();
        }

        private bool CanPlayerFixTable()
        {
            return playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer &&
                   IsTableBroken;
        }

        private void FixFlowerGrowingTable()
        {
            FixBreakableFlowerTable(
                repairsAndUpgradesSettings.FlowerGrowingTableMinQuantity * (tableLvl + 1),
                repairsAndUpgradesSettings.FlowerGrowingTableMaxQuantity * (tableLvl + 1));
            
            Save();
        }

        private bool CanPlayerUpgradeTable()
        {
            return playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer &&
                   tableLvl < repairsAndUpgradesSettings.MaxUpgradableTableLvl;
        }
    }
}
