namespace FlowerShop.RepairsAndUpgrades
{
    public interface IUpgradableTable
    {
        public void AddUpgradableTableToList();

        public void ShowUpgradeCanvas();
        
        public void UpgradeTableFinish();

        public void ShowIndicator();

        public void HideIndicator();
    }
}
