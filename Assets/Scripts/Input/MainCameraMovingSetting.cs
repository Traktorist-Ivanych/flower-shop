using UnityEngine;

namespace Input
{
    [CreateAssetMenu(fileName = "NewMainCameraMovingSetting", 
                     menuName = "Settings/Main Camera Moving", 
                     order = 2)]
    public class MainCameraMovingSetting : ScriptableObject
    {
        [field: SerializeField] public float InputScaleXZ { get; private set; }
        [field: SerializeField] public float InputScaleY { get; private set; }
        [field: SerializeField] public float MinPositionX { get; private set; }
        [field: SerializeField] public float MaxPositionX { get; private set; }
        [field: SerializeField] public float MinPositionY { get; private set; }
        [field: SerializeField] public float MaxPositionY { get; private set; }
        [field: SerializeField] public float MinPositionZ { get; private set; }
        [field: SerializeField] public float MaxPositionZ { get; private set; }
        [field: SerializeField] public float DefaultCameraPositionY { get; private set; }

        private void OnValidate()
        {
            if (MinPositionX > MaxPositionX)
            {
                Debug.LogWarning("MinPositionX can't be more then MaxPositionX");
                MinPositionX = MaxPositionX;
            }
            if (MinPositionY > MaxPositionY)
            {
                Debug.LogWarning("MinPositionY can't be more then MaxPositionY");
                MinPositionY = MaxPositionY;
            }
            if (MinPositionZ > MaxPositionZ)
            {
                Debug.LogWarning("MinPositionZ can't be more then MaxPositionZ");
                MinPositionX = MaxPositionX;
            }
        }
    }
}