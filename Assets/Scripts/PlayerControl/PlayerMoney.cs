using UnityEngine;
using Zenject;

public class PlayerMoney : MonoBehaviour
{
    [Inject] private readonly PlayerStatsCanvasLiaison playerStatsCanvasLiaison;
    [field: SerializeField] public int CurrentPlayerMoney { get; private set; }

    private void Start()
    {
        UpdatePlayerMoneyOnCanvas();
    }

    public void AddPlayerMoney(int moneyAmount)
    {
        CurrentPlayerMoney += moneyAmount;
        UpdatePlayerMoneyOnCanvas();
    }

    public void TakePlayerMoney(int moneyAmount)
    {
        CurrentPlayerMoney -= moneyAmount;
        UpdatePlayerMoneyOnCanvas();
    }

    private void UpdatePlayerMoneyOnCanvas()
    {
        playerStatsCanvasLiaison.UpdatePlayerMoneyOnCanvas(CurrentPlayerMoney.ToString());
    }
}
