using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public class FlowersStorageTableForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public string PotUniqueKey { get; private set; }
        
        public FlowersStorageTableForSaving() {}

        public FlowersStorageTableForSaving(string potUniqueKey)
        {
            IsValuesSaved = true;
            PotUniqueKey = potUniqueKey;
        }
    }
}