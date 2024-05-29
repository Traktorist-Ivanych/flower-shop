namespace FlowerShop.Achievements
{
    public class WarehouseLogistics : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.WarehouseLogisticsMaxProgress;
            UpdateScrollbar();
        }
    }
}