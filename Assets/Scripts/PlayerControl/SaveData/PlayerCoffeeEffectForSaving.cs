using UnityEngine;

namespace PlayerControl.SaveData
{
    [System.Serializable]
    public struct PlayerCoffeeEffectForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public float CurrentCoffeeEffectDuration { get; private set; }

        public PlayerCoffeeEffectForSaving(float currentCoffeeEffectDuration)
        {
            IsValuesSaved = true;
            CurrentCoffeeEffectDuration = currentCoffeeEffectDuration;
        }
    }
}