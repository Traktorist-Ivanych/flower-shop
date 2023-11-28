using UnityEngine;

namespace Input
{
    [CreateAssetMenu(fileName = "NewPlayerTapSettings", 
                     menuName = "Settings/Player Tap", 
                     order = 4)]
    public class PlayerTapSettings : ScriptableObject
    {
        [field: SerializeField] public LayerMask InteractionLayerMask { get; private set; }
        [field: SerializeField] public float MaxInteractionRayDistance { get; private set; }
    }
}