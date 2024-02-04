using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public struct WeedingTableForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public int TableLvl { get; private set; }
        [field: SerializeField] public bool IsWeedingHoeInPlayerHands { get; private set; }

        public WeedingTableForSaving(int tableLvl, bool isWeedingHoeInPlayerHands)
        {
            IsValuesSaved = true;
            TableLvl = tableLvl;
            IsWeedingHoeInPlayerHands = isWeedingHoeInPlayerHands;
        }
    }
}