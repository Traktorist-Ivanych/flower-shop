
using UnityEngine;

public class PlayerComponents : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Transform playerHandsTransform;
    [SerializeField] private Transform playerHandsForLittleObjectTransform;

    public Animator PlayerAnimator 
    {
        get => playerAnimator;
    }

    public Transform PlayerHandsTransform
    {
        get => playerHandsTransform;
    }

    public Transform PlayerHandsForLittleObjectTransform
    {
        get => playerHandsForLittleObjectTransform;
    }
}
