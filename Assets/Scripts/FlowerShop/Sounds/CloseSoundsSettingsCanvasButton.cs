using FlowerShop.ComputerPages;
using FlowerShop.Education;
using Input;
using UnityEngine;
using Zenject;

namespace FlowerShop.Sounds
{
    [RequireComponent(typeof(UIButton))]
    public class CloseSoundsSettingsCanvasButton : MonoBehaviour
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly PlayerInputActions playerInputActions;
        [Inject] private readonly SoundsSettingsCanvasLiaison soundsSettingsCanvasLiaison;

        [HideInInspector, SerializeField] private UIButton uIButton;

        [SerializeField] private OpenSoundsSettingsCanvasButton openSoundsSettingsCanvasButton;

        private void OnValidate()
        {
            uIButton = GetComponent<UIButton>();
        }

        private void OnEnable()
        {
            uIButton.OnClickEvent += OnButtonClick;
        }

        private void OnDisable()
        {
            uIButton.OnClickEvent -= OnButtonClick;
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
            soundsSettingsCanvasLiaison.HideCanvas();
            playerInputActions.DisableCanvasControlMode();

            openSoundsSettingsCanvasButton.TryToActivateScaler();
        }
    }
}
