using UnityEngine;

namespace FlowerShop.Flowers
{
    [CreateAssetMenu(fileName = "NewGrowingRoom", menuName = "GrowingRoom", order = 58)]
    public class GrowingRoom : ScriptableObject
    {
        [field: SerializeField] public string RoomName { get; private set; }
    }
}