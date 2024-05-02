namespace FlowerShop.Achievements
{
    public class ToCapacity : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.ToCapacity;
            achievementDescription.text = achievementsTextSettings.ToCapacityDescription;
            
            achievementMaxProgress = achievementsSettings.ToCapacityMaxProgress;
            UpdateScrollbar();
        }
    }
}