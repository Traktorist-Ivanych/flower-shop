using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public struct BoolForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public bool SavingBool { get; private set; }

        public BoolForSaving(bool savingBool)
        {
            IsValuesSaved = true;
            SavingBool = savingBool;
        }
    }
}