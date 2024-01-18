using System;
using FlowerShop.Flowers;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public class FlowersCrossingTableProcessForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public int TableLvl { get; private set; }
        [field: SerializeField] public int ActionsBeforeBrokenQuantity { get; private set; }
        [field: SerializeField] public float CurrentCrossingFlowerTime { get; private set; }
        [field: SerializeField] public bool IsCrossingSeedReady { get; private set; }
        [field: SerializeField] public string PlantedFlowerInfoUniqueKey { get; private set; }
        [field: SerializeField] public bool IsSeedCrossing { get; private set; }
        
        public FlowersCrossingTableProcessForSaving() {}

        public FlowersCrossingTableProcessForSaving(int tableLvl, int actionsBeforeBrokenQuantity, 
            float currentCrossingFlowerTime, bool isCrossingSeedReady, string plantedFlowerInfoUniqueKey, bool isSeedCrossing)
        {
            IsValuesSaved = true;
            TableLvl = tableLvl;
            ActionsBeforeBrokenQuantity = actionsBeforeBrokenQuantity;
            CurrentCrossingFlowerTime = currentCrossingFlowerTime;
            IsCrossingSeedReady = isCrossingSeedReady;
            PlantedFlowerInfoUniqueKey = plantedFlowerInfoUniqueKey;
            IsSeedCrossing = isSeedCrossing;
        }
    }
}