using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;

namespace PlayerControl
{
    [Binding]
    public class PlayerStatsCanvasLiaison : MonoBehaviour, INotifyPropertyChanged
    {
        [SerializeField] private Image coffeeEffectIndicator;
        [SerializeField] private Image coffeeEffectIndicatorBackGround;

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
            coffeeEffectIndicator.enabled = flag;
            coffeeEffectIndicatorBackGround.enabled = flag;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
