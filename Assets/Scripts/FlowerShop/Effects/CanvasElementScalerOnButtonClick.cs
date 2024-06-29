using DG.Tweening;
using FlowerShop.ComputerPages;
using UnityEngine;
using Zenject;

namespace FlowerShop.Effects
{
    [RequireComponent(typeof(UIButton))]
    public class CanvasElementScalerOnButtonClick : MonoBehaviour
    {
        [Inject] private readonly EffectsSettings effectsSettings;

        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Vector3 startScale;
        [SerializeField] private Vector3 endScale;

        private bool isScalingEnable;

        [HideInInspector, SerializeField] private UIButton button;

        private void OnValidate()
        {
            button = GetComponent<UIButton>();
        }

        private void OnEnable()
        {
            button.OnClickEvent += OnButtonClick;
        }

        private void OnDisable()
        {
            button.OnClickEvent -= OnButtonClick;
        }

        private void OnButtonClick()
        {
            if (!isScalingEnable)
            {
                isScalingEnable = true;
                ScaleElementToEndValue();
            }
        }

        private void ScaleElementToEndValue()
        {
            rectTransform.DOScale(endScale, effectsSettings.OnButtonClickScalingTime).OnComplete(ScaleElementToStartValue);
        }

        private void ScaleElementToStartValue()
        {
            rectTransform.DOScale(startScale, effectsSettings.OnButtonClickScalingTime).OnComplete(
                () => isScalingEnable = false);
        }
    }
}
