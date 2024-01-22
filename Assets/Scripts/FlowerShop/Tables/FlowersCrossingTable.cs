using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Tables.Abstract;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class FlowersCrossingTable : Table, ISavableObject
    {
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly ReferencesForLoad referencesForLoad;
        
        [SerializeField] private Transform tablePotTransform;
        [SerializeField] private FlowersCrossingTableProcess flowersCrossingTableProcess;
        
        [field: SerializeField] public string UniqueKey { get; private set; }
        
        public Pot PotOnTable { get; private set; }
        public bool IsPotOnCrossingTable { get; private set; }

        private void Awake()
        {
            Load();
        }

        private void Start()
        {
            flowersCrossingTableProcess.CheckCrossingAbility();
        }

        public override void ExecuteClickableAbility()
        {
            if (playerBusyness.IsPlayerFree)
            { 
                if (CanPlayerPutPotOnTable())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(PutPotOnTable);
                }
                else if (CanPlayerTakePotInHands())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(TakePotInPlayerHands);
                }
            }
        }

        public void Load()
        {
            FlowersCrossingTableForSaving flowersCrossingTableForLoading =
                SavesHandler.Load<FlowersCrossingTableForSaving>(UniqueKey);

            if (flowersCrossingTableForLoading.IsValuesSaved)
            {
                PotOnTable = referencesForLoad.GetReference<Pot>(flowersCrossingTableForLoading.PotUniqueKey);

                if (PotOnTable != null)
                {
                    PotOnTable.LoadOnTable(tablePotTransform);
                    IsPotOnCrossingTable = true;
                }
            }
        }

        public void Save()
        {
            string potOnTableUniqueKey = "Empty";

            if (PotOnTable)
            {
                potOnTableUniqueKey = PotOnTable.UniqueKey;
            }
            
            FlowersCrossingTableForSaving flowersCrossingTableForSaving = new(potOnTableUniqueKey);
            
            SavesHandler.Save(UniqueKey, flowersCrossingTableForSaving);
        }

        private bool CanPlayerPutPotOnTable()
        {
            if (!IsPotOnCrossingTable && playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                PotOnTable = currentPot;

                return PotOnTable.GrowingRoom == growingRoom && 
                       PotOnTable.FlowerGrowingLvl >= flowersSettings.MaxFlowerGrowingLvl &&
                       !PotOnTable.IsWeedInPot && 
                       !flowersCrossingTableProcess.IsTableBroken;
            }
            
            return false;
        }

        private void PutPotOnTable()
        {
            PotOnTable.PutOnTableAndSetPlayerFree(tablePotTransform);
            playerPickableObjectHandler.ResetPickableObject();
            IsPotOnCrossingTable = true;
            flowersCrossingTableProcess.CheckCrossingAbility();
            
            Save();
        }

        private bool CanPlayerTakePotInHands()
        {
            return IsPotOnCrossingTable &&
                   !flowersCrossingTableProcess.IsSeedCrossing &&
                   playerPickableObjectHandler.IsPickableObjectNull;
        }

        private void TakePotInPlayerHands()
        {
            IsPotOnCrossingTable = false;
            flowersCrossingTableProcess.CheckCrossingAbility();
            PotOnTable.TakeInPlayerHandsAndSetPlayerFree();
            PotOnTable = null;
            
            SavesHandler.DeletePlayerPrefsKey(UniqueKey);
        }
    }
}
