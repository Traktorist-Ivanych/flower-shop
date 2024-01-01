using System.Collections;
using FlowerShop.PickableObjects;
using FlowerShop.PickableObjects.Moving;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.Fertilizers
{
    [RequireComponent(typeof(ObjectMoving))]
    public abstract class Fertilizer : MonoBehaviour, IPickableObject
    {
        [Inject] private readonly FertilizersCanvasLiaison fertilizersCanvasLiaison;
        [Inject] private readonly FertilizersSetting fertilizersSetting;
        [Inject] private readonly PlayerBusyness playerBusyness;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly PlayerPickableObjectHandler playerPickableObjectHandler;

        [SerializeField] private ParticleSystem treatEffect;
        
        [HideInInspector, SerializeField] private ObjectMoving objectMoving;
        
        public int AvailableUsesNumber { get; private set; }
        
        private void OnValidate()
        {
            objectMoving = GetComponent<ObjectMoving>();
        }

        private void Start()
        {
            AvailableUsesNumber = fertilizersSetting.FertilizersStartUsesNumber;
            fertilizersCanvasLiaison.UpdateFertilizersAvailableUsesNumber();
        }

        public virtual void TreatPot(Pot potForTreating)
        {
            StartCoroutine(ShowTreatEffects());
        }

        public void TakeInPlayerHandsAndSetPlayerFree()
        {
            playerPickableObjectHandler.CurrentPickableObject = this;
            objectMoving.MoveObject(
                targetFinishTransform: playerComponents.PlayerHandsForCoffeeTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.TakeLittleObjectTrigger, 
                setPlayerFree: true);
        }

        public void PutOnTableAndSetPlayerFree(Transform targetTransform)
        {
            playerPickableObjectHandler.ResetPickableObject();
            objectMoving.MoveObject(
                targetFinishTransform: targetTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.GiveLittleObjectTrigger, 
                setPlayerFree: true);
        }

        public void IncreaseAvailableUsesNumber()
        {
            AvailableUsesNumber += fertilizersSetting.IncreaseFertilizerAmount;
            fertilizersCanvasLiaison.UpdateFertilizersAvailableUsesNumber();
        }

        private IEnumerator ShowTreatEffects()
        {
            AvailableUsesNumber--;
            fertilizersCanvasLiaison.UpdateFertilizersAvailableUsesNumber();
            treatEffect.Play();

            yield return new WaitForSeconds(fertilizersSetting.FertilizerTreatingTime);
            
            playerBusyness.SetPlayerFree();
        }
    }
}
