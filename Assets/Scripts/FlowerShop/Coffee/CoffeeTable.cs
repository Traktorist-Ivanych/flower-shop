using System.Collections;
using DG.Tweening;
using FlowerShop.Settings;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.Coffee
{
    public class CoffeeTable : FlowerTable
    {
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly CoffeeSettings coffeeSettings;
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly CoffeeCanvasLiaison coffeeCanvasLiaison;
        [Inject] private readonly PlayerCoffeeEffect playerCoffeeEffect;
        [Inject] private readonly PlayerMoney playerMoney;
        
        [SerializeField] private Transform coffeeGrinderTransform;
        [SerializeField] private CoffeeCap coffeeCap;
        [SerializeField] private Transform coffeeCapOnTableTransform;
        [SerializeField] private AnimationClip statMakingCoffeeAnimationClip;
        [SerializeField] private AnimationClip drinkCoffeeAnimationClip;

        private Tween coffeeGrinderRotation;

        private void Awake()
        {
            coffeeGrinderRotation = coffeeGrinderTransform.DORotate(
                endValue: new Vector3(0,360,0),
                duration: actionsWithTransformSettings.RotationObject360DegreesTime,
                RotateMode.WorldAxisAdd)
                .SetEase(Ease.Linear)
                .SetLoops(-1);
            
            coffeeGrinderRotation.Pause();
        }

        public override void ExecuteClickableAbility()
        {
            if (playerBusyness.IsPlayerFree && playerPickableObjectHandler.IsPickableObjectNull && 
                !playerCoffeeEffect.IsCoffeeEffectActive)
            {
                SetPlayerDestination();
            }
        }

        public override void ExecutePlayerAbility()
        {
            coffeeCanvasLiaison.CoffeeCanvas.enabled = true;
        }

        public IEnumerator MakeCoffeeProcess()
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StatMakingCoffeeTrigger);
            
            yield return new WaitForSeconds(statMakingCoffeeAnimationClip.length);
            coffeeGrinderRotation.Play();

            yield return new WaitForSeconds(coffeeSettings.CoffeeGrinderRotationDuration);
            coffeeGrinderRotation.Pause();
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
