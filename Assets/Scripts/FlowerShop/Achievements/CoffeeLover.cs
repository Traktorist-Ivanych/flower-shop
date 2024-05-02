namespace FlowerShop.Achievements
{
    public class CoffeeLover : Achievement
    {
        private void Start()
        {
            achievementText.text = achievementsTextSettings.CoffeeLover;
            achievementDescription.text = achievementsTextSettings.CoffeeLoverDescription;
            
            achievementMaxProgress = achievementsSettings.CoffeeLoverMaxProgress;
            UpdateScrollbar();
        }
    }
}