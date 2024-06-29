using FlowerShop.Education;
using FlowerShop.Effects;
using FlowerShop.Flowers;
using FlowerShop.Help;
using Input;
using PlayerControl;
using UniRx;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables.Abstract
{
    public abstract class Table : MonoBehaviour, IClickableAbility, IPlayerAbility
    {
        [Inject] private protected readonly EducationHandler educationHandler;
        [Inject] private protected readonly EffectsSettings effectsSettings;
        [Inject] private protected readonly PlayerMoving playerMoving;
        [Inject] private protected readonly PlayerAbilityExecutor playerAbilityExecutor;
        [Inject] private protected readonly PlayerPickableObjectHandler playerPickableObjectHandler;
        [Inject] private protected readonly PlayerBusyness playerBusyness;
        [Inject] private protected readonly SelectedTableEffect selectedTableEffect;
        [Inject] private protected readonly TableInfoCanvasLiaison tableInfoCanvasLiaison;

        [SerializeField] private protected TableInfo tableInfo;
        [SerializeField] private protected GrowingRoom growingRoom;
        [SerializeField] private protected Transform destinationTarget;
        [SerializeField] private protected Transform targetToLookAt;
        [SerializeField] private protected MeshRenderer[] selectedObjectsMeshRenderer;

        private protected delegate void OnPlayerArrive();
        private event OnPlayerArrive OnPlayerArriveEvent;
        
        private readonly CompositeDisposable clickableAbilityCompositeDisposable = new();

        private float currentClickableAbilityEffectTime;
        
        private protected virtual void OnEnable()
        {
            selectedTableEffect.SelectedTableCheckEvent += TryToShowSelectedTableEffect;
        }

        private protected virtual void OnDisable()
        {
            selectedTableEffect.SelectedTableCheckEvent -= TryToShowSelectedTableEffect;
        }
        
        public void ExecuteClickableAbility()
        {
            if (CanTableBeInteractedDuringEducation())
            {
                TryInteractWithTable();
            }
        }

        private protected virtual void TryInteractWithTable()
        {
            StartClickableAbilityEffect();
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
            SetClickSuccessMaterial();

            if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
            {
                educationHandler.CompleteEducationStep();
            }
        }
        
        private void TryToShowSelectedTableEffect()
        {
            if (CanTableBeInteractedDuringEducation() && CanSelectedTableEffectBeDisplayed())
            {
                SetSelectableMaterial();
            }
            else
            {
                ResetMaterial();
            }
        }

        private bool CanTableBeInteractedDuringEducation()
        {
            if (educationHandler.IsEducationActive)
            {
                return educationHandler.IsMonoBehaviourCurrentEducationStep(this);
            }

            return true;
        }

        private void StartClickableAbilityEffect()
        {
            clickableAbilityCompositeDisposable.Clear();
            currentClickableAbilityEffectTime = 0;
            SetClickFailMaterial();
            
            Observable.EveryUpdate().Subscribe( updateCoffeeEffectTimer =>
            {
                currentClickableAbilityEffectTime += Time.deltaTime;

                if (currentClickableAbilityEffectTime >= effectsSettings.ClickableEffectDuration)
                {
                    ResetMaterial();
                    clickableAbilityCompositeDisposable.Clear();
                }
            }).AddTo(clickableAbilityCompositeDisposable);
        }

        private protected virtual bool CanSelectedTableEffectBeDisplayed()
        {
            return false;
        }

        private void SetSelectableMaterial()
        {
            SetTableMaterial(effectsSettings.SelectableMaterial);
        }
        
        private void SetClickFailMaterial()
        {
            SetTableMaterial(effectsSettings.FailMaterial);
        }

        private void SetClickSuccessMaterial()
        {
            SetTableMaterial(effectsSettings.SuccessMaterial);
        }

        private void ResetMaterial()
        {
            SetTableMaterial(effectsSettings.EnvironmentMaterial);
        }

        private void SetTableMaterial(Material targetMaterial)
        {
            foreach (MeshRenderer selectedObjectMeshRenderer in selectedObjectsMeshRenderer)
            {
                selectedObjectMeshRenderer.material = targetMaterial;
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
