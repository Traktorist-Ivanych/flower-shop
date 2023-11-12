using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class ImprovementModelView : MonoBehaviour, INotifyPropertyChanged
{
    [SerializeField] private Canvas improvementCanvas;

    public event PropertyChangedEventHandler PropertyChanged;

    private int priceInt;
    private string tableName;
    private string description;
    private string price;
    private Sprite tableSprite;

    public int PriceInt
    {
        get => priceInt;
    }

    [Binding]
    public string TableName
    {
        get => tableName;
    }

    [Binding]
    public string Description
    {
        get => description;
    }

    [Binding]
    public string Price
    {
        get => price;
    }

    [Binding]
    public Sprite TableSprite
    {
        get => tableSprite;
    }

    public void SetImprovementInfo(string tableName, string description, int priceInt, Sprite tableSprite)
    {
        improvementCanvas.enabled = true;

        this.tableName = tableName;
        OnPropertyChanged("TableName");

        this.description = description;
        OnPropertyChanged("Description");

        this.priceInt = priceInt;
        price = priceInt.ToString();
        OnPropertyChanged("Price");

        this.tableSprite = tableSprite;
        OnPropertyChanged("TableSprite");
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
