namespace FlowerShop.Achievements
{
    public class LoverOfDecorativeFlowers : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.LoverOfDecorativeFlowers;
            achievementDescription.text = achievementsTextSettings.LoverOfDecorativeFlowersDescription;
            
            achievementMaxProgress = achievementsSettings.LoverOfDecorativeFlowersMaxProgress;
            UpdateScrollbar();
        }
    }
}