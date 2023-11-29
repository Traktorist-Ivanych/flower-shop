using DG.Tweening;
using FlowerShop.Settings;
using PlayerControl;
using UnityEngine;
using Zenject;

public class OldPickableObjectMoving : MonoBehaviour
{
    [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
    [Inject] private readonly PlayerComponents playerComponents;
    [Inject] private readonly PlayerBusyness playerBusyness;

    private Transform finishTransform;

    public bool ShouldPlayerBecomeFree { get; set; }

    private void Start()
    {
        ShouldPlayerBecomeFree = true;
    }

     public void PutBigPickableObjectInPlayerHands()
     {
         finishTransform = playerComponents.PlayerHandsTransform;
         playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.TakeBigObjectTrigger);
         MovePickableObject();
     }

     public void PutBigPickableObjectOnTable(Transform tableTransform)
     {
         playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.GiveBigObjectTrigger);
         finishTransform = tableTransform;
         MovePickableObject();
     }

     private void MovePickableObject()
     {
         transform.DORotateQuaternion(
                 endValue: finishTransform.rotation, 
                 duration: actionsWithTransformSettings.MovingObjectsTime);

         transform.DOMove(
                 endValue: finishTransform.position, 
                 duration: actionsWithTransformSettings.MovingObjectsTime)
             .OnComplete(FinishMoving);
     }

     private void FinishMoving()
     {
         transform.SetParent(finishTransform);
         transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));

        if (ShouldPlayerBecomeFree)
        {
            playerBusyness.SetPlayerFree();
        }

        ShouldPlayerBecomeFree = true;
    }
}
