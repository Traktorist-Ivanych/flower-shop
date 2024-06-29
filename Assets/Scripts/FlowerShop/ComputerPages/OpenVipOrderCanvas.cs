using FlowerShop.ComputerPages;
using FlowerShop.Education;
using UnityEngine;
using Zenject;

namespace FlowerShop.Sounds
{
    [RequireComponent(typeof(UIButton))]
    public class OpenVipOrderCanvas : MonoBehaviour
    {
        [Inject] private readonly VipCanvasLiaison vipCanvasLiaison;
        [Inject] private readonly EducationHandler educationHandler;

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
            if (educationHandler.IsEducationActive)
            {
                if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
                {
                    educationHandler.CompleteEducationStep();
                }
            }

            vipCanvasLiaison.VipCanvas.enabled = true;
            vipCanvasLiaison.HideIndicator();
        }
    }
}

