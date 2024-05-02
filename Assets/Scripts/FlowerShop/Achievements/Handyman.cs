namespace FlowerShop.Achievements
{
    public class Handyman : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.Handyman;
            achievementDescription.text = achievementsTextSettings.HandymanDescription;
            
            achievementMaxProgress = achievementsSettings.HandymanMaxProgress;
            UpdateScrollbar();
        }
    }
}