namespace FlowerShop.Achievements
{
    public class AspiringCollector : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.AspiringCollectorMaxProgress;
            UpdateScrollbar();
        }
    }
}