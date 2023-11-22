using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class PlayerStatsCanvasLiaison : MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    [Binding]
    public string PlayerMoney { get; private set; }

    public void UpdatePlayerMoneyOnCanvas(string transmittedPlayerMoney)
    {
        PlayerMoney = transmittedPlayerMoney;
        OnPropertyChanged("PlayerMoney");
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
