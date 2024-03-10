using System.ComponentModel;
using FlowerShop.Customers;
using FlowerShop.Flowers;
using UnityEngine;
using UnityWeld.Binding;
using Zenject;

namespace FlowerShop.ComputerPages
{
    [Binding]
    public class VipCanvasLiaison : MonoBehaviour, INotifyPropertyChanged
    {
        [Inject] private readonly CustomersSettings customersSettings;
        
        public event PropertyChangedEventHandler PropertyChanged;

        [SerializeField] private ComputerFlowerInfoButton computerFlowerInfoButton;

        [field: SerializeField] public Canvas VipCanvas { get; private set; }
        
        [Binding]
        public string FlowerName { get; private set; }
        
        [Binding]
        public string VipDescription { get; private set; }

        public void SetComplaintFlowerInfo(FlowerInfo flowerInfo, int descriptionIndex)
        {
            FlowerName = flowerInfo.FlowerNameRus;
            OnPropertyChanged(nameof(FlowerName));

            VipDescription = customersSettings.VipDescriptions[descriptionIndex];
            OnPropertyChanged(nameof(VipDescription));
            
            computerFlowerInfoButton.SetFlowerInfo(flowerInfo);
        }
        
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}