using UnityEngine;

namespace PlayerControl
{
    public class PlayerComponents : MonoBehaviour
    {
        [field: SerializeField] public Animator PlayerAnimator { get; private set; }

        [field: SerializeField] public Transform PlayerHandsTransform { get; private set; }

        [field: SerializeField] public Transform PlayerHandsForLittleObjectTransform { get; private set; }
    }
}
