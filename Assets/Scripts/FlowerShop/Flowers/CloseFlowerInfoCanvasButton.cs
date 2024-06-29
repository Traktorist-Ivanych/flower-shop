using FlowerShop.ComputerPages;
using FlowerShop.Education;
using UnityEngine;
using Zenject;

namespace FlowerShop.Flowers
{
    [RequireComponent(typeof(UIButton))]
    public class CloseFlowerInfoCanvasButton : MonoBehaviour
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly FlowerInfoCanvasLiaison flowerInfoCanvasLiaison;

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
            flowerInfoCanvasLiaison.FlowerInfoCanvas.enabled = false;

            if (educationHandler.IsEducationActive)
            {
                if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
                {
                    educationHandler.CompleteEducationStep();
                }
            }
        }
    }
}
