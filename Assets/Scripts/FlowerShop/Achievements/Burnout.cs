namespace FlowerShop.Achievements
{
    public class Burnout : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.Burnout;
            achievementDescription.text = achievementsTextSettings.BurnoutDescription;
            
            achievementMaxProgress = achievementsSettings.BurnoutMaxProgress;
            UpdateScrollbar();
        }
    }
}