namespace FlowerShop.Achievements
{
    public class AspiringCollector : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.AspiringCollector;
            achievementDescription.text = achievementsTextSettings.AspiringCollectorDescription;
            
            achievementMaxProgress = achievementsSettings.AspiringCollectorMaxProgress;
            UpdateScrollbar();
        }
    }
}