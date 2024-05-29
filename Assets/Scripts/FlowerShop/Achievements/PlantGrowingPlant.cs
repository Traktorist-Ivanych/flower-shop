namespace FlowerShop.Achievements
{
    public class PlantGrowingPlant : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.PlantGrowingPlantMaxProgress;
            UpdateScrollbar();
        }
    }
}