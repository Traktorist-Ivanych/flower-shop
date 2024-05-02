namespace FlowerShop.Achievements
{
    public class HardworkingBreeder : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.HardworkingBreeder;
            achievementDescription.text = achievementsTextSettings.HardworkingBreederDescription;
            
            achievementMaxProgress = achievementsSettings.HardworkingBreederMaxProgress;
            UpdateScrollbar();
        }
    }
}