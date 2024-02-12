using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public struct MusicSongsSwitcherTableForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public int CurrentSongNumber { get; private set; }

        public MusicSongsSwitcherTableForSaving(int currentSongNumber)
        {
            IsValuesSaved = true;
            CurrentSongNumber = currentSongNumber;
        }
    }
}