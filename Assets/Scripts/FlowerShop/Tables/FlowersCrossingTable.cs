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
                    SetPlayerDestinationAndOnPlayerArriveAction(TakePotInPlayerHands);
                }
                else if (CanPlayerPutPotOnTable())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(PutPotOnTable);
                }
            }
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
            flowersCrossingTableProcess.CheckCrossingAbility();
            PotOnTable.PutOnTableAndSetPlayerFree(tablePotTransform);
            playerPickableObjectHandler.ResetPickableObject();
            IsPotOnCrossingTable = true;
        }

        private void TakePotInPlayerHands()
        {
            flowersCrossingTableProcess.CheckCrossingAbility();
            PotOnTable.TakeInPlayerHandsAndSetPlayerFree();
            IsPotOnCrossingTable = false;
        }
    }
}
