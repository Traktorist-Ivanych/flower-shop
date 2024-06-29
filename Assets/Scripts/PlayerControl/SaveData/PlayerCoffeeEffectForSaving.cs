using UnityEngine;

namespace PlayerControl.SaveData
{
    [System.Serializable]
    public struct PlayerCoffeeEffectForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public bool IsCoffeeEffectActive { get; private set; }
        [field: SerializeField] public float CurrentCoffeeEffectDuration { get; private set; }
        [field: SerializeField] public bool IsCoffeeEffectPurchased { get; private set; }

        public PlayerCoffeeEffectForSaving(bool isCoffeeEffectActive, float currentCoffeeEffectDuration, bool isCoffeeEffectPurchased)
        {
            IsValuesSaved = true;
            IsCoffeeEffectActive = isCoffeeEffectActive;
            CurrentCoffeeEffectDuration = currentCoffeeEffectDuration;
            IsCoffeeEffectPurchased = isCoffeeEffectPurchased;
        }
    }
}