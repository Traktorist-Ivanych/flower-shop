using DG.Tweening;
using FlowerShop.Settings;
using UnityEngine;
using Zenject;

namespace FlowerShop.PickableObjects.Moving
{
    public abstract class PickableObjectMoving : MonoBehaviour
    {
        [Inject] private protected readonly ActionsWithTransformSettings actionsWithTransformSettings;

        private protected Transform finishTransform;
        
        private protected virtual void MovePickableObject()
        {
            transform.DOMove(
                    endValue: finishTransform.position, 
                    duration: actionsWithTransformSettings.MovingObjectsTime)
                .OnComplete(FinishMoving);
        }

        private protected virtual void FinishMoving()
        {
            transform.SetParent(finishTransform);
            transform.localPosition = Vector3.zero;
        }
    }
}