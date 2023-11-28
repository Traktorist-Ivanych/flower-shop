using UnityEngine;

namespace FlowerShop.Settings
{
    [CreateAssetMenu(fileName = "NewActionsWithTransformSettings", 
                     menuName = "Settings/Actions With Transform", 
                     order = 1)]
    public class ActionsWithTransformSettings : ScriptableObject
    {
        [field: Tooltip("Moving time into player's hands and back to table for all pickable objects.")]
        [field: SerializeField] public float MovingObjectsTime { get; private set; }
    }
}