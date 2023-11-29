using System.Collections;
using PlayerControl;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class CoffeeTable : FlowerTable
{
    [Inject] private readonly PlayerComponents playerComponents;
    [Inject] private readonly GameConfiguration gameConfiguration;
    [Inject] private readonly CoffeeCanvasLiaison coffeeCanvasLiaison;
    [Inject] private readonly PlayerCoffeeEffect playerCoffeeEffect;
    [Inject] private readonly PlayerMoney playerMoney;

    [SerializeField] private CoffeeCap coffeeCap;
    [SerializeField] private Transform coffeeGrinderTransform;
    [SerializeField] private Transform coffeeCapOnTableTransform;

    private bool isCoffeeMaking;

    private void Update()
    {
        // Replace with dotween StartAnimation
        if (isCoffeeMaking)
        {
            coffeeGrinderTransform.Rotate(Vector3.forward, gameConfiguration.ObjectsRotateDegreesPerSecond * Time.deltaTime);
        }
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

    public void MakeCoffe()
    {
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StatMakingCoffeeTrigger);
        playerMoney.TakePlayerMoney(gameConfiguration.CoffePrice);
        StartCoroutine(MakeCoffeeProcess());
    }

    private IEnumerator MakeCoffeeProcess()
    {
        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay);
        isCoffeeMaking = true;

        // 4 - should be in settings
        yield return new WaitForSeconds(4);
        isCoffeeMaking = false;
        coffeeCap.FillCoffeeCap();

        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay);
        coffeeCap.TakeCoffeeCapInPlayerHands();

        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay);
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.DrinkCoffeeTrigger);

        // should be in settings
        yield return new WaitForSeconds(0.35f);
        coffeeCap.EmptyCoffeeCap();
        playerCoffeeEffect.StartCoffeeEffect();

        // either animation event either get time through animation clip
        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay * 2);
        coffeeCap.PutCoffeeCapOnTable(coffeeCapOnTableTransform);
    }
}
