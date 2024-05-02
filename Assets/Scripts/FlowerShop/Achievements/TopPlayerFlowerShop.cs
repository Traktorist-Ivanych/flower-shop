namespace FlowerShop.Achievements
{
    public class TopPlayerFlowerShop : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.TopPlayerFlowerShop;
            achievementDescription.text = achievementsTextSettings.TopPlayerFlowerShopDescription;

            achievementMaxProgress = achievementsSettings.TopPlayerFlowerShopMaxProgress;
            UpdateScrollbar();
        }
    }
}