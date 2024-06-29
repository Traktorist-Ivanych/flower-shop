namespace FlowerShop.Achievements
{
    public class HardworkingBreeder : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.HardworkingBreederMaxProgress;
            UpdateScrollbar();
        }
    }
}