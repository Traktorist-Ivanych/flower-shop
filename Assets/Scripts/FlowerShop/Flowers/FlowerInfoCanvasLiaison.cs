using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;

namespace FlowerShop.Flowers
{
    [Binding]
    public class FlowerInfoCanvasLiaison : MonoBehaviour, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        [SerializeField] private Image growingTableImage;
        [SerializeField] private FlowerInfoRecipeButton flowerInfoButtonFirst;
        [SerializeField] private FlowerInfoRecipeButton flowerInfoButtonSecond;
        
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

        public void ShowFlowerInfo(FlowerInfo flowerInfo, Sprite flowerSprite)
        {
            UpgradeCanvas.enabled = true;

            growingTableImage.sprite = flowerInfo.GrowingRoom.RoomColorSprite;
            
            FlowerName = flowerInfo.FlowerNameRus;
            OnPropertyChanged(nameof(FlowerName));

            FlowerSprite = flowerSprite;
            OnPropertyChanged(nameof(FlowerSprite));

            FlowerLvl = flowerInfo.FlowerLvl.ToString();
            OnPropertyChanged(nameof(FlowerLvl));

            FlowerSellingPrice = flowerInfo.FlowerSellingPrice.ToString();
            OnPropertyChanged(nameof(FlowerSellingPrice));

            FlowerRecipeDescription = flowerInfo.CrossingRecipeDescription;
            OnPropertyChanged(nameof(FlowerRecipeDescription));
            
            flowerInfoButtonFirst.SetFlowerInfo(flowerInfo.FirstCrossingFlowerInfo);
            flowerInfoButtonSecond.SetFlowerInfo(flowerInfo.SecondCrossingFlowerInfo);
        }
        
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}