using System;
using System.Collections;
using FlowerShop.Coffee;
using FlowerShop.Settings;
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
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly CoffeeSettings coffeeSettings;
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly CoffeeCanvasLiaison coffeeCanvasLiaison;
        [Inject] private readonly PlayerCoffeeEffect playerCoffeeEffect;
        [Inject] private readonly PlayerMoney playerMoney;
        
        [SerializeField] private CoffeeCap coffeeCap;
        [SerializeField] private Transform coffeeCapOnTableTransform;
        [SerializeField] private AnimationClip statMakingCoffeeAnimationClip;
        [SerializeField] private AnimationClip drinkCoffeeAnimationClip;

        [HideInInspector, SerializeField] private TableObjectsRotation tableObjectsRotation;

        private void OnValidate()
        {
            tableObjectsRotation = GetComponent<TableObjectsRotation>();
        }

        public override void ExecuteClickableAbility()
        {
            if (playerBusyness.IsPlayerFree && playerPickableObjectHandler.IsPickableObjectNull && 
                !playerCoffeeEffect.IsCoffeeEffectActive)
            {
                SetPlayerDestinationAndOnPlayerArriveAction(OpenCoffeeCanvas);
            }
        }

        private void OpenCoffeeCanvas()
        {
            coffeeCanvasLiaison.CoffeeCanvas.enabled = true;
        }

        public IEnumerator MakeCoffeeProcess()
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StatMakingCoffeeTrigger);
            
            yield return new WaitForSeconds(statMakingCoffeeAnimationClip.length);
            tableObjectsRotation.StartObjectsRotation();

            yield return new WaitForSeconds(coffeeSettings.CoffeeGrinderRotationDuration);
            tableObjectsRotation.PauseObjectsRotation();
            coffeeCap.FillCoffeeCap();

            yield return new WaitForSeconds(coffeeSettings.CoffeeLiquidMovingTime);
            coffeeCap.TakeInPlayerHandsAndKeepPlayerBusy();

            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.DrinkCoffeeTrigger);

            yield return new WaitForSeconds(coffeeSettings.StartDrinkingCoffeeTimeDelay);
            coffeeCap.EmptyCoffeeCap();
            playerMoney.TakePlayerMoney(coffeeSettings.CoffeePrice);
            playerCoffeeEffect.StartCoffeeEffect();

            yield return new WaitForSeconds(drinkCoffeeAnimationClip.length);
            coffeeCap.PutOnTableAndSetPlayerFree(coffeeCapOnTableTransform);
        }
    }
}
