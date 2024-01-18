using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public class WateringTableForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public int TableLvl { get; private set; }
        [field: SerializeField] public int ActionsBeforeBrokenQuantity { get; private set; }
        [field: SerializeField] public bool IsWateringCanInPlayerHands { get; private set; }
        
        public WateringTableForSaving() {}

        public WateringTableForSaving(int tableLvl, int actionsBeforeBrokenQuantity, 
            int currentWateringsNumber, bool isWateringCanInPlayerHands)
        {
            IsValuesSaved = true;
            TableLvl = tableLvl;
            ActionsBeforeBrokenQuantity = actionsBeforeBrokenQuantity;
            IsWateringCanInPlayerHands = isWateringCanInPlayerHands;
        }
    }
}