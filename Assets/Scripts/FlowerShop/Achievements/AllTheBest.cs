namespace FlowerShop.Achievements
{
    public class AllTheBest : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.AllTheBestMaxProgress;
            UpdateScrollbar();
        }
    }
}