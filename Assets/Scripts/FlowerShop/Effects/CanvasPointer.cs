using Input;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.Effects
{
    public class CanvasPointer : MonoBehaviour
    {
        [Inject] private readonly CameraHandler cameraHandler;
        [Inject] private readonly PlayerStatsCanvasLiaison playerStatsCanvasLiaison;

        private Transform targetTransform;

        [field: SerializeField] public bool IsCanvasPointerActive { get; private set; }

        private void Update()
        {
            if (!IsCanvasPointerActive) return;

            Vector3 pointerDuration = targetTransform.position - cameraHandler.MainCameraPointerTarget.transform.position;
            Ray ray = new Ray(cameraHandler.MainCameraPointerTarget.position, pointerDuration);

            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cameraHandler.MainCamera);

            float minDistance = float.MaxValue;
            int planeIndex = 0;

            for (int i = 0; i < 5; i++)
            {
                if (planes[i].Raycast(ray, out float distance))
                {
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        planeIndex = i;
                    }
                }
            }

            if (planeIndex == 4) 
            {
                playerStatsCanvasLiaison.PointerImage.enabled = false;
            }
            else
            {
                playerStatsCanvasLiaison.PointerImage.enabled = true;

                //minDistance = Mathf.Clamp(minDistance, 0, pointerDuration.magnitude);

                Vector3 worldPosition = ray.GetPoint(minDistance);

                playerStatsCanvasLiaison.PointerTransform.SetPositionAndRotation(
                    position: cameraHandler.MainCamera.WorldToScreenPoint(worldPosition),
                    rotation: GetIconRotation(planeIndex));
            }
        }

        public void EnableCanvasPointer(Transform newTargetTransform)
        {
            targetTransform = newTargetTransform;
            IsCanvasPointerActive = true;
        }

        public void DisableCanvasPointer() 
        {
            IsCanvasPointerActive = false;
        }

        private Quaternion GetIconRotation(int planeIndex)
        {
            switch (planeIndex)
            {
                case 0:
                    return Quaternion.Euler(0, 0, 90);
                case 1:
                    return Quaternion.Euler(0, 0, -90);
                case 2:
                    return Quaternion.Euler(0, 0, 180);
                case 3:
                    return Quaternion.Euler(0, 0, 0);
                default:
                    return Quaternion.identity;
            }
        }
    }
}
