namespace FlowerShop.Achievements
{
    public class GreatCollector : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.GreatCollectorMaxProgress;
            UpdateScrollbar();
        }
    }
}