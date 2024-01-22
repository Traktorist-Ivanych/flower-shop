using UnityEngine;

namespace PlayerControl.SaveData
{
    [System.Serializable]
    public class PlayerCoffeeEffectForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public float CurrentCoffeeEffectDuration { get; private set; }
        
        public PlayerCoffeeEffectForSaving() {}

        public PlayerCoffeeEffectForSaving(float currentCoffeeEffectDuration)
        {
            IsValuesSaved = true;
            CurrentCoffeeEffectDuration = currentCoffeeEffectDuration;
        }
    }
}