using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public class FlowersGrowingTableForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public string PotUniqueKey { get; private set; }
        [field: SerializeField] public int TableLvl { get; private set; }
        [field: SerializeField] public int ActionsBeforeBrokenQuantity { get; private set; }
        
        public FlowersGrowingTableForSaving() {}

        public FlowersGrowingTableForSaving(string potUniqueKey, int tableLvl, int actionsBeforeBrokenQuantity)
        {
            IsValuesSaved = true;
            PotUniqueKey = potUniqueKey;
            TableLvl = tableLvl;
            ActionsBeforeBrokenQuantity = actionsBeforeBrokenQuantity;
        }
    }
}