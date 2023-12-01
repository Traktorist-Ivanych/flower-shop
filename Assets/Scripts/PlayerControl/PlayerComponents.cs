using UnityEngine;

namespace PlayerControl
{
    public class PlayerComponents : MonoBehaviour
    {
        [field: SerializeField] public Animator PlayerAnimator { get; private set; }

        [field: SerializeField] public Transform PlayerHandsForBigObjectTransform { get; private set; }

        [field: SerializeField] public Transform PlayerHandsForLittleObjectTransform { get; private set; }
        
        [field: SerializeField] public Transform PlayerHandsForCoffeeTransform { get; private set; }
    }
}
