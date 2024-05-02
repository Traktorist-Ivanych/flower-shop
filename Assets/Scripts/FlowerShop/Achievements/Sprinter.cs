namespace FlowerShop.Achievements
{
    public class Sprinter : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.Sprinter;
            achievementDescription.text = achievementsTextSettings.SprinterDescription;
            
            achievementMaxProgress = achievementsSettings.SprinterMaxProgress;
            UpdateScrollbar();
        }
    }
}