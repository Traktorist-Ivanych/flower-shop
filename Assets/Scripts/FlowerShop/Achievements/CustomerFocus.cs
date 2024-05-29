namespace FlowerShop.Achievements
{
    public class CustomerFocus : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.CustomerFocusMaxProgress;
            UpdateScrollbar();
        }
    }
}