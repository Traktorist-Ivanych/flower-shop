using FlowerShop.ComputerPages;
using Input;
using UnityEngine;
using Zenject;

namespace FlowerShop.Sounds
{
    [RequireComponent(typeof(UIButton))]
    public class CloseSoundsSettingsCanvasButton : MonoBehaviour
    {
        [Inject] private readonly PlayerInputActions playerInputActions;
        [Inject] private readonly SoundsSettingsCanvasLiaison soundsSettingsCanvasLiaison;

        [HideInInspector, SerializeField] private UIButton uIButton;

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
            soundsSettingsCanvasLiaison.HideCanvas();
            playerInputActions.DisableCanvasControlMode();
        }
    }
}
