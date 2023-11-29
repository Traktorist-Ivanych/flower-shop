using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.PickableObjects.Moving
{
    public class LittlePickableObjectMovingAndRotating : PickableObjectMovingAndRotating
    {
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly PlayerBusyness playerBusyness;
        
        public void TakeLittlePickableObjectInPlayerHandsWithRotation()
        {
            finishTransform = playerComponents.PlayerHandsForLittleObjectTransform;
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.TakeLittleObjectTrigger);
            MovePickableObject();
        }

        public void PutLittlePickableObjectOnTableWithRotation(Transform tableTransform)
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.GiveLittleObjectTrigger);
            finishTransform = tableTransform;
            MovePickableObject();
        }

        private protected override void FinishMoving()
        {
            base.FinishMoving();
            
            playerBusyness.SetPlayerFree();
        }
    }
}