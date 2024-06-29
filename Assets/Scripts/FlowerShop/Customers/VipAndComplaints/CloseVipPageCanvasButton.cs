using FlowerShop.ComputerPages;
using FlowerShop.Education;
using UnityEngine;
using Zenject;

namespace FlowerShop.Customers.VipAndComplaints
{
    [RequireComponent(typeof(UIButton))]
    public class CloseVipPageCanvasButton : MonoBehaviour
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly VipCanvasLiaison vipCanvasLiaison;

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

            vipCanvasLiaison.VipCanvas.enabled = false;
        }
    }
}
