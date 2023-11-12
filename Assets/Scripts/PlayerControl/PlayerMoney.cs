using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    private int currentPlayerMoney = 50000;

    public int CurrentPlayerMoney
    {
        get => currentPlayerMoney;
    }

    public void AddPlayerMoney(int moneyAmount)
    {
        currentPlayerMoney += moneyAmount;
    }

    public void TakePlayerMoney(int moneyAmount)
    {
        currentPlayerMoney -= moneyAmount;
    }
}
