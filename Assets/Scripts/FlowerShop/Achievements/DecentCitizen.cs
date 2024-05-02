namespace FlowerShop.Achievements
{
    public class DecentCitizen : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.DecentCitizen;
            achievementDescription.text = achievementsTextSettings.DecentCitizenDescription;
            
            achievementMaxProgress = achievementsSettings.DecentCitizenMaxProgress;
            UpdateScrollbar();
        }
    }
}