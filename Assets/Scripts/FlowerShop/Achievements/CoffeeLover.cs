namespace FlowerShop.Achievements
{
    public class CoffeeLover : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.CoffeeLoverMaxProgress;
            UpdateScrollbar();
        }
    }
}