using PlayerControl.SaveData;
using Saves;
using UnityEngine;
using Zenject;

namespace PlayerControl
{
    public class PlayerMoney : MonoBehaviour, ISavableObject
    {
        [Inject] private readonly PlayerControlSettings playerControlSettings;
        [Inject] private readonly PlayerStatsCanvasLiaison playerStatsCanvasLiaison;
        [field: SerializeField] public int CurrentPlayerMoney { get; private set; }
        [field: SerializeField] public string UniqueKey { get; private set; }

        private void Awake()
        {
            Load();
        }

        public void AddPlayerMoney(int moneyAmount)
        {
            CurrentPlayerMoney += moneyAmount;
            UpdatePlayerMoneyOnCanvas();

            Save();
        }

        public void TakePlayerMoney(int moneyAmount)
        {
            CurrentPlayerMoney -= moneyAmount;
            UpdatePlayerMoneyOnCanvas();

            Save();
        }

        public void Load()
        {
            PlayerMoneyForSaving playerMoneyForLoading = SavesHandler.Load<PlayerMoneyForSaving>(UniqueKey);

            if (playerMoneyForLoading.IsValuesSaved)
            {
                CurrentPlayerMoney = playerMoneyForLoading.CurrentPlayerMoney;
            }
            else
            {
                CurrentPlayerMoney = playerControlSettings.FirstAvailableMoney;
            }
            
            UpdatePlayerMoneyOnCanvas();
        }

        public void Save()
        {
            PlayerMoneyForSaving playerMoneyForSaving = new(CurrentPlayerMoney);
            
            SavesHandler.Save(UniqueKey, playerMoneyForSaving);
        }
        
        private void UpdatePlayerMoneyOnCanvas()
        {
            playerStatsCanvasLiaison.UpdatePlayerMoneyOnCanvas(CurrentPlayerMoney.ToString());
        }
    }
}
