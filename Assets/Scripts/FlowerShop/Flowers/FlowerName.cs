using UnityEngine;

namespace FlowerShop.Flowers
{
    [CreateAssetMenu(fileName = "NewFlower", menuName = "Flower", order = 56)]
    public class FlowerName : ScriptableObject
    {
        [field: SerializeField] public string FlowerNameString { get; private set; }
    }
}