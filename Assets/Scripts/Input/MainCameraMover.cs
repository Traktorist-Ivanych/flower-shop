using UnityEngine;
using Zenject;

namespace Input
{
    public class MainCameraMover : MonoBehaviour
    {
        [Inject] private CameraHandler cameraHandler;
        [Inject] private MainCameraMovingSetting mainCameraMovingSetting;
        
        private Camera cameraForMoving;
        private float cameraStartYPosition;
        private float scaleXZWithCameraZPosition;
        private float resolutionScaleX;
        private float resolutionScaleY;
        private float pastTouchesDistance;

        private void Start()
        {
            cameraForMoving = cameraHandler.MainCamera;
            scaleXZWithCameraZPosition = 1;
            cameraStartYPosition = cameraForMoving.transform.position.y;
            resolutionScaleX = 360f / Screen.width;
            resolutionScaleY = 740f / Screen.height;
        }

        public void MoveCameraXZ(Vector2 touch0Delta)
        {
            Vector3 cameraPosition = cameraForMoving.transform.position;
        
            float touch0DeltaX = touch0Delta.x * resolutionScaleX * 
                                 mainCameraMovingSetting.InputScaleXZ * scaleXZWithCameraZPosition;
            float cameraPositionX = Mathf.Clamp(cameraPosition.x - touch0DeltaX,
                                                min: mainCameraMovingSetting.MinPositionX, 
                                                max: mainCameraMovingSetting.MaxPositionX);

            float touch0DeltaZ = touch0Delta.y * resolutionScaleY *
                                 mainCameraMovingSetting.InputScaleXZ * scaleXZWithCameraZPosition;
            float cameraPositionZ = Mathf.Clamp(cameraPosition.z - touch0DeltaZ, 
                                                min: mainCameraMovingSetting.MinPositionZ, 
                                                max: mainCameraMovingSetting.MaxPositionZ);

            Vector3 cameraTargetPosition = new Vector3(cameraPositionX, cameraPosition.y, cameraPositionZ);
            UpdateCameraPosition(cameraTargetPosition);
        }

        public void MoveCameraY(Vector2 touch0Position, Vector2 touch1Position)
        {
            Vector2 scaledTouch0Position = new (touch0Position.x * resolutionScaleX, touch0Position.y * resolutionScaleY);
            Vector2 scaledTouch1Position = new (touch1Position.x * resolutionScaleX, touch1Position.y * resolutionScaleY);

            float currentTouchesDistance = Vector2.Distance(scaledTouch0Position, scaledTouch1Position);

            if (pastTouchesDistance != 0)
            {
                Vector3 cameraPosition = cameraForMoving.transform.position;
            
                float touchesDistanceDelta = currentTouchesDistance - pastTouchesDistance;
                float calculatedCameraY = cameraPosition.y - touchesDistanceDelta * mainCameraMovingSetting.InputScaleY;

                calculatedCameraY = Mathf.Clamp(calculatedCameraY, 
                                                min: mainCameraMovingSetting.MinPositionY,
                                                max: mainCameraMovingSetting.MaxPositionY);
                
                Vector3 cameraTargetPosition = new Vector3(cameraPosition.x, calculatedCameraY, cameraPosition.z);
                UpdateCameraPosition(cameraTargetPosition);
            }
            pastTouchesDistance = currentTouchesDistance;

            scaleXZWithCameraZPosition = cameraForMoving.transform.position.y / cameraStartYPosition;
        }

        private void UpdateCameraPosition(Vector3 targetCameraPosition)
        {
            cameraForMoving.transform.position = targetCameraPosition;
        }

        public void ResetTouchesDistance()
        {
            pastTouchesDistance = 0;
        }
    }
}
