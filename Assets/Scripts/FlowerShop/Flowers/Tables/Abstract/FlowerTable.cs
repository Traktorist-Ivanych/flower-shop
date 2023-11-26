using FlowerShop.Flowers;
using Input;
using PlayerControl;
using UnityEngine;
using Zenject;

public abstract class FlowerTable : MonoBehaviour, IClickableAbility, IPlayerAbility
{
    [Inject] private protected readonly PlayerMoving playerMoving;
    [Inject] private protected readonly PlayerAbilityExecutor playerAbilityExecutor;
    [Inject] private protected readonly PlayerPickableObjectHandler playerPickableObjectHandler;
    [Inject] private protected readonly PlayerBusyness playerBusyness;

    [SerializeField] private protected GrowingRoom growingRoom;
    [SerializeField] private protected Transform destinationTarget;
    [SerializeField] private protected Transform targetToLookAt;

    // in overrides probably better call .base, and don't call 'throw new System.NotImplementedException()' for now - just empty method
    public virtual void ExecuteClickableAbility()
    {
        throw new System.NotImplementedException();
    }

    public virtual void ExecutePlayerAbility()
    {
        throw new System.NotImplementedException();
    }

    protected private void SetPlayerDestination()
    {
        playerMoving.SetPlayerDestination(destinationTarget.position, targetToLookAt.position);
        playerAbilityExecutor.SetIPlayerAbility(this);
    }
}
