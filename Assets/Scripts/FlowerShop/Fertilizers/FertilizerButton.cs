using FlowerShop.ComputerPages;
using FlowerShop.Education;
using FlowerShop.Tables;
using UnityEngine;
using Zenject;

namespace FlowerShop.Fertilizers
{
    [RequireComponent(typeof(UIButton))]
    public class FertilizerButton : MonoBehaviour
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly FertilizersCanvasLiaison fertilizersCanvasLiaison;
        [Inject] private readonly FertilizersTable fertilizersTable;

        [SerializeField] private Fertilizer fertilizer;
        
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
            if (!educationHandler.IsEducationActive)
            {
                TryTakeFertilizerInPlayerHands();
            }
            else if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
            {
                educationHandler.CompleteEducationStep();
                TryTakeFertilizerInPlayerHands();
            }
        }

        private void TryTakeFertilizerInPlayerHands()
        {
            if (fertilizer.AvailableUsesNumber > 0)
            {
                fertilizersTable.TakeFertilizerInPlayerHands(fertilizer);
                fertilizersCanvasLiaison.DisableCanvas();
            }
        }
    }
}