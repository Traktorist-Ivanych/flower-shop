using UnityEngine;

namespace PlayerControl
{
    public class PlayerPickableObjectHandler : MonoBehaviour
    {
        public bool IsPickableObjectNull => CurrentPickableObject == null;

        public IPickableObject CurrentPickableObject { get; set; }

        public void ClearPickableObject()
        {
            CurrentPickableObject = null;
        }
    }
}
