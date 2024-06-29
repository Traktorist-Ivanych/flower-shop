using FlowerShop.Flowers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public struct RareFlowersHandlerForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public int GuaranteedRareFlowerCrossingIndex { get; private set; }
        [field: SerializeField] public List<string> ReceivedRareFlowerInfoUniqueKeys { get; private set; }

        public RareFlowersHandlerForSaving(int guaranteedRareFlowerCrossingIndex, List<FlowerInfo> receivedRareFlowerInfo)
        {
            IsValuesSaved = true;
            GuaranteedRareFlowerCrossingIndex = guaranteedRareFlowerCrossingIndex;

            ReceivedRareFlowerInfoUniqueKeys = new();
            foreach (FlowerInfo rareFlowerInfo in receivedRareFlowerInfo)
            {
                ReceivedRareFlowerInfoUniqueKeys.Add(rareFlowerInfo.UniqueKey);
            }
        }
    }
}
