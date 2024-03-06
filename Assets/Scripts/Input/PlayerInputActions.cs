using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Input
{
    public class PlayerInputActions : MonoBehaviour
    {
        [Inject] private MainCameraMover mainCameraMover;
        [Inject] private PlayerTapInput playerTapInput;
        
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
            inputControls.Player.UIButton.started += EnableCanvasControlMode;
        }

        private void OnDisable()
        {
            inputControls.Player.UIButton.started -= EnableCanvasControlMode;
            inputControls.Player.Tap.started -= TapInput;
            inputControls.Player.Disable();
        }

        private void Update()
        {
            if (isCanvasControlEnable) return;
            
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

        private void EnableCanvasControlMode(InputAction.CallbackContext context)
        {
            isCanvasControlEnable = true;
        }

        private void TapInput(InputAction.CallbackContext context)
        {
            if (isCanvasControlEnable) return;
            
            playerTapInput.PlayerTap(inputControls.Player.TapPosition.ReadValue<Vector2>());
        }
    }
}