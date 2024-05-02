namespace FlowerShop.Achievements
{
    public class PlantGrowingPlant : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.PlantGrowingPlant;
            achievementDescription.text = achievementsTextSettings.PlantGrowingPlantDescription;

            achievementMaxProgress = achievementsSettings.PlantGrowingPlantMaxProgress;
            UpdateScrollbar();
        }
    }
}