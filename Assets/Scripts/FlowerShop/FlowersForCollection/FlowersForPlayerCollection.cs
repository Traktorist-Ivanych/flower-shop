using System.Collections.Generic;
using FlowerShop.Flowers;
using UnityEngine;

namespace FlowerShop.FlowersForCollection
{
    /// <summary>
    /// Contains list of flowers, that player has added to his collection
    /// </summary>
    public class FlowersForPlayerCollection : MonoBehaviour
    {
        private readonly List<FlowerInfo> flowerCollection = new();

        public void AddFlowerToCollectionList(FlowerInfo addedFlowerInfo)
        {
            flowerCollection.Add(addedFlowerInfo);
        }

        public bool IsFlowerForCollectionUnique(FlowerInfo verifiableFlowerInfo)
        {
            return !flowerCollection.Contains(verifiableFlowerInfo);
        }
    }
}
