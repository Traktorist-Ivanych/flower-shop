namespace FlowerShop.Achievements
{
    public class WeedKiller : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.WeedKillerMaxProgress;
            UpdateScrollbar();
        }
    }
}