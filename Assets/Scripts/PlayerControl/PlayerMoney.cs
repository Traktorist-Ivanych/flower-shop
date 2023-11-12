using UnityEngine;
using Zenject;

public class PlayerMoney : MonoBehaviour
{
    [Inject] private ModelViewAll ModelViewAll;

    private int currentPlayerMoney = 0;

    public int CurrentPlayerMoney
    {
        get => currentPlayerMoney;
    }

    private void Start()
    {
        UpdatePlayerMoneyOnCanvas();
    }

    public void AddPlayerMoney(int moneyAmount)
    {
        currentPlayerMoney += moneyAmount;
        UpdatePlayerMoneyOnCanvas();
    }

    public void TakePlayerMoney(int moneyAmount)
    {
        currentPlayerMoney -= moneyAmount;
        UpdatePlayerMoneyOnCanvas();
    }

    private void UpdatePlayerMoneyOnCanvas()
    {
        ModelViewAll.PlayerStatsModelView.UpdatePlayerMoneyOnCanvas(currentPlayerMoney.ToString());
    }
}
