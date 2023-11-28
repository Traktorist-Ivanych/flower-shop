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
    }
}