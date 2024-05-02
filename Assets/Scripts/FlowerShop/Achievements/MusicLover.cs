namespace FlowerShop.Achievements
{
    public class MusicLover : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.MusicLover;
            achievementDescription.text = achievementsTextSettings.MusicLoverDescription;
            
            achievementMaxProgress = achievementsSettings.MusicLoverMaxProgress;
            UpdateScrollbar();
        }
    }
}