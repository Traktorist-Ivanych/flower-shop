using System.Collections;
using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using FlowerShop.Settings;
using FlowerShop.Tables.Abstract;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class PlantingSeedsTable : Table
    {
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;

        [SerializeField] private Transform potOnTableTransform;
        [SerializeField] private Canvas seedsCanvas;

        private Pot plantingSeedPot;

        public override void ExecuteClickableAbility()
        {
            if (CanPlayerPlantSeed())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(() => StartCoroutine(StartPlantingSeed()));
            }
        }

        public void PlantSeed(FlowerInfo transmittedFlowerInfo)
        {
            plantingSeedPot.PlantSeed(transmittedFlowerInfo);
            seedsCanvas.enabled = false;
            plantingSeedPot.TakeInPlayerHandsAndSetPlayerFree();
        }

        private bool CanPlayerPlantSeed()
        {
            if (playerBusyness.IsPlayerFree && playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                plantingSeedPot = currentPot;

                return plantingSeedPot.GrowingRoom == growingRoom &&
                       plantingSeedPot.IsSoilInsidePot &&
                       plantingSeedPot.PlantedFlowerInfo == flowersSettings.FlowerInfoEmpty;
            }

            return false;
        }
        
        private IEnumerator StartPlantingSeed()
        {
            plantingSeedPot.PutOnTableAndKeepPlayerBusy(potOnTableTransform);
            
            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            
            seedsCanvas.enabled = true;
        }
    }
}
