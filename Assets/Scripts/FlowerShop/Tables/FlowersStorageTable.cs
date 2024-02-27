using FlowerShop.Fertilizers;
using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Tables.Abstract;
using FlowerShop.Weeds;
using PlayerControl;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class FlowersStorageTable : Table, ISavableObject
    {
        [Inject] private readonly PlayerAnimationEvents playerAnimationEvents;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly ReferencesForLoad referencesForLoad;
        [Inject] private readonly FlowersSettings flowersSettings;
        
        [SerializeField] private Transform tablePotTransform;
        [SerializeField] private WeedPlanter weedPlanter;

        private WeedingHoe weedingHoe;
        private WateringCan wateringCan;
        private Fertilizer fertilizer;
        private Pot potOnTable;
        private bool isPotOnTable;
        
        [field: SerializeField] public string UniqueKey { get; private set; }

        private void Awake()
        {
            Load();
        }

        private void Start()
        {
            if (potOnTable != null)
            {
                TryAddPotOnTableInPlantingWeedList();
            }
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
            }
        }

        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerPutPotOnTable() || CanPlayerPourPotOnTable() ||
                   CanPlayerDeleteWeedInPot() || CanPlayerUseFertilizer();
        }

        public void Load()
        {
            FlowersStorageTableForSaving flowersStorageTableForLoading =
                SavesHandler.Load<FlowersStorageTableForSaving>(UniqueKey);

            if (flowersStorageTableForLoading.IsValuesSaved)
            {
                potOnTable = referencesForLoad.GetReference<Pot>(flowersStorageTableForLoading.PotUniqueKey);
                
                if (potOnTable != null)
                {
                    potOnTable.LoadOnTable(tablePotTransform);
                    isPotOnTable = true;
                }
            }
        }

        public void Save()
        {
            FlowersStorageTableForSaving flowersStorageTableForSaving = new(potOnTable.UniqueKey);
            
            SavesHandler.Save(UniqueKey, flowersStorageTableForSaving);
        }

        private bool CanPlayerPutPotOnTable()
        {
            if (!isPotOnTable && playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                potOnTable = currentPot;

                return potOnTable.GrowingRoom == growingRoom || growingRoom == flowersSettings.GrowingRoomAny;
            }

            return false;
        }

        private void PutPotOnTable()
        {
            potOnTable.PutOnTableAndSetPlayerFree(tablePotTransform);
            playerPickableObjectHandler.ResetPickableObject();
            
            isPotOnTable = true;
            TryAddPotOnTableInPlantingWeedList();
            
            Save();
        }

        private bool CanPlayerPourPotOnTable()
        {
            if (isPotOnTable &&
                playerPickableObjectHandler.CurrentPickableObject is WateringCan currentWateringCan)
            {
                wateringCan = currentWateringCan;

                return wateringCan.GrowingRoom == potOnTable.GrowingRoom && 
                       wateringCan.CurrentWateringsNumber > 0 && 
                       potOnTable.IsFlowerNeedWater;
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
            if (isPotOnTable &&
                playerPickableObjectHandler.CurrentPickableObject is WeedingHoe currentWeedingHoe)
            {
                weedingHoe = currentWeedingHoe;

                return potOnTable.IsWeedInPot && weedingHoe.GrowingRoom == potOnTable.GrowingRoom;
            }

            return false;
        }

        private void DeleteWeedInPot()
        {
            StartCoroutine(weedingHoe.DeleteWeed(potOnTable, weedPlanter));
        }

        private bool CanPlayerUseFertilizer()
        {
            if (isPotOnTable &&
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
            return isPotOnTable && playerPickableObjectHandler.IsPickableObjectNull;
        }

        private void TakePotInPlayerHands()
        {
            potOnTable.TakeInPlayerHandsAndSetPlayerFree();
            isPotOnTable = false;
            weedPlanter.RemovePotFormPlantingWeedList(potOnTable);
            potOnTable = null;

            SavesHandler.DeletePlayerPrefsKey(UniqueKey);
        }

        private void TryAddPotOnTableInPlantingWeedList()
        {
            if (potOnTable.IsSoilInsidePot && !potOnTable.IsWeedInPot)
            {
                weedPlanter.AddPotInPlantingWeedList(potOnTable);
            }
        }
    }
}
