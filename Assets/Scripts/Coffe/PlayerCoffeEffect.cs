using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class PlayerCoffeEffect : MonoBehaviour
{
    [Inject] private readonly NavMeshAgent playerNavAgent;
    [Inject] private readonly PlayerMoving playerMoving;
    [Inject] private readonly GameConfiguration gameConfiguration;

    private float currentCoffeEffectDuration;
    private bool isCoffeEffectActive;

    public bool IsCoffeEffectActive
    {
        get => isCoffeEffectActive;
    }

    private void Update()
    {
        if (isCoffeEffectActive)
        {
            currentCoffeEffectDuration += Time.deltaTime;

            if (currentCoffeEffectDuration >= gameConfiguration.CoffeEffectDurationTime)
            {
                FinishCoffeeEffect();
            }
        }
    }

    public void StartCoffeEffect()
    {
        isCoffeEffectActive = true;

        playerNavAgent.speed = gameConfiguration.PlayerNavAgentCoffeSpeed;
        playerNavAgent.angularSpeed = gameConfiguration.PlayerNavAgentCoffeAngularSpeed;
        playerNavAgent.acceleration = gameConfiguration.PlayerNavAgentCoffeAcceleration;
        playerMoving.SetCoffeRotationSpeed();
    }

    private void FinishCoffeeEffect()
    {
        isCoffeEffectActive = false;

        playerNavAgent.speed = gameConfiguration.PlayerNavAgentSpeed;
        playerNavAgent.angularSpeed = gameConfiguration.PlayerNavAgentAngularSpeed;
        playerNavAgent.acceleration = gameConfiguration.PlayerNavAgentAcceleration;
        playerMoving.SetOrdinaryRotationSpeed();
    }
}
