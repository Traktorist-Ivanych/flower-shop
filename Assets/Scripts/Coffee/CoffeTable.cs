using System.Collections;
using UnityEngine;
using Zenject;

public class CoffeTable : FlowerTable
{
    [Inject] private readonly PlayerComponents playerComponents;
    [Inject] private readonly GameConfiguration gameConfiguration;
    [Inject] private readonly AllCanvasLiaisons allCanvasLiaisons;
    [Inject] private readonly PlayerCoffeeEffect playerCoffeeEffect;
    [Inject] private readonly PlayerMoney playerMoney;

    [SerializeField] private CoffeCap coffeCap;
    [SerializeField] private Transform coffeeGrinderTransform;
    [SerializeField] private Transform coffeCapOnTableTransform;

    private bool isCoffeMaking;

    private void Update()
    {
        // Replace with dotween StartAnimation
        if (isCoffeMaking)
        {
            coffeeGrinderTransform.Rotate(Vector3.forward, gameConfiguration.ObjectsRotateDegreesPerSecond * Time.deltaTime);
        }
    }

    public override void ExecuteClickableAbility()
    {
        if (playerBusyness.IsPlayerFree && PlayerPickableObjectHandler.IsPlayerPickableObjectNull() && 
            !playerCoffeeEffect.IsCoffeeEffectActive)
        {
            SetPlayerDestination();
        }
    }

    public override void ExecutePlayerAbility()
    {
        allCanvasLiaisons.CoffeCanvas.enabled = true;
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
        isCoffeMaking = true;

        // 4 - should be in settings
        yield return new WaitForSeconds(4);
        isCoffeMaking = false;
        coffeCap.FillCoffeCap();

        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay);
        coffeCap.DynamicObjectMoving.ShouldPlayerBecomeFree = false;
        coffeCap.TakeCoffeCapInPlayerHands();

        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay);
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.DrinkCoffeeTrigger);

        // should be in settings
        yield return new WaitForSeconds(0.35f);
        coffeCap.EmptyCoffeCap();
        playerCoffeeEffect.StartCoffeeEffect();

        // either animation event either get time through animation clip
        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay * 2);
        coffeCap.PutCoffeCapOnTable(coffeCapOnTableTransform);
    }
}
