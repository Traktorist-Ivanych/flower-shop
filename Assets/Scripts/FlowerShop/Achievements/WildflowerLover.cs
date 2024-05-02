namespace FlowerShop.Achievements
{
    public class WildflowerLover : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.WildflowerLover;
            achievementDescription.text = achievementsTextSettings.WildflowerLoverDescription;
            
            achievementMaxProgress = achievementsSettings.WildflowerLoverMaxProgress;
            UpdateScrollbar();
        }
    }
}