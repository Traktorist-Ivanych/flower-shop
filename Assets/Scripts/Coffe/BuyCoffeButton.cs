using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class BuyCoffeButton : MonoBehaviour
{
    [Inject] private readonly PlayerMoney playerMoney;
    [Inject] private readonly GameConfiguration gameConfiguration;

    [SerializeField] private CoffeTable coffeTable;

    private Button buyButton;

    private void Start()
    {
        buyButton = GetComponent<Button>();
        buyButton.onClick.AddListener(OnBuyCoffeButtonClick);
    }

    public void OnBuyCoffeButtonClick()
    {
        if (playerMoney.CurrentPlayerMoney - gameConfiguration.CoffePrice >= 0)
        {
            coffeTable.MakeCoffe();
        }
    }
}
