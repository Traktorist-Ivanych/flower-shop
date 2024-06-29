using FlowerShop.ComputerPages;
using FlowerShop.Education;
using UnityEngine;
using Zenject;

namespace FlowerShop.Fertilizers
{
    [RequireComponent(typeof(UIButton))]
    public class CloseFertilizerInfoPanelButton : MonoBehaviour
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly FertilizersCanvasLiaison fertilizersCanvasLiaison;

        [HideInInspector, SerializeField] private UIButton fertilizerButton;

        private void OnValidate()
        {
            fertilizerButton = GetComponent<UIButton>();
        }

        private void OnEnable()
        {
            fertilizerButton.OnClickEvent += OnFertilizerButtonClick;
        }

        private void OnDisable()
        {
            fertilizerButton.OnClickEvent -= OnFertilizerButtonClick;
        }

        private void OnFertilizerButtonClick()
        {
            if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
            {
                educationHandler.CompleteEducationStep();
            }

            fertilizersCanvasLiaison.HideFertilizerInfoPanel();
        }
    }
}
