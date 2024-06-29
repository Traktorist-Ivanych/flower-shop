namespace FlowerShop.Achievements
{
    public class LoverOfExoticFlowers : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.LoverOfExoticFlowersMaxProgress;
            UpdateScrollbar();
        }
    }
}