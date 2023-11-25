using UnityEngine;

namespace Input
{
    [CreateAssetMenu(fileName = "NewPlayerTapSettings", menuName = "Player Tap Settings", order = 55)]
    public class PlayerTapSettings : ScriptableObject
    {
        [field: SerializeField] public LayerMask InteractionLayerMask { get; private set; }
        [field: SerializeField] public float MaxInteractionRayDistance { get; private set; }
    }
}