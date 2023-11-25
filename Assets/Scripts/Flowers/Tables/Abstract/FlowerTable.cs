using Input;
using PlayerControl;
using UnityEngine;
using Zenject;

public abstract class FlowerTable : MonoBehaviour, IClickableAbility, IPlayerAbility
{
    [Inject] protected private readonly PlayerMoving playerMoving;
    [Inject] protected private readonly PlayerAbilityExecutor playerAbilityExecutor;
    [Inject] protected private readonly PlayerPickableObjectHandler playerPickableObjectHandler;
    [Inject] protected private readonly PlayerBusyness playerBusyness;

    [SerializeField] protected private IGrowingRoom.GroweringRoom groweringRoom;
    [SerializeField] protected private Transform destinationTarget;
    [SerializeField] protected private Transform targetToLookAt;

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
