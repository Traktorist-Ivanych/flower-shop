using UnityEngine;

[CreateAssetMenu(fileName = "New Flower", menuName = "Flower", order = 52)]
public class Flower : ScriptableObject
{
    [Header("FlowerInfo")]
    [SerializeField] private string flowerName;
    [SerializeField] private Sprite flowerSprite;
    [SerializeField] private IFlower.Flower flowerEnum;
    [SerializeField] private IGrowingRoom.GroweringRoom flowerGrowingRoom;
    [SerializeField] private int flowerLvl;
    [SerializeField] private int flowerSellingPrice;
    [SerializeField] private Mesh[] flowerLvlMeshes = new Mesh[4];
    [Header("FlowerCrossingRecipe")]
    [SerializeField] private IFlower.Flower firstCrossingFlowerEnum;
    [SerializeField] private IFlower.Flower secondCrossingFlowerEnum;

    public string FlowerName
    {
        get => flowerName;
    }

    public Sprite FlowerSprite
    {
        get => flowerSprite;
    }

    public IFlower.Flower FlowerEnum
    {
        get => flowerEnum;
    }

    public IGrowingRoom.GroweringRoom FlowerGrowingRoom
    {
        get => flowerGrowingRoom;
    }

    public int FlowerLvl
    {
        get => flowerLvl;
    }

    public int FlowerSellingPrice
    {
        get => flowerSellingPrice;
    }

    public IFlower.Flower FirstCrossingFlowerEnum
    {
        get => firstCrossingFlowerEnum;
    }

    public IFlower.Flower SecondCrossingFlowerEnum
    {
        get => secondCrossingFlowerEnum;
    }

    public Mesh GetFlowerLvlMesh(int flowerGrowingLvl)
    {
        return flowerLvlMeshes[flowerGrowingLvl];
    }
}
