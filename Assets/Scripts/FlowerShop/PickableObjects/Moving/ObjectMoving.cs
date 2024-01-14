﻿using DG.Tweening;
using FlowerShop.Settings;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.PickableObjects.Moving
{
    public class ObjectMoving : MonoBehaviour
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly PlayerBusyness playerBusyness;

        private Transform finishTransform;

        private bool shouldPlayerBecomeFree;

        public void MoveObject(Transform targetFinishTransform, string movingObjectAnimatorTrigger, bool setPlayerFree)
        {
            shouldPlayerBecomeFree = setPlayerFree;
            finishTransform = targetFinishTransform;
            playerComponents.PlayerAnimator.SetTrigger(movingObjectAnimatorTrigger);
            MoveAndRotateObjectWithDoTween();
        }

        public void SetParentAndParentPositionAndRotationOnLoad(Transform targetFinishTransform)
        {
            transform.SetParent(targetFinishTransform);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
        }

        private void MoveAndRotateObjectWithDoTween()
        {
            transform.DOMove(
                    endValue: finishTransform.position, 
                    duration: actionsWithTransformSettings.MovingPickableObjectTime)
                .OnComplete(FinishMoving);
            
            transform.DORotateQuaternion(
                    endValue: finishTransform.rotation, 
                    duration: actionsWithTransformSettings.MovingPickableObjectTime);
        }

        private void FinishMoving()
        {
            transform.SetParent(finishTransform);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));

            if (shouldPlayerBecomeFree)
            {
                playerBusyness.SetPlayerFree();
            }
        }
    }
}