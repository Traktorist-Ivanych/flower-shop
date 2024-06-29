using System.Collections.Generic;
using FlowerShop.Achievements;
using FlowerShop.Flowers;
using UnityEngine;
using Zenject;

namespace FlowerShop.FlowersForCollection
{
    /// <summary>
    /// Contains list of flowers, that player has added to his collection
    /// </summary>
    public class FlowersForPlayerCollection : MonoBehaviour
    {
        [Inject] private readonly AspiringCollector aspiringCollector;
        [Inject] private readonly FlowersCanvasLiaison flowersCanvasLiaison;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly LoverOfDecorativeFlowers loverOfDecorativeFlowers;
        [Inject] private readonly LoverOfExoticFlowers loverOfExoticFlowers;
        [Inject] private readonly WildflowerLover wildflowerLover;
        
        private readonly List<FlowerInfo> flowerCollection = new();

        public void AddFlowerToCollectionList(FlowerInfo addedFlowerInfo)
        {
            flowerCollection.Add(addedFlowerInfo);

            flowersCanvasLiaison.SetInCollectionInicator(addedFlowerInfo);

            if (addedFlowerInfo.GrowingRoom.Equals(flowersSettings.GrowingRoomDecorative))
            {
                loverOfDecorativeFlowers.IncreaseProgress();
            }
            else if (addedFlowerInfo.GrowingRoom.Equals(flowersSettings.GrowingRoomExotic))
            {
                loverOfExoticFlowers.IncreaseProgress();
            }
            else if (addedFlowerInfo.GrowingRoom.Equals(flowersSettings.GrowingRoomWild))
            {
                wildflowerLover.IncreaseProgress();
            }
            
            aspiringCollector.IncreaseProgress();
        }

        public bool IsFlowerForCollectionUnique(FlowerInfo verifiableFlowerInfo)
        {
            return !flowerCollection.Contains(verifiableFlowerInfo);
        }

        public FlowerInfo GetRandomFlowerInfoFromPlayerCollection()
        {
            return flowerCollection[Random.Range(0, flowerCollection.Count)];
        }

        public int FlowersInPlayerCollectionCount()
        {
            return flowerCollection.Count;
        }
    }
}
