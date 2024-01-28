using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public struct WateringCanForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public int CurrentWateringsNumber { get; private set; }
        [field: SerializeField] public int WateringCanLvl { get; private set; }

        public WateringCanForSaving(int currentWateringsNumber, int wateringCanLvl)
        {
            IsValuesSaved = true;
            CurrentWateringsNumber = currentWateringsNumber;
            WateringCanLvl = wateringCanLvl;
        }
    }
}