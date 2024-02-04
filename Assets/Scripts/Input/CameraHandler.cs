using UnityEngine;

namespace Input
{
    public class CameraHandler : MonoBehaviour
    {
        [field: SerializeField] public Camera MainCamera { get; private set; }
    }
}