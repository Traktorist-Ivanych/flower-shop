namespace FlowerShop.Achievements
{
    public class ToCapacity : Achievement
    {
        private void Start()
        {
            achievementMaxProgress = achievementsSettings.ToCapacityMaxProgress;
            UpdateScrollbar();
        }
    }
}