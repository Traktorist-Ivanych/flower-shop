using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class ImprovementCanvasLiaison : MonoBehaviour, INotifyPropertyChanged
{
    [field: SerializeField] public Canvas ImprovementCanvas { get; private set; }

    public event PropertyChangedEventHandler PropertyChanged;
    
    [Binding]
    public string TableName { get; private set; }
    
    [Binding]
    public string Description { get; private set; }
    
    [Binding]
    public Sprite TableSprite { get; private set; }
    
    [Binding]
    public string Price { get; private set; }
    
    public int PriceInt { get; private set; }
    
    public void SetImprovementInfo(string tableName, string description, int priceInt, Sprite tableSprite)
    {
        ImprovementCanvas.enabled = true;

        TableName = tableName;
        OnPropertyChanged(nameof(TableName));

        TableSprite = tableSprite;
        OnPropertyChanged(nameof(TableSprite));

        Description = description;
        OnPropertyChanged(nameof(Description));

        PriceInt = priceInt;
        Price = priceInt.ToString();
        OnPropertyChanged(nameof(Price));
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
