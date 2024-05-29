using System.ComponentModel;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using UnityWeld.Binding;
using Zenject;

namespace FlowerShop.Flowers
{
    [Binding]
    public class FlowerInfoCanvasLiaison : MonoBehaviour, INotifyPropertyChanged
    {
        [Inject] private readonly FlowersSettings flowersSettings;

        public event PropertyChangedEventHandler PropertyChanged;
        
        [SerializeField] private Image growingTableImage;
        [SerializeField] private FlowerInfoRecipeButton flowerInfoButtonFirst;
        [SerializeField] private FlowerInfoRecipeButton flowerInfoButtonSecond;
        [SerializeField] private LocalizedString LocalizedPriceText;
        [SerializeField] private LocalizedString LocalizedLvlText;

        [Binding]
        public string FlowerName { get; private set; }
    
        [Binding]
        public Sprite FlowerSprite { get; private set; }
        
        [Binding]
        public string FlowerLvl { get; private set; }
    
        [Binding]
        public string FlowerSellingPrice { get; private set; }
        
        [Binding]
        public string FlowerRecipeDescription { get; private set; }
        
        [field: SerializeField] public Canvas UpgradeCanvas { get; private set; }

        public void ShowFlowerInfo(bool isFlowerAvailable, FlowerInfo flowerInfo)
        {
            UpgradeCanvas.enabled = true;

            FlowerLvl = LocalizedLvlText.GetLocalizedString() + " " + flowerInfo.FlowerLvl.ToString();
            OnPropertyChanged(nameof(FlowerLvl));

            FlowerRecipeDescription = flowerInfo.LocalizedCrossingRecipeDescription.GetLocalizedString();
            OnPropertyChanged(nameof(FlowerRecipeDescription));

            flowerInfoButtonFirst.SetFlowerInfo(flowerInfo.FirstCrossingFlowerInfo);
            flowerInfoButtonSecond.SetFlowerInfo(flowerInfo.SecondCrossingFlowerInfo);

            if (isFlowerAvailable || flowerInfo.FlowerLvl == flowersSettings.MinFlowerLvl)
            {
                growingTableImage.sprite = flowerInfo.GrowingRoom.RoomColorSprite;

                FlowerSprite = flowerInfo.FlowerSprite512;
                FlowerName = flowerInfo.LocalizedFlowerName.GetLocalizedString();
                FlowerSellingPrice = LocalizedPriceText.GetLocalizedString() + " " + flowerInfo.FlowerSellingPrice.ToString();
            }
            else
            {
                growingTableImage.sprite = flowersSettings.UnknownFlower;

                FlowerSprite = flowersSettings.UnknownFlower;
                FlowerName = flowersSettings.FlowerInfoEmpty.LocalizedFlowerName.GetLocalizedString();
                FlowerSellingPrice = LocalizedPriceText.GetLocalizedString() + " " + flowersSettings.QuestionMark;
            }

            OnPropertyChanged(nameof(FlowerSprite));
            OnPropertyChanged(nameof(FlowerName));
            OnPropertyChanged(nameof(FlowerSellingPrice));

        }

        public void ShowFlowerInfo(FlowerInfo flowerInfo, string vipOrderMultipler)
        {
            UpgradeCanvas.enabled = true;

            FlowerLvl = LocalizedLvlText.GetLocalizedString() + " " + flowerInfo.FlowerLvl.ToString();
            OnPropertyChanged(nameof(FlowerLvl));

            FlowerRecipeDescription = flowerInfo.LocalizedCrossingRecipeDescription.GetLocalizedString();
            OnPropertyChanged(nameof(FlowerRecipeDescription));

            flowerInfoButtonFirst.SetFlowerInfo(flowerInfo.FirstCrossingFlowerInfo);
            flowerInfoButtonSecond.SetFlowerInfo(flowerInfo.SecondCrossingFlowerInfo);

            growingTableImage.sprite = flowerInfo.GrowingRoom.RoomColorSprite;

            FlowerSprite = flowerInfo.FlowerSprite512;
            OnPropertyChanged(nameof(FlowerSprite));

            FlowerName = flowerInfo.LocalizedFlowerName.GetLocalizedString();
            OnPropertyChanged(nameof(FlowerName));

            FlowerSellingPrice = LocalizedPriceText.GetLocalizedString() + " " +
                flowerInfo.FlowerSellingPrice.ToString() + vipOrderMultipler;
            OnPropertyChanged(nameof(FlowerSellingPrice));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}