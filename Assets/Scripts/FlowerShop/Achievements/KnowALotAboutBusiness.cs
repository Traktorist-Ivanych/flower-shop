namespace FlowerShop.Achievements
{
    public class KnowALotAboutBusiness : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.KnowALotAboutBusinessMaxProgress;
            UpdateScrollbar();
        }
    }
}