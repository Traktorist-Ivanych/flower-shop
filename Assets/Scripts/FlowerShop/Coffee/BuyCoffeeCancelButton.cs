using FlowerShop.Effects;
using PlayerControl;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.Coffee
{
    [RequireComponent(typeof(Button))]
    public class BuyCoffeeCancelButton : MonoBehaviour
    {
        [Inject] private readonly CoffeeCanvasLiaison coffeeCanvasLiaison;
        [Inject] private readonly PlayerBusyness playerBusyness;
        [Inject] private readonly SelectedTableEffect selectedTableEffect;
        
        [HideInInspector, SerializeField] private Button canselButton;

        private void OnValidate()
        {
            canselButton = GetComponent<Button>();
        }
        
        private void OnEnable()
        {
            canselButton.onClick.AddListener(OnUpgradeCancelButtonClick);
        }

        private void OnDisable()
        {
            canselButton.onClick.RemoveListener(OnUpgradeCancelButtonClick);
        }

        private void OnUpgradeCancelButtonClick()
        {
            coffeeCanvasLiaison.CoffeeCanvas.enabled = false;
            playerBusyness.SetPlayerFree();
            selectedTableEffect.ActivateEffectWithDelay();
        }
    }
}