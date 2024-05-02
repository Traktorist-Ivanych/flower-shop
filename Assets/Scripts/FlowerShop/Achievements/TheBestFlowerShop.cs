namespace FlowerShop.Achievements
{
    public class TheBestFlowerShop : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.TheBestFlowerShop;
            achievementDescription.text = achievementsTextSettings.TheBestFlowerShopDescription;

            achievementMaxProgress = achievementsSettings.TheBestFlowerShopMaxProgress;
            UpdateScrollbar();
        }
    }
}