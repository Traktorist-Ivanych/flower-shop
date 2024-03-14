using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

namespace PlayerControl
{
    [Binding]
    public class PlayerStatsCanvasLiaison : MonoBehaviour, INotifyPropertyChanged
    {
        [SerializeField] private GameObject coffeeEffectPanel;
        
        public event PropertyChangedEventHandler PropertyChanged;

        [Binding]
        public string PlayerMoney { get; private set; }
        
        [Binding]
        public string ShopRating { get; private set; }
        
        [Binding]
        public float CoffeeEffectIndicatorFillAmount { get; private set; }

        public void UpdatePlayerMoneyOnCanvas(string transmittedPlayerMoney)
        {
            PlayerMoney = transmittedPlayerMoney;
            OnPropertyChanged(nameof(PlayerMoney));
        }

        public void UpdateShopRating(string shopRating)
        {
            ShopRating = shopRating;
            OnPropertyChanged(nameof(ShopRating));
        }

        public void UpdateCoffeeEffectIndicatorFillAmount(float fillAmountValue)
        {
            CoffeeEffectIndicatorFillAmount = fillAmountValue;
            OnPropertyChanged(nameof(CoffeeEffectIndicatorFillAmount));
        }

        public void SetCoffeeEffectPanelActive(bool flag)
        {
            coffeeEffectPanel.SetActive(flag);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
