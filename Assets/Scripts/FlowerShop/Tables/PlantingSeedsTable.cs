using System.Collections;
using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using FlowerShop.Settings;
using FlowerShop.Sounds;
using FlowerShop.Tables.Abstract;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class PlantingSeedsTable : Table
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly PlayerMoney playerMoney;
        [Inject] private readonly SoundsHandler soundsHandler;

        [SerializeField] private Transform potOnTableTransform;
        [SerializeField] private Canvas seedsCanvas;

        private Pot plantingSeedPot;
        
        public override void ExecuteClickableAbility()
        {
            base.ExecuteClickableAbility();

            if (CanPlayerPlantSeed())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(() => StartCoroutine(StartPlantingSeed()));
            }
        }

        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerPlantSeed();
        }

        public void PlantSeed(FlowerInfo transmittedFlowerInfo)
        {
            plantingSeedPot.PlantSeed(transmittedFlowerInfo);
            seedsCanvas.enabled = false;
            playerMoney.TakePlayerMoney(flowersSettings.FirstLvlFlowersPrice);
            soundsHandler.PlayTakeMoneyAudio();
            plantingSeedPot.TakeInPlayerHandsAndSetPlayerFree();
        }

        private bool CanPlayerPlantSeed()
        {
            if (playerBusyness.IsPlayerFree && playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                plantingSeedPot = currentPot;

                return plantingSeedPot.GrowingRoom == growingRoom &&
                       plantingSeedPot.IsSoilInsidePot &&
                       plantingSeedPot.PlantedFlowerInfo == flowersSettings.FlowerInfoEmpty &&
                       playerMoney.CurrentPlayerMoney >= flowersSettings.PrimaryFlowerGrowingLvl;
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
