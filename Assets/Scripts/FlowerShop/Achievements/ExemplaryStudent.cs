namespace FlowerShop.Achievements
{
    public class ExemplaryStudent : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.ExemplaryStudent;
            achievementDescription.text = achievementsTextSettings.ExemplaryStudentDescription;
            
            achievementMaxProgress = achievementsSettings.ExemplaryStudentMaxProgress;
            UpdateScrollbar();
        }
    }
}