namespace FlowerShop.Achievements
{
    public class WildflowerLover : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.WildflowerLoverMaxProgress;
            UpdateScrollbar();
        }
    }
}