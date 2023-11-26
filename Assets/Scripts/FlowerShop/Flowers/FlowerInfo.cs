using FlowerShop.Flowers;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFlowerInfo", menuName = "Flower Info", order = 57)]
public class FlowerInfo : ScriptableObject
{
    [field: Header("FlowerInfo")]
    [field: SerializeField] public string FlowerNameRus { get; private set; }
    [field: SerializeField] public Sprite FlowerSprite { get; private set; }
    [field: SerializeField] public Flower Flower { get; private set; }
    [field: SerializeField] public GrowingRoom GrowingRoom { get; private set; }
    [field: SerializeField] public int FlowerLvl { get; private set; }
    [field: SerializeField] public int FlowerSellingPrice { get; private set; }
    [SerializeField] private Mesh[] flowerLvlMeshes = new Mesh[4];
    
    [field: Header("FlowerCrossingRecipe")]
    [field: SerializeField] public Flower FirstCrossingFlower { get; private set; }
    [field: SerializeField] public Flower SecondCrossingFlower { get; private set; }

    public Mesh GetFlowerLvlMesh(int flowerGrowingLvl)
    {
        return flowerLvlMeshes[flowerGrowingLvl];
    }
}
