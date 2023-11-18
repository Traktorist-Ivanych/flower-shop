using UnityEngine;
using Zenject;

public class PlayerMoney : MonoBehaviour
{
    [Inject] private readonly AllCanvasLiaisons allCanvasLiaisons;

    private int currentPlayerMoney = 5000;

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
        allCanvasLiaisons.PlayerStatsCanvasLiaison.UpdatePlayerMoneyOnCanvas(currentPlayerMoney.ToString());
    }
}
