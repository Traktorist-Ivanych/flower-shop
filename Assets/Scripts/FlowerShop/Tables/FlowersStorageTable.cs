using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using FlowerShop.Tables.Abstract;
using FlowerShop.Weeds;
using UnityEngine;

namespace FlowerShop.Tables
{
    public class FlowersStorageTable : Table
    {
        [SerializeField] private Transform tablePotTransform;
        [SerializeField] private WeedPlanter weedPlanter;
        [SerializeField] private GrowingRoom growingRoomAny;

        private WeedingHoe weedingHoe;
        private WateringCan wateringCan;
        private Pot potOnTable;
        private bool isFlowerOnStorageTable;

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
                else if (CanPlayerTakePotInHands())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(TakePotInPlayerHands);
                }
            }
        }

        private bool CanPlayerPutPotOnTable()
        {
            if (!isFlowerOnStorageTable && playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                potOnTable = currentPot;

                return potOnTable.GrowingRoom == growingRoom || growingRoom == growingRoomAny;
            }

            return false;
        }

        private void PutPotOnTable()
        {
            potOnTable.PutOnTableAndSetPlayerFree(tablePotTransform);
            playerPickableObjectHandler.ResetPickableObject();
            isFlowerOnStorageTable = true;
            if (potOnTable.IsSoilInsidePot)
            {
                weedPlanter.AddPotInPlantingWeedList(potOnTable);
            }
        }

        private bool CanPlayerPourPotOnTable()
        {
            if (isFlowerOnStorageTable &&
                playerPickableObjectHandler.CurrentPickableObject is WateringCan currentWateringCan)
            {
                wateringCan = currentWateringCan;

                return wateringCan.GrowingRoom == growingRoom && wateringCan.CurrentWateringsNumber > 0 &&
                       potOnTable.IsFlowerNeedWater;
            }

            return false;
        }

        private void PourPotOnTable()
        {
            potOnTable.PourFlower();
        }

        private bool CanPlayerDeleteWeedInPot()
        {
            if (isFlowerOnStorageTable &&
                playerPickableObjectHandler.CurrentPickableObject is WeedingHoe currentWeedingHoe)
            {
                weedingHoe = currentWeedingHoe;

                return potOnTable.IsWeedInPot && weedingHoe.GrowingRoom == growingRoom;
            }

            return false;
        }

        private void DeleteWeedInPot()
        {
            StartCoroutine(weedingHoe.DeleteWeed(potOnTable, weedPlanter));
        }

        private bool CanPlayerTakePotInHands()
        {
            return isFlowerOnStorageTable && playerPickableObjectHandler.IsPickableObjectNull;
        }

        private void TakePotInPlayerHands()
        {
            potOnTable.TakeInPlayerHandsAndSetPlayerFree();
            isFlowerOnStorageTable = false;
            weedPlanter.RemovePotFormPlantingWeedList(potOnTable);
        }
    }
}
