using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using FlowerShop.Tables.Abstract;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class FlowersCrossingTable : Table
    {
        [Inject] private readonly FlowersSettings flowersSettings;
        
        [SerializeField] private Transform tablePotTransform;
        [SerializeField] private FlowersCrossingTableProcess flowersCrossingTableProcess;

        public Pot PotOnTable { get; private set; }
        public bool IsPotOnCrossingTable { get; private set; }

        public override void ExecuteClickableAbility()
        {
            if (playerBusyness.IsPlayerFree)
            {
                if (CanPlayerTakePotInHands())
                {
                    SetPlayerDestination();
                    
                    ResetOnPlayerArriveEvent();
                    OnPlayerArriveEvent += TakePotInPlayerHands;
                }
                else if (CanPlayerPutPotOnTable())
                {
                    SetPlayerDestination();
                    
                    ResetOnPlayerArriveEvent();
                    OnPlayerArriveEvent += PutPotOnTable;
                }
            }
        }

        public override void ExecutePlayerAbility()
        {
            base.ExecutePlayerAbility();
            
            flowersCrossingTableProcess.CheckCrossingAbility();
        }

        private bool CanPlayerTakePotInHands()
        {
            return IsPotOnCrossingTable &&
                   !flowersCrossingTableProcess.IsSeedCrossing &&
                   playerPickableObjectHandler.IsPickableObjectNull;
        }

        private bool CanPlayerPutPotOnTable()
        {
            if (playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
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
        }

        private void TakePotInPlayerHands()
        {
            PotOnTable.TakeInPlayerHandsAndSetPlayerFree();
            IsPotOnCrossingTable = false;
        }
    }
}
