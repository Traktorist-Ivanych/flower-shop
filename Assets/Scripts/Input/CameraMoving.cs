using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    [SerializeField] private float inputScaleXZ;
    [SerializeField] private float inputScaleY;
    [SerializeField] private float maxPositionX;
    [SerializeField] private float minPositionX;
    [SerializeField] private float maxPositionY;
    [SerializeField] private float minPositionY;
    [SerializeField] private float maxPositionZ;
    [SerializeField] private float minPositionZ;
    private Camera mainCamera;
    private float cameraStartYPosition;
    private float scaleXZWithCameraZPosition;
    private float resolutionScaleX;
    private float resolutionScaleY;
    private float pastTouchesDistance;

    private void Start()
    {
        scaleXZWithCameraZPosition = 1;
        mainCamera = Camera.main;
        cameraStartYPosition = mainCamera.transform.position.y;
        resolutionScaleX = 360f / Screen.width;
        resolutionScaleY = 740f / Screen.height;
    }

    public void MoveCameraXZ(Vector2 touch0Delta)
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        
        float touch0DeltaX = touch0Delta.x * resolutionScaleX * inputScaleXZ * scaleXZWithCameraZPosition;
        float cameraPositionX = Mathf.Clamp(cameraPosition.x - touch0DeltaX, minPositionX, maxPositionX);

        float touch0DeltaZ = touch0Delta.y * resolutionScaleY * inputScaleXZ * scaleXZWithCameraZPosition;
        float cameraPositionZ = Mathf.Clamp(cameraPosition.z - touch0DeltaZ, minPositionZ, maxPositionZ);

        Vector3 cameraTargetPosition = new Vector3(cameraPositionX, cameraPosition.y, cameraPositionZ);
        UpdateCameraPosition(cameraTargetPosition);
    }

    // it should on pc as well for fast testing and etc - add logic in defines UNITY_EDITOR
    public void MoveCameraY(Vector2 touch0Position, Vector2 touch1Position)
    {
        Vector2 scaledTouch0Position = new (touch0Position.x * resolutionScaleX, touch0Position.y * resolutionScaleY);
        Vector2 scaledTouch1Position = new (touch1Position.x * resolutionScaleX, touch1Position.y * resolutionScaleY);

        float currentTouchesDistance = Vector2.Distance(scaledTouch0Position, scaledTouch1Position);

        if (pastTouchesDistance != 0)
        {
            Vector3 cameraPosition = mainCamera.transform.position;
            
            float touchesDistanceDelta = currentTouchesDistance - pastTouchesDistance;
            float targetCameraY = Mathf.Clamp(cameraPosition.y - touchesDistanceDelta * inputScaleY,
                                              minPositionY, maxPositionY);

            Vector3 cameraTargetPosition = new Vector3(cameraPosition.x, targetCameraY, cameraPosition.z);
            UpdateCameraPosition(cameraTargetPosition);
        }
        pastTouchesDistance = currentTouchesDistance;

        scaleXZWithCameraZPosition = mainCamera.transform.position.y / cameraStartYPosition;
    }

    private void UpdateCameraPosition(Vector3 targetCameraPosition)
    {
        mainCamera.transform.position = targetCameraPosition;
    }

    public void ResetTouchesDistance()
    {
        pastTouchesDistance = 0;
    }
}
