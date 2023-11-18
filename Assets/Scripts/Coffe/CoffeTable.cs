using System.Collections;
using UnityEngine;
using Zenject;

public class CoffeTable : FlowerTable
{
    [Inject] private readonly PlayerComponents playerComponents;
    [Inject] private readonly GameConfiguration gameConfiguration;
    [Inject] private readonly AllCanvasLiaisons allCanvasLiaisons;
    [Inject] private readonly PlayerCoffeEffect playerCoffeEffect;
    [Inject] private readonly PlayerMoney playerMoney;

    [SerializeField] private CoffeCap coffeCap;
    [SerializeField] private Transform coffeeGrinderTransform;
    [SerializeField] private Transform coffeCapOnTableTransform;

    private bool isCoffeMaking;

    private void Update()
    {
        if (isCoffeMaking)
        {
            coffeeGrinderTransform.Rotate(Vector3.forward, gameConfiguration.ObjectsRotateDegreesPerSecond * Time.deltaTime);
        }
    }

    public override void ExecuteClickableAbility()
    {
        if (playerBusyness.IsPlayerFree && playerDinamicObject.IsPlayerDinamicObjectNull() && 
            !playerCoffeEffect.IsCoffeEffectActive)
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
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StatMakingCoffeTrigger);
        playerMoney.TakePlayerMoney(gameConfiguration.CoffePrice);
        StartCoroutine(MakeCoffeProcces());
    }

    private IEnumerator MakeCoffeProcces()
    {
        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay);
        isCoffeMaking = true;

        yield return new WaitForSeconds(4);
        isCoffeMaking = false;
        coffeCap.FillCoffeCap();

        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay);
        coffeCap.DinamicObjectMoving.ShouldPlayerBecomeFree = false;
        coffeCap.TakeCoffeCapInPlayerHands();

        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay);
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.DrinkCoffeTrigger);

        yield return new WaitForSeconds(0.35f);
        coffeCap.EmptyCoffeCap();
        playerCoffeEffect.StartCoffeEffect();

        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay * 2);
        coffeCap.PutCoffeCapOnTable(coffeCapOnTableTransform);
    }
}
