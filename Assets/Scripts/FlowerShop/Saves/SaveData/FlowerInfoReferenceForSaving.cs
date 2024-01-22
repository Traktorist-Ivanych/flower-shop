using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public class FlowerInfoReferenceForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public string FlowerInfoOnTableUniqueKey { get; private set; }
        
        public FlowerInfoReferenceForSaving() {}

        public FlowerInfoReferenceForSaving(string flowerInfoOnTableUniqueKey)
        {
            IsValuesSaved = true;
            FlowerInfoOnTableUniqueKey = flowerInfoOnTableUniqueKey;
        }
    }
}