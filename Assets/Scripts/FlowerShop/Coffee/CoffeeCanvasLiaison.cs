using System.ComponentModel;
using Input;
using UnityEngine;
using UnityWeld.Binding;
using Zenject;

namespace FlowerShop.Coffee
{
    [Binding]
    public class CoffeeCanvasLiaison : MonoBehaviour
    {
        [Inject] private readonly CoffeeSettings coffeeSettings;
        [Inject] private readonly PlayerInputActions playerInputActions;

        [SerializeField] private Canvas coffeeCanvas;
        
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
    }
}
