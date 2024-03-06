using System.Collections.Generic;
using FlowerShop.Saves.SaveData;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Flowers
{
    public class FlowersContainer : MonoBehaviour, ISavableObject
    {
        [Inject] private readonly FlowersCanvasLiaison flowersCanvasLiaison;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly ReferencesForLoad referencesForLoad;
        
        [SerializeField] private List<FlowerInfo> allFlowersInfo;

        private Dictionary<string, FlowerInfo> crossingRecipes;
        
        private readonly List<FlowerInfo> crossedFlowerInfos = new();
        
        [field: SerializeField] public FlowerInfo EmptyFlowerInfo { get; private set; }
        [field: SerializeField] public string UniqueKey { get; private set; }

        public void Awake()
        {
            crossingRecipes = new Dictionary<string, FlowerInfo>();
            
            foreach (FlowerInfo flowerInfo in allFlowersInfo)
            {
                if (flowerInfo.FlowerLvl >= flowersSettings.CrossingFlowerMinLvl)
                {
                    string crossingRecipe = GetCrossingRecipe(
                        firstFlowerName: flowerInfo.FirstCrossingFlowerInfo.FlowerName, 
                        secondFlowerName: flowerInfo.SecondCrossingFlowerInfo.FlowerName);

                    crossingRecipes.Add(crossingRecipe, flowerInfo);
                }
            }
            
            Load();
        }

        public void Save()
        {
            FlowersContainerForSaving flowersContainerForSaving =
                new FlowersContainerForSaving(crossedFlowerInfos);
            
            SavesHandler.Save(UniqueKey, flowersContainerForSaving);
        }

        public void Load()
        {
            FlowersContainerForSaving flowersContainerForSaving =
                SavesHandler.Load<FlowersContainerForSaving>(UniqueKey);

            if (flowersContainerForSaving.IsValuesSaved)
            {
                foreach (string uniqueKeys in flowersContainerForSaving.CrossedFlowerInfoUniqueKeys)
                {
                    crossedFlowerInfos.Add(referencesForLoad.GetReference<FlowerInfo>(uniqueKeys));
                }
            }
        }

        public void TryToAddCrossedFlowerInfo(FlowerInfo crossedFlowerInfo)
        {
            if (!crossedFlowerInfos.Contains(crossedFlowerInfo))
            {
                crossedFlowerInfos.Add(crossedFlowerInfo);
                flowersCanvasLiaison.ShowCrossedFlowerInfo(crossedFlowerInfo);

                Save();
            }
        }

        public bool IsFlowerInfoCrossed(FlowerInfo flowerInfo)
        {
            return crossedFlowerInfos.Contains(flowerInfo);
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
