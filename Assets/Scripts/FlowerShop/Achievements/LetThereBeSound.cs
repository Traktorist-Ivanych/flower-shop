namespace FlowerShop.Achievements
{
    public class LetThereBeSound : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.LetThereBeSound;
            achievementDescription.text = achievementsTextSettings.LetThereBeSoundDescription;
            
            achievementMaxProgress = achievementsSettings.LetThereBeSoundMaxProgress;
            UpdateScrollbar();
        }
    }
}
