namespace FlowerShop.Achievements
{
    public class CustomerFocus : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.CustomerFocus;
            achievementDescription.text = achievementsTextSettings.CustomerFocusDescription;

            achievementMaxProgress = achievementsSettings.CustomerFocusMaxProgress;
            UpdateScrollbar();
        }
    }
}