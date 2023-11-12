using System.Collections.Generic;
using UnityEngine;

public class FlowersContainer : MonoBehaviour
{
    [SerializeField] private Flower emptyFlower;
    [SerializeField] private List<Flower> playableFlowers;

    private readonly Dictionary<string, Flower> crossingRecipes = new();

    public Flower EmptyFlower
    {
        get => emptyFlower;
    }

    private void Start()
    {
        foreach (Flower flower in playableFlowers)
        {
            if (flower.FlowerLvl > 1)
            {
                string crossingRecipe = GetCrossingRecipe(flower.FirstCrossingFlowerEnum, flower.SecondCrossingFlowerEnum);

                crossingRecipes.Add(crossingRecipe, flower);
            }
        }
    }

    public Flower GetFlowerFromCrossingRecipe(IFlower.Flower firstFlowerEnum, IFlower.Flower secondFlowerEnum)
    {
        string crossingRecipe = GetCrossingRecipe(firstFlowerEnum, secondFlowerEnum);

        if (crossingRecipes.ContainsKey(crossingRecipe))
        {
            return crossingRecipes[crossingRecipe];
        }
        else
        {
            return emptyFlower;
        }
    }

    private string GetCrossingRecipe(IFlower.Flower firstFlowerEnum, IFlower.Flower secondFlowerEnum)
    {
        List<string> crossableFlowers = new()
        {
            firstFlowerEnum.ToString(),
            secondFlowerEnum.ToString()
        };
        crossableFlowers.Sort();

        return crossableFlowers[0] + crossableFlowers[1];
    }
}
