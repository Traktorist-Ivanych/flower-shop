using PlayerControl;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class BuyCoffeButton : MonoBehaviour
{
    [Inject] private readonly PlayerMoney playerMoney;
    [Inject] private readonly GameConfiguration gameConfiguration;

    [FormerlySerializedAs("coffeTable")] [SerializeField] private CoffeeTable coffeeTable;

    private Button buyButton;

    // можно перенести на onvalidate, buyButton - serializeField
    private void Start()
    {
        buyButton = GetComponent<Button>();
        buyButton.onClick.AddListener(OnBuyCoffeButtonClick);
    }

    public void OnBuyCoffeButtonClick()
    {
        if (playerMoney.CurrentPlayerMoney - gameConfiguration.CoffePrice >= 0)
        {
            coffeeTable.MakeCoffe();
        }
    }
}
