using UnityEngine;

[RequireComponent(typeof(BreakableFlowerTableBase))]
public abstract class BreakableFlowerTable : FlowerTable, IBreakableTable
{
    [SerializeField] private BreakableFlowerTableBase breakableFlowerTableBase;

    public bool IsTableBroken
    {
        get => breakableFlowerTableBase.IsTableBroken;
    }

    public void ShowBreakdownIndicator()
    {
        breakableFlowerTableBase.ShowBreakdownIndicator();
    }

    public void HideBreakdownIndicator()
    {
        breakableFlowerTableBase.HideBreakdownIndicator();
    }

    public void UseBreakableFlowerTable()
    {
        breakableFlowerTableBase.UseBreakableFlowerTable();
    }

    public void FixBreakableFlowerTable(int minQuantity, int maxQuantity)
    {
        breakableFlowerTableBase.FixBreakableFlowerTable(minQuantity, maxQuantity);
    }

    public void SetActionsBeforeBrokenQuantity(int minQuantity, int maxQuantity)
    {
        breakableFlowerTableBase.SetActionsBeforeBrokenQuantity(minQuantity, maxQuantity);
    }
}
