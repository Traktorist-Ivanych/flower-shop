using Input;
using UnityEngine;
using Zenject;

namespace FlowerShop.ComputerPages
{
    [RequireComponent(typeof(UIButton))]
    public class EnableCanvasControlModeOnButtonClick : MonoBehaviour
    {
        [Inject] private readonly PlayerInputActions playerInputActions;

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
            playerInputActions.EnableCanvasControlMode();
        }
    }
}
