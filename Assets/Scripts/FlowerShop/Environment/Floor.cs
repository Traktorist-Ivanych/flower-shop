using FlowerShop.Education;
using FlowerShop.Effects;
using Input;
using PlayerControl;
using UniRx;
using UnityEngine;
using Zenject;

namespace FlowerShop.Environment
{
    public class Floor : MonoBehaviour, IClicableFloor
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly EffectsSettings effectsSettings;
        [Inject] private readonly PlayerAbilityExecutor playerAbilityExecutor;
        [Inject] private readonly PlayerBusyness playerBusyness;
        [Inject] private readonly PlayerMoving playerMoving;

        [SerializeField] private Transform sphereTransform;

        [HideInInspector, SerializeField] private MeshRenderer sphereRenderer;

        private float currentSphereEnableTime;

        private readonly CompositeDisposable sphereCompositeDisposable = new();

        private void OnValidate()
        {
            sphereRenderer = sphereTransform.GetComponent<MeshRenderer>();
        }

        public void ExecuteClickableFloorAbility(Vector3 floorClickCoordinates)
        {
            if (educationHandler.IsEducationActive)
            {
                sphereCompositeDisposable.Clear();
                currentSphereEnableTime = 0;
                sphereRenderer.enabled = true;
                sphereRenderer.material = effectsSettings.FailMaterial;

                sphereTransform.position = floorClickCoordinates;

                Observable.EveryUpdate().Subscribe(updateProgressbar =>
                {
                    currentSphereEnableTime += Time.deltaTime;

                    if (currentSphereEnableTime >= effectsSettings.ClickableEffectDuration)
                    {
                        sphereRenderer.enabled = false;

                        sphereCompositeDisposable.Clear();
                    }
                }).AddTo(sphereCompositeDisposable);
            }
            else
            {
                sphereCompositeDisposable.Clear();
                currentSphereEnableTime = 0;
                sphereRenderer.enabled = true;

                if (playerBusyness.IsPlayerFree)
                {
                    playerAbilityExecutor.ResetPlayerAbility();
                    playerMoving.SetNotTablePlayerDestination(floorClickCoordinates);
                    sphereRenderer.material = effectsSettings.SuccessMaterial;
                }
                else
                {
                    sphereRenderer.material = effectsSettings.FailMaterial;
                }

                sphereTransform.position = floorClickCoordinates;

                Observable.EveryUpdate().Subscribe(updateProgressbar =>
                {
                    currentSphereEnableTime += Time.deltaTime;

                    if (currentSphereEnableTime >= effectsSettings.ClickableEffectDuration)
                    {
                        sphereRenderer.enabled = false;

                        sphereCompositeDisposable.Clear();
                    }
                }).AddTo(sphereCompositeDisposable);
            }
        }
    }
}

