namespace FlowerShop.Achievements
{
    public class MusicLover : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.MusicLoverMaxProgress;
            UpdateScrollbar();
        }
    }
}