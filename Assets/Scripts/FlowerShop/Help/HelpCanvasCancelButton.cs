using FlowerShop.ComputerPages;
using FlowerShop.Education;
using Input;
using UnityEngine;
using Zenject;

namespace FlowerShop.Help
{
    [RequireComponent(typeof(UIButton))]
    public class HelpCanvasCancelButton : MonoBehaviour
    {
        [Inject] private readonly EducationHandler educationHandler;

        [Inject] private readonly HelpCanvasLiaison helpCanvasLiaison;
        [Inject] private readonly PlayerInputActions playerInputActions;

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
                    helpCanvasLiaison.HelpCanvas.enabled = false;
                    playerInputActions.DisableCanvasControlMode();
                    educationHandler.CompleteEducationStep();
                }
            }
            else
            {
                helpCanvasLiaison.HelpCanvas.enabled = false;
                playerInputActions.DisableCanvasControlMode();
            }
        }
    }
}

