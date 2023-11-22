using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class PlayerCoffeeEffect : MonoBehaviour
{
    [Inject] private readonly PlayerMoving playerMoving;
    [Inject] private readonly GameConfiguration gameConfiguration;

    private float currentCoffeeEffectDuration;

    public bool IsCoffeeEffectActive { get; private set; }

    private void Update()
    {
        // replace with dotween
        if (IsCoffeeEffectActive)
        {
            currentCoffeeEffectDuration += Time.deltaTime;

            if (currentCoffeeEffectDuration >= gameConfiguration.CoffeEffectDurationTime)
            {
                FinishCoffeeEffect();
            }
        }
    }

    public void StartCoffeeEffect()
    {
        IsCoffeeEffectActive = true;
        playerMoving.SetCoffeeNavAgentSetting();
    }

    private void FinishCoffeeEffect()
    {
        IsCoffeeEffectActive = false;
        playerMoving.SetOrdinaryNavAgentSetting();
    }
}
