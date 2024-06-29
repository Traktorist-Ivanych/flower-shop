using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public struct MusicSongsSwitcherTableForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public int CurrentSongNumber { get; private set; }
        [field: SerializeField] public float[] ListenSongsTimes { get; private set; }
        [field: SerializeField] public bool[] IsSongsListened { get; private set; }

        public MusicSongsSwitcherTableForSaving(int currentSongNumber, float[] listenSongsTimes, 
            bool[] isSongsListened)
        {
            IsValuesSaved = true;
            CurrentSongNumber = currentSongNumber;
            ListenSongsTimes = listenSongsTimes;
            IsSongsListened = isSongsListened;
        }
    }
}