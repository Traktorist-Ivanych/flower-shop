namespace FlowerShop.Achievements
{
    public class LoverOfExoticFlowers : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.LoverOfExoticFlowers;
            achievementDescription.text = achievementsTextSettings.LoverOfExoticFlowersDescription;
            
            achievementMaxProgress = achievementsSettings.LoverOfExoticFlowersMaxProgress;
            UpdateScrollbar();
        }
    }
}