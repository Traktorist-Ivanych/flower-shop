using FlowerShop.Flowers;
using Input;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables.Abstract
{
    public abstract class Table : MonoBehaviour, IClickableAbility, IPlayerAbility
    {
        [Inject] private protected readonly PlayerMoving playerMoving;
        [Inject] private protected readonly PlayerAbilityExecutor playerAbilityExecutor;
        [Inject] private protected readonly PlayerPickableObjectHandler playerPickableObjectHandler;
        [Inject] private protected readonly PlayerBusyness playerBusyness;

        [SerializeField] private protected GrowingRoom growingRoom;
        [SerializeField] private protected Transform destinationTarget;
        [SerializeField] private protected Transform targetToLookAt;

        private protected delegate void OnPlayerArrive();

        private event OnPlayerArrive OnPlayerArriveEvent;

        public virtual void ExecuteClickableAbility()
        {
            throw new System.NotImplementedException();
        }

        public virtual void ExecutePlayerAbility()
        {
            OnPlayerArriveEvent?.Invoke();
        }

        private protected void SetPlayerDestinationAndOnPlayerArriveAction(OnPlayerArrive targetOnPlayerArriveAction)
        {
            SetPlayerDestination();
            
            OnPlayerArriveEvent = null;
            OnPlayerArriveEvent += targetOnPlayerArriveAction;
        }

        private void SetPlayerDestination()
        {
            playerMoving.SetPlayerDestination(
                destinationTarget: destinationTarget.position, 
                transmittedTargetToLookAt: targetToLookAt.position);
            
            playerAbilityExecutor.SetIPlayerAbility(this);
        }
    }
}
