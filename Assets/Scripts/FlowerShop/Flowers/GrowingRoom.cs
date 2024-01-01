using UnityEngine;

namespace FlowerShop.Flowers
{
    [CreateAssetMenu(fileName = "GrowingRoom", menuName = "Growing Room", order = 58)]
    public class GrowingRoom : ScriptableObject
    {
        [field: SerializeField] public string RoomName { get; private set; }
    }
}