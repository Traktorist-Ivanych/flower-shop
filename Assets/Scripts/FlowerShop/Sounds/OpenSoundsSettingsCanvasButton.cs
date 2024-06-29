using FlowerShop.ComputerPages;
using FlowerShop.Education;
using FlowerShop.Effects;
using Input;
using UnityEngine;
using Zenject;

namespace FlowerShop.Sounds
{
    [RequireComponent(typeof(UIButton))]
    public class OpenSoundsSettingsCanvasButton : MonoBehaviour
    {
        [Inject] private readonly PlayerInputActions playerInputActions;
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly SoundsSettingsCanvasLiaison soundsSettingsCanvasLiaison;

        [HideInInspector, SerializeField] private UIButton uIButton;
        [HideInInspector, SerializeField] private CanvasElementScaler canvasElementScaler;

        private void OnValidate()
        {
            uIButton = GetComponent<UIButton>();
            canvasElementScaler = GetComponent<CanvasElementScaler>();
        }

        private void OnEnable()
        {
            uIButton.OnClickEvent += OnButtonClick;
        }

        private void OnDisable()
        {
            uIButton.OnClickEvent -= OnButtonClick;
        }

        public void TryToActivateScaler()
        {
            if (educationHandler.IsEducationActive && educationHandler.Step == 181)
            {
                canvasElementScaler.ActivateEffect();
            }
        }

        private void OnButtonClick()
        {
            if (educationHandler.IsEducationActive)
            {
                if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
                {
                    educationHandler.CompleteEducationStep();
                }
            }

            soundsSettingsCanvasLiaison.ShowCanvas();
            playerInputActions.EnableCanvasControlMode();

            if (educationHandler.IsEducationActive && educationHandler.Step == 181)
            {
                canvasElementScaler.DeactivateEffect();
            }
        }


    }
}
