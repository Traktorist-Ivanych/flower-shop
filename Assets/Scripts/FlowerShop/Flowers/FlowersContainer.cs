using System.Collections.Generic;
using FlowerShop.Flowers;
using UnityEngine;
using UnityEngine.Serialization;

public class FlowersContainer : MonoBehaviour
{
    [FormerlySerializedAs("emptyFlower")] [SerializeField] private FlowerInfo emptyFlowerInfo;
    [SerializeField] private List<FlowerInfo> playableFlowers;

    private readonly Dictionary<string, FlowerInfo> crossingRecipes = new();

    public FlowerInfo EmptyFlowerInfo
    {
        get => emptyFlowerInfo;
    }

    private void Start()
    {
        foreach (FlowerInfo flower in playableFlowers)
        {
            if (flower.FlowerLvl > 1)
            {
                string crossingRecipe = GetCrossingRecipe(flower.FirstCrossingFlower, flower.SecondCrossingFlower);

                crossingRecipes.Add(crossingRecipe, flower);
            }
        }
    }

    public FlowerInfo GetFlowerFromCrossingRecipe(Flower firstFlower, Flower secondFlower)
    {
        string crossingRecipe = GetCrossingRecipe(firstFlower, secondFlower);

        if (crossingRecipes.TryGetValue(crossingRecipe, out FlowerInfo recipe))
        {
            return recipe;
        }
        else
        {
            return emptyFlowerInfo;
        }
    }

    private string GetCrossingRecipe(Flower firstFlower, Flower secondFlower)
    {
        List<string> crossableFlowers = new()
        {
            firstFlower.FlowerName,
            secondFlower.FlowerName
        };
        crossableFlowers.Sort();

        return crossableFlowers[0] + crossableFlowers[1];
    }
}
