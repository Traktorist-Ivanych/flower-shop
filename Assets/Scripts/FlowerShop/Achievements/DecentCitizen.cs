namespace FlowerShop.Achievements
{
    public class DecentCitizen : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.DecentCitizenMaxProgress;
            UpdateScrollbar();
        }
    }
}