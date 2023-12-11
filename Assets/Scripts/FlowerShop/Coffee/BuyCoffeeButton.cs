using FlowerShop.Tables;
using PlayerControl;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.Coffee
{
    [RequireComponent(typeof(Button))]
    public class BuyCoffeeButton : MonoBehaviour
    {
        [Inject] private readonly PlayerMoney playerMoney;
        [Inject] private readonly CoffeeSettings coffeeSettings;

        [SerializeField] private CoffeeTable coffeeTable;

        [HideInInspector, SerializeField] private Button buyButton;

        private void OnValidate()
        {
            buyButton = GetComponent<Button>();
        }

        private void OnEnable()
        {
            buyButton.onClick.AddListener(OnBuyCoffeeButtonClick);
        }

        private void OnDisable()
        {
            buyButton.onClick.RemoveListener(OnBuyCoffeeButtonClick);
        }

        private void OnBuyCoffeeButtonClick()
        {
            if (playerMoney.CurrentPlayerMoney - coffeeSettings.CoffeePrice >= 0)
            {
                StartCoroutine(coffeeTable.MakeCoffeeProcess());
            }
        }
    }
}
