using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public class RepairsAndUpgradesTableForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public bool IsRepairingAndUpgradingHammerInPlayerHands { get; private set; }
        
        public RepairsAndUpgradesTableForSaving() {}

        public RepairsAndUpgradesTableForSaving(bool isRepairingAndUpgradingHammerInPlayerHands)
        {
            IsValuesSaved = true;
            IsRepairingAndUpgradingHammerInPlayerHands = isRepairingAndUpgradingHammerInPlayerHands;
        }
    }
}