namespace FlowerShop.Achievements
{
    public class ThereIsNoTimeToWait : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.ThereIsNoTimeToWait;
            achievementDescription.text = achievementsTextSettings.ThereIsNoTimeToWaitDescription;
            
            achievementMaxProgress = achievementsSettings.ThereIsNoTimeToWaitMaxProgress;
            UpdateScrollbar();
        }
    }
}