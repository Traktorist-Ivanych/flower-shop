namespace FlowerShop.Achievements
{
    public class LoverOfDecorativeFlowers : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.LoverOfDecorativeFlowersMaxProgress;
            UpdateScrollbar();
        }
    }
}