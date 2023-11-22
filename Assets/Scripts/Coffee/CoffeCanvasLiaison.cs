using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;
using Zenject;

[Binding]
public class CoffeCanvasLiaison : MonoBehaviour, INotifyPropertyChanged
{
    [Inject] private readonly GameConfiguration gameConfiguration;

    public event PropertyChangedEventHandler PropertyChanged;

    private string coffePrice;

    [Binding]
    public string CoffePrice 
    {
        get => coffePrice;
    }

    private void Start()
    {
        coffePrice = gameConfiguration.CoffePrice.ToString();
        OnPropertyChanged(nameof(CoffePrice));
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
