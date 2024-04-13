using FlowerShop.Ads;
using FlowerShop.ComputerPages;
using FlowerShop.Tables;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.Money
{
    [RequireComponent(typeof(UIButton))]
    public class AdsForMoney : MonoBehaviour
    {
        [Inject] private readonly LevelPlayAds levelPlayAds;
        [Inject] private readonly PlayerControlSettings playerControlSettings;
        [Inject] private readonly PlayerMoney playerMoney;
        
        [HideInInspector, SerializeField] private UIButton uiButton;
        
        private void OnValidate()
        {
            uiButton = GetComponent<UIButton>();
        }

        private void OnEnable()
        {
            uiButton.OnClickEvent += OnButtonClick;
        }
        
        private void OnDisable()
        {
            uiButton.OnClickEvent -= OnButtonClick;
        }

        private void OnButtonClick()
        {
            levelPlayAds.ShowRewardedAd(AdsReward);
        }

        private void AdsReward()
        {
            playerMoney.AddPlayerMoney(playerControlSettings.AdsMoneyReward);
        }
    }
}
