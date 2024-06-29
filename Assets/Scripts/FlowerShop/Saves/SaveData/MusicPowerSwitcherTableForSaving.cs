using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public struct MusicPowerSwitcherTableForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public bool IsMusicPowerOn { get; private set; }

        public MusicPowerSwitcherTableForSaving(bool isMusicPowerOn)
        {
            IsValuesSaved = true;
            IsMusicPowerOn = isMusicPowerOn;
        }
    }
}