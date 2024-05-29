using FlowerShop.ComputerPages;
using FlowerShop.Education;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.Fertilizers
{
    [RequireComponent(typeof(UIButton))]
    public class ShowFertilizerInfoButton : MonoBehaviour
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly FertilizersCanvasLiaison fertilizersCanvasLiaison;
        
        [SerializeField] private FertilizerInfo fertilizerInfo;
        
        [HideInInspector, SerializeField] private UIButton showButton;

        private void OnValidate()
        {
            showButton = GetComponent<UIButton>();
        }

        private void OnEnable()
        {
            showButton.OnClickEvent += ShowInfoPanel;
        }

        private void OnDisable()
        {
            showButton.OnClickEvent -= ShowInfoPanel;
        }

        private void ShowInfoPanel()
        {
            if (!educationHandler.IsEducationActive)
            {
                fertilizersCanvasLiaison.ShowFertilizerInfoPanel(fertilizerInfo);
            }
            else if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
            {
                educationHandler.CompleteEducationStep();
                fertilizersCanvasLiaison.ShowFertilizerInfoPanel(fertilizerInfo);
            }
        }
    }
}