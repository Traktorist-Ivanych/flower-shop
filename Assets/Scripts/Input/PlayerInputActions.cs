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

        private void Awake()
        {
            inputControls = new InputActions();
        }

        private void Update()
        {
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

        private void OnEnable()
        {
            inputControls.Player.Enable();
            inputControls.Player.Tap.started += TapInput;
        }

        private void OnDisable()
        {
            inputControls.Player.Tap.started -= TapInput;
            inputControls.Player.Disable();
        }

        private void TapInput(InputAction.CallbackContext context)
        {
            playerTapInput.PlayerTap(inputControls.Player.TapPosition.ReadValue<Vector2>());
        }
    }
}