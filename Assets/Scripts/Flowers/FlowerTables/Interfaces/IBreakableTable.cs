using System.Collections;

public interface IBreakableTable
{
    public bool IsTableBroken
    {
        get;
    }

    public void ShowBreakdownIndicator();

    public void HideBreakdownIndicator();

    public void UseBreakableFlowerTable();

    public void FixBreakableFlowerTable(int minQuantity, int maxQuantity);

    public void SetActionsBeforeBrokenQuantity(int minQuantity, int maxQuantity);
}
