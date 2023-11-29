using DG.Tweening;
using UnityEngine;

namespace FlowerShop.PickableObjects.Moving
{
    public abstract class PickableObjectMovingAndRotating : PickableObjectMoving
    {
        private protected override void MovePickableObject()
        {
            base.MovePickableObject();
            
            transform.DORotateQuaternion(
                endValue: finishTransform.rotation, 
                duration: actionsWithTransformSettings.MovingObjectsTime);
        }
        
        private protected override void FinishMoving()
        {
            base.FinishMoving();
            
            transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }
}