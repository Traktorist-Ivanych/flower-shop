using DG.Tweening;
using FlowerShop.Settings;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables.Helpers
{
    public class TableObjectsRotation : MonoBehaviour
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        
        [SerializeField] private Transform[] rotatingObjectsTransform;
        [SerializeField] private Vector3 rotatingEndValue;

        private Tween[] objectsRotators;
        
        private void Awake()
        {
            objectsRotators = new Tween[rotatingObjectsTransform.Length];

            for (int i = 0; i < rotatingObjectsTransform.Length; i++)
            {
                objectsRotators[i] = rotatingObjectsTransform[i].DORotate(
                        endValue: rotatingEndValue,
                        duration: actionsWithTransformSettings.RotationObject360DegreesTime, 
                        mode: RotateMode.WorldAxisAdd)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1);

                objectsRotators[i].Pause();
            }
        }

        public void StartObjectsRotation()
        {
            foreach (Tween objectsRotator in objectsRotators)
            {
                objectsRotator.Play();
            }
        }

        public void PauseObjectsRotation()
        {
            foreach (Tween objectsRotator in objectsRotators)
            {
                objectsRotator.Pause();
            }
        }
    }
}