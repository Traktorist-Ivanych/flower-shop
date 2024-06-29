using UnityEngine;

namespace Input
{
    public class CameraHandler : MonoBehaviour
    {
        [field: SerializeField] public Camera MainCamera { get; private set; }
        [field: SerializeField] public Transform MainCameraPointerTarget { get; private set; }
    }
}