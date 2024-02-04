using UnityEngine;

namespace Input
{
    public class MainCameraMovingBorders : MonoBehaviour
    {
        [SerializeField] private MainCameraMovingSetting mainCameraMovingSetting;
        
        private void OnDrawGizmos()
        {
            float wireCubeSizeX = mainCameraMovingSetting.MaxPositionX - mainCameraMovingSetting.MinPositionX;
            float wireCubeSizeY = mainCameraMovingSetting.MaxPositionY - mainCameraMovingSetting.MinPositionY;
            float wireCubeSizeZ = mainCameraMovingSetting.MaxPositionZ - mainCameraMovingSetting.MinPositionZ;
            Vector3 wireCubeSize = new(wireCubeSizeX, wireCubeSizeY, wireCubeSizeZ);

            float wireCubeCenterX = mainCameraMovingSetting.MinPositionX + wireCubeSizeX / 2;
            float wireCubeCenterY = mainCameraMovingSetting.MinPositionY + wireCubeSizeY / 2;
            float wireCubeCenterZ = mainCameraMovingSetting.MinPositionZ + wireCubeSizeZ / 2;
            Vector3 wireCubeCenter = new(wireCubeCenterX, wireCubeCenterY, wireCubeCenterZ);
            
            Gizmos.DrawWireCube(wireCubeCenter, wireCubeSize);
        }
    }
}