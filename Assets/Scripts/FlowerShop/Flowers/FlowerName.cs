using UnityEngine;

namespace FlowerShop.Flowers
{
    [CreateAssetMenu(fileName = "NewFlower", menuName = "Flower", order = 56)]
    public class Flower : ScriptableObject
    {
        [field: SerializeField] public string FlowerName { get; private set; }
    }
}