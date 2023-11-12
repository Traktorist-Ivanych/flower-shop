using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class PlayerStatsModelView : MonoBehaviour
{
    [SerializeField] private Canvas playerStatsCanvas;

    public event PropertyChangedEventHandler PropertyChanged;

    private string playerMoney;

    [Binding]
    public string PlayerMoney
    {
        get => playerMoney;
    }
}
