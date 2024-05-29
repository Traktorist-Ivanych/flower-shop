namespace FlowerShop.Achievements
{
    public class ThereIsNoTimeToWait : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.ThereIsNoTimeToWaitMaxProgress;
            UpdateScrollbar();
        }
    }
}