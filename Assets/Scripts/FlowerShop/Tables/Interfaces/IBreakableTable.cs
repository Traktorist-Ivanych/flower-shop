namespace FlowerShop.Tables.Interfaces
{
    public interface IBreakableTable
    {
        public bool IsTableBroken
        {
            get;
        }

        public void ShowBreakdownIndicator();

        public void HideBreakdownIndicator();

        public void UseBreakableTable();

        public void FixBreakableFlowerTable(int minQuantity, int maxQuantity);

        public void SetActionsBeforeBrokenQuantity(int minQuantity, int maxQuantity);
    }
}
