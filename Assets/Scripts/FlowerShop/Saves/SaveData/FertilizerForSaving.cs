using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public class FertilizerForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public int AvailableUsesNumber { get; private set; }
        
        public FertilizerForSaving() {}

        public FertilizerForSaving(int availableUsesNumber)
        {
            IsValuesSaved = true;
            AvailableUsesNumber = availableUsesNumber;
        }
    }
}