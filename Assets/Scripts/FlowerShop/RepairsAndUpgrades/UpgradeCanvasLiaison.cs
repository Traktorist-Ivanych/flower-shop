using System.ComponentModel;
using Input;
using UnityEngine;
using UnityWeld.Binding;
using Zenject;

namespace FlowerShop.RepairsAndUpgrades
{
    [Binding]
    public class UpgradeCanvasLiaison : MonoBehaviour, INotifyPropertyChanged
    {
        [Inject] private readonly PlayerInputActions playerInputActions;
        
        public event PropertyChangedEventHandler PropertyChanged;

        [SerializeField] private Canvas upgradeCanvas;
    
        [Binding]
        public string TableName { get; private set; }
    
        [Binding]
        public string Description { get; private set; }
    
        [Binding]
        public Sprite TableSprite { get; private set; }
    
        [Binding]
        public string Price { get; private set; }
    
        public int PriceInt { get; private set; }
        
        public void DisableCanvas()
        {
            playerInputActions.DisableCanvasControlMode();
            upgradeCanvas.enabled = false;
        }
        
        public void SetUpgradableTableInfo(string tableName, string description, int priceInt, Sprite tableSprite)
        {
            EnableCanvas();

            TableName = tableName;
            OnPropertyChanged(nameof(TableName));

            TableSprite = tableSprite;
            OnPropertyChanged(nameof(TableSprite));

            Description = description;
            OnPropertyChanged(nameof(Description));

            PriceInt = priceInt;
            Price = priceInt.ToString();
            OnPropertyChanged(nameof(Price));
        }
        
        private void EnableCanvas()
        {
            playerInputActions.EnableCanvasControlMode();
            upgradeCanvas.enabled = true;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
