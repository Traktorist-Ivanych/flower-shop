using UnityEngine;
using Zenject;

namespace FlowerShop.Effects
{
    public class CanvasElementScaler : MonoBehaviour
    {
        [Inject] private readonly EffectsSettings effectsSettings;

        [HideInInspector, SerializeField] private RectTransform rectTransform;

        private Vector3 targetEndScale;
        private float currentScalingTime;
        private bool isScaleIncrease;
        private bool isEffectActive;
        private bool shouldEffectActive;

        private void OnValidate()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (isEffectActive)
            {
                currentScalingTime += Time.deltaTime;

                if (currentScalingTime >= effectsSettings.ScalingTime)
                {
                    currentScalingTime = 0;
                    isScaleIncrease = !isScaleIncrease;

                    if (isScaleIncrease && !shouldEffectActive)
                    {
                        isEffectActive = false;
                    }
                }

                float currentLerpCoeff = currentScalingTime / effectsSettings.ScalingTime;

                if (isScaleIncrease)
                {
                    rectTransform.localScale = Vector3.Lerp(effectsSettings.StartScale, targetEndScale, currentLerpCoeff);
                }
                else
                {
                    rectTransform.localScale = Vector3.Lerp(targetEndScale, effectsSettings.StartScale, currentLerpCoeff);
                }
            }
        }

        public void ActivateEffect()
        {
            isEffectActive = true;
            isScaleIncrease = true;
            shouldEffectActive = true;
            targetEndScale = effectsSettings.EndScale;
        }

        public void ActivateEffect(Vector3 overrideTargetEndScale)
        {
            isEffectActive = true;
            isScaleIncrease = true;
            shouldEffectActive = true;
            targetEndScale = overrideTargetEndScale;
        }

        public void DeactivateEffect()
        {
            shouldEffectActive = false;
        }
    }
}
