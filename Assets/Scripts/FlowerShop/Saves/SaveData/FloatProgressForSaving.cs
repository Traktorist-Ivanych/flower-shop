using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public struct FloatProgressForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public float Progress { get; private set; }

        public FloatProgressForSaving(float progress)
        {
            IsValuesSaved = true;
            Progress = progress;
        }
    }
}