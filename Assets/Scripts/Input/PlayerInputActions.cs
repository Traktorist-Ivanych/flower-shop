using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Input
{
    public class PlayerInputActions : MonoBehaviour
    {
        [Inject] private readonly MainCameraMover mainCameraMover;
        [Inject] private readonly PlayerTapInput playerTapInput;

        public delegate void BackCall();
        public event BackCall BackCallEvent;

        private InputActions inputControls;
        private bool isCanvasControlEnable;

        private void Awake()
        {
            inputControls = new InputActions();
        }

        private void OnEnable()
        {
            inputControls.Player.Enable();
            inputControls.Player.Tap.started += TapInput;
            inputControls.Player.UIButton.started += OnUiButtonClick;
            inputControls.Player.Back.started += OnBackCall;
        }

        private void OnDisable()
        {
            inputControls.Player.UIButton.started -= OnUiButtonClick;
            inputControls.Player.Tap.started -= TapInput;
            inputControls.Player.Back.started -= OnBackCall;
            inputControls.Player.Disable();
        }

        private void Update()
        {
            if (isCanvasControlEnable || inputControls.Player.UIButton.IsPressed()) return;
            
            if (inputControls.Player.Touch1Delta.ReadValue<Vector2>() == Vector2.zero)
            {
                mainCameraMover.MoveCameraXZ(inputControls.Player.Touch0Delta.ReadValue<Vector2>());
                mainCameraMover.ResetTouchesDistance();
            }
            else
            {
                mainCameraMover.MoveCameraY(inputControls.Player.Touch0Position.ReadValue<Vector2>(),
                    inputControls.Player.Touch1Position.ReadValue<Vector2>());
            }
        }

        public void DisableCanvasControlMode()
        {
            isCanvasControlEnable = false;
        }
        
        public void EnableCanvasControlMode()
        {
            isCanvasControlEnable = true;
        }

        private void OnUiButtonClick(InputAction.CallbackContext context)
        {
            //EnableCanvasControlMode();
        }

        private void TapInput(InputAction.CallbackContext context)
        {
            if (isCanvasControlEnable || inputControls.Player.UIButton.IsPressed()) return;
            
            playerTapInput.PlayerTap(inputControls.Player.TapPosition.ReadValue<Vector2>());
        }

        private void OnBackCall(InputAction.CallbackContext context)
        {
            BackCallEvent?.Invoke();
        }
    }
}