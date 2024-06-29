namespace FlowerShop.Achievements
{
    public class Dedication : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.DedicationMaxProgress;
            UpdateScrollbar();
        }
    }
}