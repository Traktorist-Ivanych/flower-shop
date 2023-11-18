using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    [SerializeField] private float cameraStartYPosition;
    [SerializeField] private float inputScaleXZ;
    [SerializeField] private float inputScaleY;
    [SerializeField] private float maxPositionX;
    [SerializeField] private float minPositionX;
    [SerializeField] private float maxPositionY;
    [SerializeField] private float minPositionY;
    [SerializeField] private float maxPositionZ;
    [SerializeField] private float minPositionZ;
    private Camera mainCamera;
    private Vector3 targetPosition;
    private float scaleXZWithCameraZPosition;
    private float resolutionScaleX;
    private float resolutionScaleY;
    private float pastTouchesDistance;

    private void Start()
    {
        scaleXZWithCameraZPosition = 1;
        mainCamera = Camera.main;
        targetPosition = mainCamera.transform.position;
        resolutionScaleX = 360f / Screen.width;
        resolutionScaleY = 740f / Screen.height;
    }

    private void Update()
    {
        mainCamera.transform.position = targetPosition;
    }

    public void MoveCameraXZ(Vector2 touch0Delta)
    {
        float touch0DeltaX = touch0Delta.x * resolutionScaleX * inputScaleXZ * scaleXZWithCameraZPosition;
        float cameraPositionX = Mathf.Clamp(mainCamera.transform.position.x - touch0DeltaX, minPositionX, maxPositionX);

        float touch0DeltaZ = touch0Delta.y * resolutionScaleY * inputScaleXZ * scaleXZWithCameraZPosition;
        float cameraPositionZ = Mathf.Clamp(mainCamera.transform.position.z - touch0DeltaZ, minPositionZ, maxPositionZ);

        targetPosition = new Vector3(cameraPositionX, mainCamera.transform.position.y, cameraPositionZ);
    }

    // it should on pc as well for fast testing and etc - add logic in defines UNITY_EDITOR
    public void MoveCameraY(Vector2 touch0Position, Vector2 touch1Position)
    {
        Vector2 scaledTouch0Position = new (touch0Position.x * resolutionScaleX, touch0Position.y * resolutionScaleY);
        Vector2 scaledTouch1Position = new (touch1Position.x * resolutionScaleX, touch1Position.y * resolutionScaleY);

        float currentTouchesDistance = Vector2.Distance(scaledTouch0Position, scaledTouch1Position);

        if (pastTouchesDistance != 0)
        {
            float touchesDistanceDelta = currentTouchesDistance - pastTouchesDistance;
            float targetCameraY = Mathf.Clamp(mainCamera.transform.position.y - touchesDistanceDelta * inputScaleY,
                                              minPositionY, maxPositionY);

            targetPosition = new Vector3(mainCamera.transform.position.x, targetCameraY, mainCamera.transform.position.z);
        }
        pastTouchesDistance = currentTouchesDistance;

        scaleXZWithCameraZPosition = mainCamera.transform.position.y / cameraStartYPosition;
    }

    public void ReserTouchesDistance()
    {
        pastTouchesDistance = 0;
    }
}
