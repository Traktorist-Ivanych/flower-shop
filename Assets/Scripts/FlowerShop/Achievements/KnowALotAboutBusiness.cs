namespace FlowerShop.Achievements
{
    public class KnowALotAboutBusiness : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.KnowALotAboutBusiness;
            achievementDescription.text = achievementsTextSettings.KnowALotAboutBusinessDescription;

            achievementMaxProgress = achievementsSettings.KnowALotAboutBusinessMaxProgress;
            UpdateScrollbar();
        }
    }
}