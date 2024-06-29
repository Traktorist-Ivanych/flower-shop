using Input;
using UnityEngine;
using Zenject;

namespace FlowerShop.ComputerPages
{
    [RequireComponent(typeof(UIButton))]
    public class EnableCanvasControlButton : MonoBehaviour
    {
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
            playerInputActions.EnableCanvasControlMode();
        }
    }
}
