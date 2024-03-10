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
        [SerializeField] private List<FlowerInfo> firstLvlFlowerInfos;

        private Dictionary<string, FlowerInfo> crossingRecipes;
        
        private readonly List<FlowerInfo> availableFlowerInfos = new();
        
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
                new FlowersContainerForSaving(availableFlowerInfos);
            
            SavesHandler.Save(UniqueKey, flowersContainerForSaving);
        }

        public void Load()
        {
            FlowersContainerForSaving flowersContainerForSaving =
                SavesHandler.Load<FlowersContainerForSaving>(UniqueKey);

            foreach (FlowerInfo firstLvlFlowerInfo in firstLvlFlowerInfos)
            {
                availableFlowerInfos.Add(firstLvlFlowerInfo);
            }
            
            if (flowersContainerForSaving.IsValuesSaved)
            {
                foreach (string uniqueKeys in flowersContainerForSaving.CrossedFlowerInfoUniqueKeys)
                {
                    availableFlowerInfos.Add(referencesForLoad.GetReference<FlowerInfo>(uniqueKeys));
                }
            }
        }

        public void TryToAddAvailableFlowerInfo(FlowerInfo crossedFlowerInfo)
        {
            if (!availableFlowerInfos.Contains(crossedFlowerInfo))
            {
                availableFlowerInfos.Add(crossedFlowerInfo);
                flowersCanvasLiaison.ShowCrossedFlowerInfo(crossedFlowerInfo);

                Save();
            }
        }

        public FlowerInfo GetRandomAvailableFlowerInfo()
        {
            return availableFlowerInfos[Random.Range(0, availableFlowerInfos.Count)];
        }

        public bool IsFlowerInfoAvailable(FlowerInfo flowerInfo)
        {
            return availableFlowerInfos.Contains(flowerInfo);
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
