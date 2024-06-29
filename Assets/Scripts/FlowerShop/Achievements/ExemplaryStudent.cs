namespace FlowerShop.Achievements
{
    public class ExemplaryStudent : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.ExemplaryStudentMaxProgress;
            UpdateScrollbar();
        }
    }
}