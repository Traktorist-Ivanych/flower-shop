using FlowerShop.Ads;
using FlowerShop.ComputerPages;
using FlowerShop.Tables;
using UnityEngine;
using Zenject;

namespace FlowerShop.Fertilizers
{
    [RequireComponent(typeof(UIButton))]
    public class AdsForFertilizers : MonoBehaviour
    {
        [Inject] private readonly FertilizersTable fertilizersTable;
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
            levelPlayAds.ShowRewardedAd(fertilizersTable.IncreaseAvailableFertilizersUsesNumber);
        }
    }
}