namespace FlowerShop.Upgrades
{
    public interface IUpgradableTable
    {
        public void AddUpgradableTableToList();

        public void ShowUpgradeIndicator();

        public void HideUpgradeIndicator();

        public void ShowUpgradeCanvas();

        public void UpgradeTable();
    }
}
