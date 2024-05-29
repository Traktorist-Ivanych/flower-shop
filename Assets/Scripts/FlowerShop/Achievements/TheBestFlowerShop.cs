namespace FlowerShop.Achievements
{
    public class TheBestFlowerShop : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.TheBestFlowerShopMaxProgress;
            UpdateScrollbar();
        }
    }
}