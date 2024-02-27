using FlowerShop.Effects;
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
        [Inject] private protected readonly SelectedTableEffect selectedTableEffect;

        [SerializeField] private protected GrowingRoom growingRoom;
        [SerializeField] private protected Transform destinationTarget;
        [SerializeField] private protected Transform targetToLookAt;
        [SerializeField] private protected MeshRenderer[] selectedObjectsMeshRenderer;

        private protected delegate void OnPlayerArrive();
        private event OnPlayerArrive OnPlayerArriveEvent;
        
        private void OnEnable()
        {
            selectedTableEffect.SelectedTableCheckEvent += TryToShowSelectedTableEffect;
        }

        private void OnDisable()
        {
            selectedTableEffect.SelectedTableCheckEvent -= TryToShowSelectedTableEffect;
        }
        
        public virtual void ExecuteClickableAbility()
        {
            throw new System.NotImplementedException();
        }

        public void ExecutePlayerAbility()
        {
            OnPlayerArriveEvent?.Invoke();
        }

        private protected void SetPlayerDestinationAndOnPlayerArriveAction(OnPlayerArrive targetOnPlayerArriveAction)
        {
            SetPlayerDestination();
            
            OnPlayerArriveEvent = null;
            OnPlayerArriveEvent += targetOnPlayerArriveAction;
            
            selectedTableEffect.DeactivateEffect();
        }
        
        private void TryToShowSelectedTableEffect()
        {
            if (CanSelectedTableEffectBeDisplayed())
            {
                SetSelectableMaterial();
            }
            else
            {
                ResetMaterial();
            }
        }

        private protected virtual bool CanSelectedTableEffectBeDisplayed()
        {
            return false;
        }

        private void SetSelectableMaterial()
        {
            foreach (MeshRenderer selectedObjectMeshRenderer in selectedObjectsMeshRenderer)
            {
                selectedObjectMeshRenderer.material = selectedTableEffect.EffectMaterial;
            }
        }

        private void ResetMaterial()
        {
            foreach (MeshRenderer selectedObjectMeshRenderer in selectedObjectsMeshRenderer)
            {
                selectedObjectMeshRenderer.material = selectedTableEffect.EnvironmentMaterial;
            }
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
