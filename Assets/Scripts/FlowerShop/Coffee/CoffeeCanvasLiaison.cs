using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;
using Zenject;

[Binding]
public class CoffeeCanvasLiaison : MonoBehaviour, INotifyPropertyChanged
{
    [Inject] private readonly GameConfiguration gameConfiguration;

    [field: SerializeField] public Canvas CoffeeCanvas { get; private set; }
    
    public event PropertyChangedEventHandler PropertyChanged;
    
    [Binding] 
    public string CoffeePrice { get; private set; }

    private void Start()
    {
        CoffeePrice = gameConfiguration.CoffePrice.ToString();
        OnPropertyChanged(nameof(CoffeePrice));
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
