using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class PlayerStatsCanvasLiaison : MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private string playerMoney;

    [Binding]
    public string PlayerMoney
    {
        get => playerMoney;
    }

    public void UpdatePlayerMoneyOnCanvas(string playerMoney)
    {
        this.playerMoney = playerMoney;
        OnPropertyChanged("PlayerMoney");
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
