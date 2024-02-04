using DG.Tweening;
using FlowerShop.PickableObjects.Moving;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.Coffee
{
    [RequireComponent (typeof(ObjectMoving))]
    public class CoffeeCap : MonoBehaviour
    {
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly CoffeeSettings coffeeSettings;
    
        [SerializeField] private Transform coffeeLiquidTransform;
        [SerializeField] private Transform coffeeLiquidEmptyTransform;
        [SerializeField] private Transform coffeeLiquidFullTransform;

        [HideInInspector, SerializeField] private ObjectMoving objectMoving;
        [HideInInspector, SerializeField] private MeshRenderer coffeeLiquidRenderer;
    
        private Transform finishTransform;

        private void OnValidate()
        {
            objectMoving = GetComponent<ObjectMoving>();
            coffeeLiquidRenderer = coffeeLiquidTransform.GetComponent<MeshRenderer>();
        }

        public void FillCoffeeCap()
        {
            coffeeLiquidRenderer.enabled = true;
            finishTransform = coffeeLiquidFullTransform;
            MoveAndScaleCoffeeLiquid();
        }

        public void SetCoffeeCapFull()
        {
            coffeeLiquidRenderer.enabled = true;
            coffeeLiquidTransform.position = coffeeLiquidFullTransform.position;
        }

        public void TakeInPlayerHandsAndKeepPlayerBusy()
        {
            objectMoving.MoveObject(
                targetFinishTransform: playerComponents.PlayerHandsForCoffeeTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.TakeLittleObjectTrigger, 
                setPlayerFree: false);
        }

        public void EmptyCoffeeCap()
        {
            finishTransform = coffeeLiquidEmptyTransform;
            MoveAndScaleCoffeeLiquid();
        }

        public void PutOnTableAndSetPlayerFree(Transform targetTransform)
        {
            objectMoving.MoveObject(
                targetFinishTransform: targetTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.GiveLittleObjectTrigger, 
                setPlayerFree: true);
        }

        private void MoveAndScaleCoffeeLiquid()
        {
            coffeeLiquidTransform.DOMove(
                endValue: finishTransform.position,
                duration: coffeeSettings.CoffeeLiquidMovingTime)
                .OnComplete(FinishMovingCoffeeLiquid);
            
            coffeeLiquidTransform.DOScale(
                endValue: finishTransform.localScale,
                duration: coffeeSettings.CoffeeLiquidMovingTime);
        }

        private void FinishMovingCoffeeLiquid()
        {
            if (finishTransform.Equals(coffeeLiquidEmptyTransform))
            {
                coffeeLiquidRenderer.enabled = false;
            }
        }
    }
}
