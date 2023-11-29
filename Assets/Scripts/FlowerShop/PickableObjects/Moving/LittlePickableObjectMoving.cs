using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.PickableObjects.Moving
{
    public class LittlePickableObjectMoving : PickableObjectMoving
    {
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly PlayerBusyness playerBusyness;

        private bool setPlayerFree;
        
        public void TakeLittlePickableObjectInPlayerHandsAndKeepPlayerBusy()
        {
            setPlayerFree = false;
            TakeLittlePickableObjectInPlayerHands();
        }

        public void PutLittlePickableObjectOnTable(Transform tableTransform)
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.GiveLittleObjectTrigger);
            finishTransform = tableTransform;
            MovePickableObject();
        }
        
        private protected override void FinishMoving()
        {
            base.FinishMoving();

            if (setPlayerFree)
            {
                playerBusyness.SetPlayerFree();
            }
            else
            {
                setPlayerFree = true;
            }
        }
        
        private void TakeLittlePickableObjectInPlayerHands()
        {
            finishTransform = playerComponents.PlayerHandsForLittleObjectTransform;
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.TakeLittleObjectTrigger);
            MovePickableObject();
        }
    }
}