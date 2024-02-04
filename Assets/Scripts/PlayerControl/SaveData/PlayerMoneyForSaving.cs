using UnityEngine;

namespace PlayerControl.SaveData
{
    [System.Serializable]
    public struct PlayerMoneyForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public int CurrentPlayerMoney { get; private set; }

        public PlayerMoneyForSaving(int currentPlayerMoney)
        {
            IsValuesSaved = true;
            CurrentPlayerMoney = currentPlayerMoney;
        }
    }
}