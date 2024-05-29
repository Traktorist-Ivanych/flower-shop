namespace FlowerShop.Achievements
{
    public class Sprinter : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.SprinterMaxProgress;
            UpdateScrollbar();
        }
    }
}