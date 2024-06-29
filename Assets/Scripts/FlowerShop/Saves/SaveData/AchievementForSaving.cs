using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public struct AchievementForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public int AchievementProgress { get; private set; }
        [field: SerializeField] public bool IsAchievementDone { get; private set; }
        [field: SerializeField] public bool IsAwardReceived { get; private set; }

        public AchievementForSaving(int achievementProgress, bool isAchievementDone, bool isAwardReceived)
        {
            IsValuesSaved = true;
            AchievementProgress = achievementProgress;
            IsAchievementDone = isAchievementDone;
            IsAwardReceived = isAwardReceived;
        }
    }
}