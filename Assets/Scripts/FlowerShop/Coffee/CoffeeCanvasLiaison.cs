using System.ComponentModel;
using Input;
using UnityEngine;
using UnityWeld.Binding;
using Zenject;

namespace FlowerShop.Coffee
{
    [Binding]
    public class CoffeeCanvasLiaison : MonoBehaviour, INotifyPropertyChanged
    {
        [Inject] private readonly CoffeeSettings coffeeSettings;
        [Inject] private readonly PlayerInputActions playerInputActions;

        [SerializeField] private Canvas coffeeCanvas;
    
        public event PropertyChangedEventHandler PropertyChanged;
    
        [Binding] 
        public string CoffeePrice { get; private set; }

        private void Start()
        {
            CoffeePrice = coffeeSettings.CoffeePrice.ToString();
            OnPropertyChanged(nameof(CoffeePrice));
        }
        
        public void EnableCanvas()
        {
            playerInputActions.EnableCanvasControlMode();
            coffeeCanvas.enabled = true;
        }
        
        public void DisableCanvas()
        {
            playerInputActions.DisableCanvasControlMode();
            coffeeCanvas.enabled = false;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
