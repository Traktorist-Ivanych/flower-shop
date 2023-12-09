using System;
using UnityEngine;

namespace FlowerShop.Settings
{
    [CreateAssetMenu(fileName = "NewActionsWithTransformSettings", 
                     menuName = "Settings/Actions With Transform", 
                     order = 1)]
    public class ActionsWithTransformSettings : ScriptableObject
    {
        [field: Tooltip("Time, it takes for moving pickable object into player's hands and back to table " +
                        "Important: depends on PlayerAnimation!")]
        [field: SerializeField] public float MovingPickableObjectTime { get; private set; }
        
        [field: Tooltip("Time delay for successfully moving pickable object into player's hands and back to table")]
        [field: SerializeField] public float MovingPickableObjectTimeDelay { get; private set; }
        
        [field: Tooltip("Time, it takes for rotate object on 360 degrees around it's axis")]
        [field: SerializeField] public float RotationObject360DegreesTime { get; private set; }
        
        [field: Tooltip("Height, to which PickableObject jumps, when PlayerAnimator playing Throw animation")]
        [field: SerializeField] public float PickableObjectDoTweenJumpPower { get; private set; }
        
        [field: Tooltip("Default number of jumps, when DOTween.Jump is used")]
        [field: SerializeField] public int DefaultDoTweenJumpsNumber { get; private set; }
        
        [field: Tooltip("Defines Transform.Rotation (Euler) of indicators, which cannot be rotated")]
        [field: SerializeField] public Vector3 ConstantIndicatorRotation { get; private set; }

        private void OnValidate()
        {
            if (MovingPickableObjectTimeDelay < MovingPickableObjectTime + 0.1f)
            {
                Debug.LogWarning("MovingPickableObjectTimeDelay can't be less then MovingPickableObjectTime + 0.1f");
                MovingPickableObjectTimeDelay = MovingPickableObjectTime + 0.1f;
            }
        }
    }
}