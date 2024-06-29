using FlowerShop.Effects;
using FlowerShop.PickableObjects.Moving;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.PickableObjects
{
    [RequireComponent(typeof(ObjectMoving))]
    public class InfoBook : MonoBehaviour, IPickableObject
    {
        [Inject] private readonly PlayerPickableObjectHandler playerPickableObjectHandler;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly SelectedTableEffect selectedTableEffect;

        [HideInInspector, SerializeField] private ObjectMoving objectMoving;

        private void OnValidate()
        {
            objectMoving = GetComponent<ObjectMoving>();
        }

        public void TakeInPlayerHandsAndSetPlayerFree()
        {
            playerPickableObjectHandler.CurrentPickableObject = this;
            selectedTableEffect.ActivateEffectWithDelay();

            objectMoving.MoveObject(
                targetFinishTransform: playerComponents.PlayerHandsForCoffeeTransform,
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.TakeLittleObjectTrigger,
                setPlayerFree: true);
        }

        public void PutOnTableAndSetPlayerFree(Transform targetTransform)
        {
            selectedTableEffect.ActivateEffectWithDelay();

            objectMoving.MoveObject(
                targetFinishTransform: targetTransform,
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.GiveLittleObjectTrigger,
                setPlayerFree: true);
        }

        public void LoadInPlayerHands()
        {
            objectMoving.SetParentAndParentPositionAndRotation(playerComponents.PlayerHandsForCoffeeTransform);
            playerPickableObjectHandler.CurrentPickableObject = this;
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.LoadToHoldLittleObject);
            selectedTableEffect.ActivateEffectWithDelay();
        }
    }
}
