namespace FlowerShop.Achievements
{
    public class WeedKiller : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.WeedKiller;
            achievementDescription.text = achievementsTextSettings.WeedKillerDescription;
            
            achievementMaxProgress = achievementsSettings.WeedKillerMaxProgress;
            UpdateScrollbar();
        }
    }
}