namespace FlowerShop.Achievements
{
    public class TakingCareOfPlants : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.TakingCareOfPlantsMaxProgress;
            UpdateScrollbar();
        }
    }
}