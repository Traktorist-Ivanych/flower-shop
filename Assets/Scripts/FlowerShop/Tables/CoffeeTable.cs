using System.Collections;
using FlowerShop.Achievements;
using FlowerShop.Coffee;
using FlowerShop.Effects;
using FlowerShop.Help;
using FlowerShop.PickableObjects;
using FlowerShop.Settings;
using FlowerShop.Sounds;
using FlowerShop.Tables.Abstract;
using FlowerShop.Tables.Helpers;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    [RequireComponent(typeof(TableObjectsRotation))]
    public class CoffeeTable : Table
    {
        [Inject] private readonly AchievementsSettings achievementsSettings;
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly CoffeeCanvasLiaison coffeeCanvasLiaison;
        [Inject] private readonly CoffeeLover coffeeLover;
        [Inject] private readonly CoffeeSettings coffeeSettings;
        [Inject] private readonly HelpCanvasLiaison helpCanvasLiaison;
        [Inject] private readonly HelpTexts helpTexts;
        [Inject] private readonly PlayerCoffeeEffect playerCoffeeEffect;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly SoundsHandler soundsHandler;

        [SerializeField] private CoffeeCap coffeeCap;
        [SerializeField] private Transform coffeeCapOnTableTransform;
        [SerializeField] private AnimationClip statMakingCoffeeAnimationClip;
        [SerializeField] private AnimationClip drinkCoffeeAnimationClip;

        [HideInInspector, SerializeField] private TableObjectsRotation tableObjectsRotation;
        [HideInInspector, SerializeField] private ActionProgressbar actionProgressbar;

        private void OnValidate()
        {
            tableObjectsRotation = GetComponent<TableObjectsRotation>();
            actionProgressbar = GetComponentInChildren<ActionProgressbar>();
        }

        private protected override void TryInteractWithTable()
        {
            base.TryInteractWithTable();

            if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
            {
                SetPlayerDestinationAndOnPlayerArriveAction(() => StartCoroutine(MakeCoffeeProcessForEducation()));
            }
            else if (CanPlayerStartMakingCoffee())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(OpenCoffeeCanvas);
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

        private void TryToShowHelpCanvas()
        {
            if (!playerBusyness.IsPlayerFree)
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.PlayerBusy);
            }
            else if (!playerPickableObjectHandler.IsPickableObjectNull)
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.HandsFull);
            }
            else if (playerCoffeeEffect.IsCoffeeEffectPurchased)
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.CoffeeEffectPurchased);
            }
            else if (playerCoffeeEffect.IsCoffeeEffectActive)
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.CoffeeEffectAlreadyActive);
            }
        }

        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerStartMakingCoffee() || CanPlayerUseTableInfoCanvas();
        }

        private bool CanPlayerStartMakingCoffee()
        {
            return playerBusyness.IsPlayerFree && playerPickableObjectHandler.IsPickableObjectNull &&
                   !playerCoffeeEffect.IsCoffeeEffectActive;
        }

        private bool CanPlayerUseTableInfoCanvas()
        {
            return playerBusyness.IsPlayerFree && playerPickableObjectHandler.CurrentPickableObject is InfoBook;
        }

        private void UseTableInfoCanvas()
        {
            tableInfoCanvasLiaison.ShowCanvas(tableInfo, growingRoom);
        }

        private void OpenCoffeeCanvas()
        {
            coffeeCanvasLiaison.EnableCanvas();
        }

        public IEnumerator MakePurchasedCoffeeProcess()
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StatMakingCoffeeTrigger);

            yield return new WaitForSeconds(statMakingCoffeeAnimationClip.length);
            tableObjectsRotation.StartObjectsRotation();
            soundsHandler.StartPlayingCoffeeGrinderAudio();
            actionProgressbar.EnableActionProgressbar(coffeeSettings.CoffeeGrinderRotationDuration);

            yield return new WaitForSeconds(coffeeSettings.CoffeeGrinderRotationDuration);
            tableObjectsRotation.PauseObjectsRotation();
            soundsHandler.StopPlayingCoffeeGrinderAudio();
            coffeeCap.FillCoffeeCap();
            soundsHandler.PlayFillCoffeeCupAudio();

            yield return new WaitForSeconds(coffeeSettings.CoffeeLiquidMovingTime);
            coffeeCap.TakeInPlayerHandsAndKeepPlayerBusy();

            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.DrinkCoffeeTrigger);

            yield return new WaitForSeconds(coffeeSettings.StartDrinkingCoffeeTimeDelay);
            coffeeCap.EmptyCoffeeCap();
            coffeeLover.SetProgress(achievementsSettings.CoffeeLoverMaxProgress);
            playerCoffeeEffect.SetPurchasedCoffeeEffectIndicator();

            yield return new WaitForSeconds(drinkCoffeeAnimationClip.length);
            coffeeCap.PutOnTableAndSetPlayerFree(coffeeCapOnTableTransform);
            selectedTableEffect.ActivateEffectWithDelay();
        }

        public IEnumerator MakeCoffeeProcessForAds()
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StatMakingCoffeeTrigger);

            yield return new WaitForSeconds(statMakingCoffeeAnimationClip.length);
            tableObjectsRotation.StartObjectsRotation();
            soundsHandler.StartPlayingCoffeeGrinderAudio();
            actionProgressbar.EnableActionProgressbar(coffeeSettings.CoffeeGrinderRotationDuration);

            yield return new WaitForSeconds(coffeeSettings.CoffeeGrinderRotationDuration);
            tableObjectsRotation.PauseObjectsRotation();
            soundsHandler.StopPlayingCoffeeGrinderAudio();
            coffeeCap.FillCoffeeCap();
            soundsHandler.PlayFillCoffeeCupAudio();

            yield return new WaitForSeconds(coffeeSettings.CoffeeLiquidMovingTime);
            coffeeCap.TakeInPlayerHandsAndKeepPlayerBusy();

            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.DrinkCoffeeTrigger);

            yield return new WaitForSeconds(coffeeSettings.StartDrinkingCoffeeTimeDelay);
            coffeeCap.EmptyCoffeeCap();
            coffeeLover.IncreaseProgress();
            playerCoffeeEffect.StartCoffeeEffect();

            yield return new WaitForSeconds(drinkCoffeeAnimationClip.length);
            coffeeCap.PutOnTableAndSetPlayerFree(coffeeCapOnTableTransform);
            selectedTableEffect.ActivateEffectWithDelay();
        }

        public IEnumerator MakeCoffeeProcessForEducation()
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StatMakingCoffeeTrigger);

            yield return new WaitForSeconds(statMakingCoffeeAnimationClip.length);
            tableObjectsRotation.StartObjectsRotation();
            soundsHandler.StartPlayingCoffeeGrinderAudio();
            actionProgressbar.EnableActionProgressbar(coffeeSettings.CoffeeGrinderRotationDuration);

            yield return new WaitForSeconds(coffeeSettings.CoffeeGrinderRotationDuration);
            tableObjectsRotation.PauseObjectsRotation();
            soundsHandler.StopPlayingCoffeeGrinderAudio();
            coffeeCap.FillCoffeeCap();
            soundsHandler.PlayFillCoffeeCupAudio();

            yield return new WaitForSeconds(coffeeSettings.CoffeeLiquidMovingTime);
            coffeeCap.TakeInPlayerHandsAndKeepPlayerBusy();

            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.DrinkCoffeeTrigger);

            yield return new WaitForSeconds(coffeeSettings.StartDrinkingCoffeeTimeDelay);
            coffeeCap.EmptyCoffeeCap();
            coffeeLover.IncreaseProgress();
            playerCoffeeEffect.StartCoffeeEffect();
            educationHandler.CompleteEducationStep();

            yield return new WaitForSeconds(drinkCoffeeAnimationClip.length);
            coffeeCap.PutOnTableAndSetPlayerFree(coffeeCapOnTableTransform);
            selectedTableEffect.ActivateEffectWithDelay();
        }
    }
}
