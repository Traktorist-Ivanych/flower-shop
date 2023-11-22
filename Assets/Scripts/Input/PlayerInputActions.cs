using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputActions : MonoBehaviour
{
    [SerializeField] private CameraMoving cameraMoving;
    [SerializeField] private PlayerTapInput playerTapInput;
    private InputActions inputControls;
    
#if UNITY_EDITOR
    [SerializeField] private bool moveCameraY;
#endif

    private void Awake()
    {
        inputControls = new InputActions();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (moveCameraY)
        {
            cameraMoving.MoveCameraY(inputControls.Player.Touch0Position.ReadValue<Vector2>(),
                Vector2.zero);
            return;
        }
#endif
        
        if (inputControls.Player.Touch1Delta.ReadValue<Vector2>() == Vector2.zero)
        {
            cameraMoving.MoveCameraXZ(inputControls.Player.Touch0Delta.ReadValue<Vector2>());
        }
        else
        {
            cameraMoving.MoveCameraY(inputControls.Player.Touch0Position.ReadValue<Vector2>(),
                inputControls.Player.Touch1Position.ReadValue<Vector2>());
        }
    }

    private void OnEnable()
    {
        inputControls.Player.Enable();
        inputControls.Player.Tap.started += TapInput;
        inputControls.Player.Touch1Position.canceled += ResetTouchesDistance;
    }

    private void OnDisable()
    {
        inputControls.Player.Tap.started -= TapInput;
        inputControls.Player.Touch1Position.canceled -= ResetTouchesDistance;
        inputControls.Player.Disable();
    }

    private void TapInput(InputAction.CallbackContext context)
    {
        playerTapInput.PlayerTap(inputControls.Player.TapPosition.ReadValue<Vector2>());
    }
    
    private void ResetTouchesDistance(InputAction.CallbackContext context)
    {
        cameraMoving.ResetTouchesDistance();
    }
}