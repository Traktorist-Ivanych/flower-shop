using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;
using Zenject;

namespace FlowerShop.Fertilizers
{
    [Binding]
    public class FertilizersCanvasLiaison : MonoBehaviour, INotifyPropertyChanged
    {
        [Inject] private readonly FertilizersSetting fertilizersSetting;
        
        [SerializeField] private GrowthAccelerator growthAccelerator;
        [SerializeField] private GrowingLvlIncreaser growingLvlIncreaser;
        [SerializeField] private GrowerToMaxLvl growerToMaxLvl;
        [SerializeField] private GameObject fertilizerInfoPanel;
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        [field: SerializeField] public Canvas FertilizersCanvas { get; private set; }

        [Binding]
        public int GrowthAcceleratorAvailableUsesNumber => growthAccelerator.AvailableUsesNumber;
        
        [Binding]
        public int GrowingLvlIncreaserAvailableUsesNumber => growingLvlIncreaser.AvailableUsesNumber;
        
        [Binding]
        public int GrowerToMaxLvlAvailableUsesNumber => growerToMaxLvl.AvailableUsesNumber;
        
        [Binding]
        public string FertilizersPriceDescription { get; private set; }
        
        [Binding]
        public int FertilizersPrice => fertilizersSetting.FertilizersPrice;
        
        [Binding]
        public string FertilizerName { get; private set; }
        
        [Binding]
        public Sprite FertilizerSprite { get; private set; }
        
        [Binding]
        public string FertilizerDescription { get; private set; }

        private void Start()
        {
            OnPropertyChanged(nameof(FertilizersPrice));

            FertilizersPriceDescription =
                "Цена " + fertilizersSetting.IncreaseFertilizerAmount + " ед. каждого удобрения";
            OnPropertyChanged(nameof(FertilizersPriceDescription));
        }

        public void UpdateFertilizersAvailableUsesNumber()
        {
            OnPropertyChanged(nameof(GrowthAcceleratorAvailableUsesNumber));
            OnPropertyChanged(nameof(GrowingLvlIncreaserAvailableUsesNumber));
            OnPropertyChanged(nameof(GrowerToMaxLvlAvailableUsesNumber));
        }

        public void ShowFertilizerInfoPanel(FertilizerInfo fertilizerInfo)
        {
            fertilizerInfoPanel.SetActive(true);

            FertilizerName = fertilizerInfo.FertilizerName;
            OnPropertyChanged(nameof(FertilizerName));
            
            FertilizerSprite = fertilizerInfo.FertilizerSprite;
            OnPropertyChanged(nameof(FertilizerSprite));
            
            FertilizerDescription = fertilizerInfo.FertilizerDescription;
            OnPropertyChanged(nameof(FertilizerDescription));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}