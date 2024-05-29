using System.ComponentModel;
using FlowerShop.Customers;
using FlowerShop.Flowers;
using FlowerShop.Tables;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;
using Zenject;

namespace FlowerShop.ComputerPages
{
    [Binding]
    public class VipCanvasLiaison : MonoBehaviour, INotifyPropertyChanged
    {
        [Inject] private readonly ComputerTable computerTable;
        [Inject] private readonly CustomersSettings customersSettings;
        
        public event PropertyChangedEventHandler PropertyChanged;

        [SerializeField] private ComputerFlowerInfoButton computerFlowerInfoButton;
        [SerializeField] private Image vipIndicator;

        private bool isIndicatorActive;

        [field: SerializeField] public Canvas VipCanvas { get; private set; }
        
        [Binding]
        public string FlowerName { get; private set; }
        
        [Binding]
        public string VipDescription { get; private set; }

        public void SetVipFlowerInfo(FlowerInfo flowerInfo, int descriptionIndex)
        {
            FlowerName = flowerInfo.LocalizedFlowerName.GetLocalizedString();
            OnPropertyChanged(nameof(FlowerName));

            VipDescription = customersSettings.LocalizedVipDescriptions[descriptionIndex].GetLocalizedString();
            OnPropertyChanged(nameof(VipDescription));
            
            computerFlowerInfoButton.SetFlowerInfo(flowerInfo);
        }

        public void ShowIndicator()
        {
            if (!isIndicatorActive)
            {
                vipIndicator.enabled = true;
                isIndicatorActive = true;
                computerTable.ShowIndicator();
            }
        }

        public void HideIndicator()
        {
            if (isIndicatorActive)
            {
                vipIndicator.enabled = false;
                isIndicatorActive = false;
                computerTable.HideIndicator();
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}