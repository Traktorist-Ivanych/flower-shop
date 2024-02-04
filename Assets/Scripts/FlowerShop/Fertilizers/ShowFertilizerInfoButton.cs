using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.Fertilizers
{
    [RequireComponent(typeof(Button))]
    public class ShowFertilizerInfoButton : MonoBehaviour
    {
        [Inject] private readonly FertilizersCanvasLiaison fertilizersCanvasLiaison;
        
        [SerializeField] private FertilizerInfo fertilizerInfo;
        
        [HideInInspector, SerializeField] private Button showButton;

        private void OnValidate()
        {
            showButton = GetComponent<Button>();
        }

        private void OnEnable()
        {
            showButton.onClick.AddListener(ShowInfoPanel);
        }

        private void OnDisable()
        {
            showButton.onClick.RemoveListener(ShowInfoPanel);
        }

        private void ShowInfoPanel()
        {
            fertilizersCanvasLiaison.ShowFertilizerInfoPanel(fertilizerInfo);
        }
    }
}