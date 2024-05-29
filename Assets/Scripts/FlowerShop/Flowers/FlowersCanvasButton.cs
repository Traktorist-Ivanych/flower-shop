using FlowerShop.ComputerPages;
using FlowerShop.Education;
using FlowerShop.Sounds;
using UnityEngine;
using Zenject;

namespace FlowerShop.Flowers
{
    [RequireComponent(typeof(UIButton))]
    public class FlowersCanvasButton : MonoBehaviour
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly FlowersCanvasLiaison flowersCanvasLiaison;
        
        [HideInInspector, SerializeField] private UIButton uiButton;

        private void OnValidate()
        {
            uiButton = GetComponent<UIButton>();
        }

        private void OnEnable()
        {
            uiButton.OnClickEvent += OnButtonClick;
        }

        private void OnDisable()
        {
            uiButton.OnClickEvent -= OnButtonClick;
        }

        private void OnButtonClick()
        {
            flowersCanvasLiaison.ShowFlowersCanvas();
            
            if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
            {
                educationHandler.CompleteEducationStep();
            }
        }
    }
}