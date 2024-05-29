using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public struct IntForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public int SavingInt { get; private set; }

        public IntForSaving(int savingInt)
        {
            IsValuesSaved = true;
            SavingInt = savingInt;
        }
    }
}
