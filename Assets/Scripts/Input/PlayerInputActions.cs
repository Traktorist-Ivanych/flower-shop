using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputActions : MonoBehaviour
{
    [SerializeField] private CameraMoving cameraMoving;
    [SerializeField] private PlayerTapInput playerTapInput;
    private MobileInputActions inputControls;

    private void Start()
    {
        inputControls = new MobileInputActions();
        inputControls.ActionMap.Enable();

        inputControls.ActionMap.Tap.started += TapInput;
        inputControls.ActionMap.UIButton.started += ButtonsProcessing;
    }

    private void Update()
    {
        if (!IsScreenButtonPressed())
        {
            if (inputControls.ActionMap.Touch1Delta.ReadValue<Vector2>() == Vector2.zero)
            {
                cameraMoving.MoveCameraXZ(inputControls.ActionMap.Touch0Delta.ReadValue<Vector2>());
                cameraMoving.ReserTouchesDistance();
            }
            else
            {
                cameraMoving.MoveCameraY(inputControls.ActionMap.Touch0Position.ReadValue<Vector2>(),
                                         inputControls.ActionMap.Touch1Position.ReadValue<Vector2>());
            }
        }
    }

    private bool IsScreenButtonPressed()
    {
        return inputControls.ActionMap.UIButton.IsPressed();
    }

    private void ButtonsProcessing(InputAction.CallbackContext context)
    {

    }

    private void TapInput(InputAction.CallbackContext context)
    {
        if (!IsScreenButtonPressed())
        {
            playerTapInput.PlayerTap(inputControls.ActionMap.TapPosition.ReadValue<Vector2>());
        }
    }
}
