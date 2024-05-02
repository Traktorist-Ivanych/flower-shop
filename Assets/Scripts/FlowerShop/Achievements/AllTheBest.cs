namespace FlowerShop.Achievements
{
    public class AllTheBest : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.AllTheBest;
            achievementDescription.text = achievementsTextSettings.AllTheBestDescription;

            achievementMaxProgress = achievementsSettings.AllTheBestMaxProgress;
            UpdateScrollbar();
        }
    }
}