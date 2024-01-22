using Saves;
using UnityEngine;

namespace FlowerShop.Flowers
{
    [CreateAssetMenu(fileName = "NewFlowerInfo", menuName = "Flower Info", order = 57)]
    public class FlowerInfo : ScriptableObject, IUniqueKey
    {
        [field: SerializeField] public string UniqueKey { get; private set; }
        
        [field: Header("FlowerInfo")]
        [field: SerializeField] public string FlowerNameRus { get; private set; }
        [field: SerializeField] public Sprite FlowerSprite { get; private set; }
        [field: SerializeField] public FlowerName FlowerName { get; private set; }
        [field: SerializeField] public GrowingRoom GrowingRoom { get; private set; }
        [field: SerializeField] public int FlowerLvl { get; private set; }
        [field: SerializeField] public int FlowerSellingPrice { get; private set; }
        [field: SerializeField] public Mesh FlowerSoilMesh { get; private set; }
        [SerializeField] private Mesh[] flowerLvlMeshes = new Mesh[4];
    
        [field: Header("FlowerCrossingRecipe")]
        [field: SerializeField] public FlowerName FirstCrossingFlowerName { get; private set; }
        [field: SerializeField] public FlowerName SecondCrossingFlowerName { get; private set; }

        public Mesh GetFlowerLvlMesh(int flowerGrowingLvl)
        {
            return flowerLvlMeshes[flowerGrowingLvl];
        }
    }
}
