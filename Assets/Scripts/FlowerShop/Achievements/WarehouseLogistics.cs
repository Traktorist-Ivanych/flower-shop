namespace FlowerShop.Achievements
{
    public class WarehouseLogistics : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.WarehouseLogistics;
            achievementDescription.text = achievementsTextSettings.WarehouseLogisticsDescription;
            
            achievementMaxProgress = achievementsSettings.WarehouseLogisticsMaxProgress;
            UpdateScrollbar();
        }
    }
}