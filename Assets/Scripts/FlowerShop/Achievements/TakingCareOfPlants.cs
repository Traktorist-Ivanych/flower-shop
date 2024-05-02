namespace FlowerShop.Achievements
{
    public class TakingCareOfPlants : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.TakingCareOfPlants;
            achievementDescription.text = achievementsTextSettings.TakingCareOfPlantsDescription;
            
            achievementMaxProgress = achievementsSettings.TakingCareOfPlantsMaxProgress;
            UpdateScrollbar();
        }
    }
}