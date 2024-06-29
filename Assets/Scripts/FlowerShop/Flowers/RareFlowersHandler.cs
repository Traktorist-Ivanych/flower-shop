using FlowerShop.Achievements;
using FlowerShop.Saves.SaveData;
using Saves;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FlowerShop.Flowers
{
    public class RareFlowersHandler : MonoBehaviour, ISavableObject
    {
        [Inject] private readonly FlowersCanvasLiaison flowersCanvasLiaison;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly GreatCollector greatCollector; 
        [Inject] private readonly ReferencesForLoad referencesForLoad;

        [SerializeField] private List<FlowerInfo> rareFlowerInfos;

        private readonly List<FlowerInfo> collectibleRareFlowerInfo = new();
        private readonly List<FlowerInfo> availableRareFlowerInfo = new();
        private readonly List<FlowerInfo> receivedRareFlowerInfo = new();
        private int guaranteedRareFlowerCrossingIndex;

        [field: SerializeField] public string UniqueKey { get; private set;}

        private void Awake()
        {
            Load();
        }

        private void Start()
        {
            foreach (FlowerInfo rareFlowerInfo in rareFlowerInfos)
            {
                if (!collectibleRareFlowerInfo.Contains(rareFlowerInfo))
                {
                    availableRareFlowerInfo.Add(rareFlowerInfo);
                }
            }
        }

        public bool IsRareFlowerResultOfCrossing()
        {
            if (flowersSettings.ShouldRareFlowerAppearance())
            {
                ResetGuaranteedRareFlowerCrossingIndex();
                Save();
                return true;
            }
            else
            {
                guaranteedRareFlowerCrossingIndex++;
                if (guaranteedRareFlowerCrossingIndex >= flowersSettings.GuaranteedRareFlowerCrossingMaxIndex)
                {
                    ResetGuaranteedRareFlowerCrossingIndex();
                    Save();
                    return true;
                }
                else
                {
                    Save();
                    return false; 
                }
            }
        }

        public FlowerInfo GetRareFlower()
        {
            if (collectibleRareFlowerInfo.Count == rareFlowerInfos.Count)
            {
                int flowerIndex = Random.Range(0, rareFlowerInfos.Count);
                return rareFlowerInfos[flowerIndex];
            }
            else
            {
                int flowerIndex = Random.Range(0, availableRareFlowerInfo.Count);
                TryAddReceivedRareFlowerInfo(availableRareFlowerInfo[flowerIndex]);
                return availableRareFlowerInfo[flowerIndex];
            }
        }

        public bool IsRareFlowerReceived(FlowerInfo rareFlowerInfo)
        {
            return receivedRareFlowerInfo.Contains(rareFlowerInfo);
        }

        public bool IsRareFlowerForCollectionUnique(FlowerInfo addedFlowerInfo)
        {
            return !collectibleRareFlowerInfo.Contains(addedFlowerInfo);
        }

        public void AddRareFlowerInCollection(FlowerInfo addedFlowerInfo)
        {
            collectibleRareFlowerInfo.Add(addedFlowerInfo);

            greatCollector.IncreaseProgress();

            flowersCanvasLiaison.SetRareInCollectionInicator(addedFlowerInfo);

            if (availableRareFlowerInfo.Contains(addedFlowerInfo))
            {
                availableRareFlowerInfo.Remove(addedFlowerInfo);
            }
        }

        public void Save()
        {
            RareFlowersHandlerForSaving rareFlowersHandlerForSaving = 
                new RareFlowersHandlerForSaving(guaranteedRareFlowerCrossingIndex, receivedRareFlowerInfo);

            SavesHandler.Save(UniqueKey, rareFlowersHandlerForSaving);
        }

        public void Load()
        {
            RareFlowersHandlerForSaving rareFlowersHandlerForLoading = SavesHandler.Load<RareFlowersHandlerForSaving>(UniqueKey);

            if (rareFlowersHandlerForLoading.IsValuesSaved)
            {
                guaranteedRareFlowerCrossingIndex = rareFlowersHandlerForLoading.GuaranteedRareFlowerCrossingIndex;

                foreach (string uniqueKeys in rareFlowersHandlerForLoading.ReceivedRareFlowerInfoUniqueKeys)
                {
                    receivedRareFlowerInfo.Add(referencesForLoad.GetReference<FlowerInfo>(uniqueKeys));
                }
            }
        }

        private void TryAddReceivedRareFlowerInfo(FlowerInfo addedFlowerInfo)
        {
            if (!receivedRareFlowerInfo.Contains(addedFlowerInfo))
            {
                receivedRareFlowerInfo.Add(addedFlowerInfo);
                flowersCanvasLiaison.ShowRareFlowerInfo(addedFlowerInfo);

                Save();
            }
        }

        private void ResetGuaranteedRareFlowerCrossingIndex()
        {
            guaranteedRareFlowerCrossingIndex = 0;
        }
    }
}
