namespace FlowerShop.Achievements
{
    public class GreatCollector : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.GreatCollector;
            achievementDescription.text = achievementsTextSettings.GreatCollectorDescription;
            
            achievementMaxProgress = achievementsSettings.GreatCollectorMaxProgress;
            UpdateScrollbar();
        }
    }
}