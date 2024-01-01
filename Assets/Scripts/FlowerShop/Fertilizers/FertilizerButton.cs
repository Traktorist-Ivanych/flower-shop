using FlowerShop.Tables;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.Fertilizers
{
    [RequireComponent(typeof(Button))]
    public class FertilizerButton : MonoBehaviour
    {
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
            if (fertilizer.AvailableUsesNumber > 0)
            {
                fertilizersTable.TakeFertilizerInPlayerHands(fertilizer);
                fertilizersCanvasLiaison.FertilizersCanvas.enabled = false;
            }
        }
    }
}