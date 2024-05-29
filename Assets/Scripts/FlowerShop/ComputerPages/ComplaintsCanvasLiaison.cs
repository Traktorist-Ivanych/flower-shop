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
    public class ComplaintsCanvasLiaison : MonoBehaviour, INotifyPropertyChanged
    {
        [Inject] private readonly ComputerTable computerTable;
        [Inject] private readonly CustomersSettings customersSettings;
        
        public event PropertyChangedEventHandler PropertyChanged;

        [SerializeField] private ComputerFlowerInfoButton computerFlowerInfoButton;
        [SerializeField] private Image complaintIndicator;

        private bool isIndicatorActive;

        [field: SerializeField] public Canvas ComplaintsCanvas { get; private set; }
        
        [Binding]
        public string FlowerName { get; private set; }
        
        [Binding]
        public string ComplaintDescription { get; private set; }

        public void SetComplaintFlowerInfo(FlowerInfo flowerInfo, int descriptionIndex)
        {
            FlowerName = flowerInfo.LocalizedFlowerName.GetLocalizedString();
            OnPropertyChanged(nameof(FlowerName));

            ComplaintDescription = customersSettings.LocalizedComplaintDescriptions[descriptionIndex].GetLocalizedString();
            OnPropertyChanged(nameof(ComplaintDescription));
            
            computerFlowerInfoButton.SetFlowerInfo(flowerInfo);
        }

        public void ShowIndicator()
        {
            if (!isIndicatorActive)
            {
                complaintIndicator.enabled = true;
                isIndicatorActive = true;
                computerTable.ShowIndicator();
            }
        }

        public void HideIndicator()
        {
            if (isIndicatorActive)
            {
                complaintIndicator.enabled = false;
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