using System;
using System.Collections.Generic;
using FlowerShop.Flowers;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public struct FlowersContainerForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public List<string> CrossedFlowerInfoUniqueKeys { get; private set; }

        public FlowersContainerForSaving(List<FlowerInfo> crossedFlowerInfos)
        {
            CrossedFlowerInfoUniqueKeys = new List<string>();
            
            IsValuesSaved = true;
            foreach (FlowerInfo crossedFlowerInfo in crossedFlowerInfos)
            {
                CrossedFlowerInfoUniqueKeys.Add(crossedFlowerInfo.UniqueKey);
            }
        }
    }
}