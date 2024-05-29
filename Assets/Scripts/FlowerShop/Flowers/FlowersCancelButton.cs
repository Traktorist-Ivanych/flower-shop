using FlowerShop.ComputerPages;
using FlowerShop.Education;
using Input;
using UnityEngine;
using Zenject;

namespace FlowerShop.Flowers
{
    [RequireComponent(typeof(UIButton))]
    public class FlowersCancelButton : MonoBehaviour
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly FlowersCanvasLiaison flowersCanvasLiaison;
        [Inject] private readonly PlayerInputActions playerInputActions;
        
        [HideInInspector, SerializeField] private UIButton uiButton;

        private void OnValidate()
        {
            uiButton = GetComponent<UIButton>();
        }

        private void OnEnable()
        {
            uiButton.OnClickEvent += OnCancelButtonClick;
        }
        
        private void OnDisable()
        {
            uiButton.OnClickEvent -= OnCancelButtonClick;
        }
        
        private void OnCancelButtonClick()
        {
            playerInputActions.DisableCanvasControlMode();
            flowersCanvasLiaison.HideFlowersCanvas();
            
            if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
            {
                educationHandler.CompleteEducationStep();
            }
        }
    }
}