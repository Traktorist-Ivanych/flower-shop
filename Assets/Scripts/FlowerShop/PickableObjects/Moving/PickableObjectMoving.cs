using DG.Tweening;
using FlowerShop.Settings;
using UnityEngine;
using Zenject;

namespace FlowerShop.PickableObjects.Moving
{
    public abstract class PickableObjectMoving : MonoBehaviour
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;

        private protected Transform finishTransform;
        
        private protected void MovePickableObject()
        {
            transform.DORotateQuaternion(
                endValue: finishTransform.rotation, 
                duration: actionsWithTransformSettings.MovingObjectsTime);

            transform.DOMove(
                    endValue: finishTransform.position, 
                    duration: actionsWithTransformSettings.MovingObjectsTime)
                .OnComplete(FinishMoving);
        }

        private protected virtual void FinishMoving()
        {
            transform.SetParent(finishTransform);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
        }
    }
}