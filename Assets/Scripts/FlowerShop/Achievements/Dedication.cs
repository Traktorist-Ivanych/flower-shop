namespace FlowerShop.Achievements
{
    public class Dedication : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.Dedication;
            achievementDescription.text = achievementsTextSettings.DedicationDescription;
            
            achievementMaxProgress = achievementsSettings.DedicationMaxProgress;
            UpdateScrollbar();
        }
    }
}