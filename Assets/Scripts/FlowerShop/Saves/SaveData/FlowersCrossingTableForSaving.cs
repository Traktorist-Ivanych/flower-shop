using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public class FlowersCrossingTableForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public string PotUniqueKey { get; private set; }
        
        public FlowersCrossingTableForSaving() {}

        public FlowersCrossingTableForSaving(string potUniqueKey)
        {
            IsValuesSaved = true;
            PotUniqueKey = potUniqueKey;
        }
    }
}