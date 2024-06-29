namespace FlowerShop.Achievements
{
    public class Handyman : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.HandymanMaxProgress;
            UpdateScrollbar();
        }
    }
}