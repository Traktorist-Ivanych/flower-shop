using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public struct EducationHandlerForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public bool IsEducationActive { get; private set; }
        [field: SerializeField] public int Step { get; private set; }

        public EducationHandlerForSaving(bool isEducationActive, int step)
        {
            IsValuesSaved = true;
            IsEducationActive = isEducationActive;
            Step = step;
        }
    }
}