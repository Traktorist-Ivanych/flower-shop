using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FlowerShop.Flowers
{
    public class FlowersContainer : MonoBehaviour
    {
        [Inject] private readonly FlowersSettings flowersSettings;
        
        [SerializeField] private List<FlowerInfo> allFlowersInfo;

        private Dictionary<string, FlowerInfo> crossingRecipes;
        
        [field: SerializeField] public FlowerInfo EmptyFlowerInfo { get; private set; }

        public void Awake()
        {
            crossingRecipes = new Dictionary<string, FlowerInfo>();
            
            foreach (FlowerInfo flowerInfo in allFlowersInfo)
            {
                if (flowerInfo.FlowerLvl >= flowersSettings.CrossingFlowerMinLvl)
                {
                    string crossingRecipe = GetCrossingRecipe(
                        firstFlowerName: flowerInfo.FirstCrossingFlowerName, 
                        secondFlowerName: flowerInfo.SecondCrossingFlowerName);

                    crossingRecipes.Add(crossingRecipe, flowerInfo);
                }
            }
        }

        public FlowerInfo GetFlowerFromCrossingRecipe(FlowerName firstFlowerName, FlowerName secondFlowerName)
        {
            string crossingRecipe = GetCrossingRecipe(firstFlowerName, secondFlowerName);

            crossingRecipes.TryGetValue(crossingRecipe, out FlowerInfo flowerInfoFromRecipe);

            return flowerInfoFromRecipe == null 
                ? EmptyFlowerInfo
                : flowerInfoFromRecipe;
        }

        private string GetCrossingRecipe(FlowerName firstFlowerName, FlowerName secondFlowerName)
        {
            List<string> flowersForCrossingRecipe = new()
            {
                firstFlowerName.FlowerNameString,
                secondFlowerName.FlowerNameString
            };
            flowersForCrossingRecipe.Sort();

            return flowersForCrossingRecipe[0] + flowersForCrossingRecipe[1];
        }
    }
}
