namespace FlowerShop.Achievements
{
    public class Burnout : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.BurnoutMaxProgress;
            UpdateScrollbar();
        }
    }
}