using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public class SoilPreparationTableForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public int TableLvl { get; private set; }
        [field: SerializeField] public int ActionsBeforeBrokenQuantity { get; private set; }
        
        public SoilPreparationTableForSaving() {}

        public SoilPreparationTableForSaving(int tableLvl, int actionsBeforeBrokenQuantity)
        {
            IsValuesSaved = true;
            TableLvl = tableLvl;
            ActionsBeforeBrokenQuantity = actionsBeforeBrokenQuantity;
        }
    }
}