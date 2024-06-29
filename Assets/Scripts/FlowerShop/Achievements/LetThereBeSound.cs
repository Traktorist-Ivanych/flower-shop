namespace FlowerShop.Achievements
{
    public class LetThereBeSound : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.LetThereBeSoundMaxProgress;
            UpdateScrollbar();
        }
    }
}
