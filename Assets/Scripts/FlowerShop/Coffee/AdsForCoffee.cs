using FlowerShop.Ads;
using FlowerShop.ComputerPages;
using FlowerShop.Tables;
using UnityEngine;
using Zenject;

namespace FlowerShop.Coffee
{
    [RequireComponent(typeof(UIButton))]
    public class AdsForCoffee : MonoBehaviour
    {
        [Inject] private readonly CoffeeTable coffeeTable;
        [Inject] private readonly LevelPlayAds levelPlayAds;
        
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
            levelPlayAds.ShowRewardedAd(() => StartCoroutine(coffeeTable.MakeCoffeeProcessForAds()));
        }
    }
}