using FlowerShop.Education;
using FlowerShop.Tables;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.Fertilizers
{
    [RequireComponent(typeof(Button))]
    public class FertilizerButton : MonoBehaviour
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly FertilizersCanvasLiaison fertilizersCanvasLiaison;
        [Inject] private readonly FertilizersTable fertilizersTable;

        [SerializeField] private Fertilizer fertilizer;
        
        [HideInInspector, SerializeField] private Button fertilizerButton;

        private void OnValidate()
        {
            fertilizerButton = GetComponent<Button>();
        }

        private void OnEnable()
        {
            fertilizerButton.onClick.AddListener(OnFertilizerButtonClick);
        }

        private void OnDisable()
        {
            fertilizerButton.onClick.RemoveListener(OnFertilizerButtonClick);
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