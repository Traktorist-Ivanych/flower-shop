using FlowerShop.ComputerPages;
using FlowerShop.Effects;
using PlayerControl;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.Coffee
{
    [RequireComponent(typeof(UIButton))]
    public class BuyCoffeeCancelButton : MonoBehaviour
    {
        [Inject] private readonly CoffeeCanvasLiaison coffeeCanvasLiaison;
        [Inject] private readonly PlayerBusyness playerBusyness;
        [Inject] private readonly SelectedTableEffect selectedTableEffect;
        
        [HideInInspector, SerializeField] private UIButton canselButton;

        private void OnValidate()
        {
            canselButton = GetComponent<UIButton>();
        }
        
        private void OnEnable()
        {
            canselButton.OnClickEvent += OnUpgradeCancelButtonClick;
        }

        private void OnDisable()
        {
            canselButton.OnClickEvent -= OnUpgradeCancelButtonClick;
        }

        private void OnUpgradeCancelButtonClick()
        {
            coffeeCanvasLiaison.DisableCanvas();
            playerBusyness.SetPlayerFree();
            selectedTableEffect.ActivateEffectWithDelay();
        }
    }
}