using UnityEngine;

[RequireComponent(typeof(BreakableFlowerTableBaseLogic))]
public abstract class BreakableFlowerTable : FlowerTable, IBreakableTable
{
    [SerializeField] private BreakableFlowerTableBaseLogic breakableBaseLogic;

    public bool IsTableBroken
    {
        get => breakableBaseLogic.IsTableBroken;
    }

    public void ShowBreakdownIndicator()
    {
        breakableBaseLogic.ShowBreakdownIndicator();
    }

    public void HideBreakdownIndicator()
    {
        breakableBaseLogic.HideBreakdownIndicator();
    }

    public void UseBreakableFlowerTable()
    {
        breakableBaseLogic.UseBreakableFlowerTable();
    }

    public void FixBreakableFlowerTable(int minQuantity, int maxQuantity)
    {
        breakableBaseLogic.FixBreakableFlowerTable(minQuantity, maxQuantity);
    }

    public void SetActionsBeforeBrokenQuantity(int minQuantity, int maxQuantity)
    {
        breakableBaseLogic.SetActionsBeforeBrokenQuantity(minQuantity, maxQuantity);
    }
}
