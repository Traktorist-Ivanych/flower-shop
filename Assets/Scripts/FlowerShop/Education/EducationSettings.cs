using UnityEngine;
using UnityEngine.Localization;

namespace FlowerShop.Education
{
    [CreateAssetMenu(
        fileName = "EducationSettings", 
        menuName = "Settings/Education Settings", 
        order = 17)]
    public class EducationSettings : ScriptableObject
    {
        [field: SerializeField] public float HeightCanvasFieldsForEducationText { get; private set; }
        [field: SerializeField] public LocalizedString WelcomeText { get; private set; }
        [field: SerializeField] public LocalizedString FirstDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString MoneyAndShopRatingDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString FlowersCanvasDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString PotsRackDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString SoilPreparationDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString SeedPlantDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString GrowingTableDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString PlantNextFlowerDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString FertilizersDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString FertilizersEndUsingDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString CrossingTableDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString CrossingTableProcessDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString CrossingTableProcessStartDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString BrokenTableDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString UpgradeTableDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString CompleteUpgradingTableDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString SoilForNewSeedDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString WaitThenPlantNewSeedDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString PlantNewSeedDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString GrowthAcceleratorUsingDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString GrowthAcceleratorEndUsingDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString TakePotFromCrossingTableDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString WeedDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString PourPotFirstTimeDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString GrowingLvlIncreaserDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString PourPotSecondTimeDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString CollectionRoomDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString PutPotInCollectionRoomDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString StorageTableRoomDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString SalesDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString PutPotOnSaleTableDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString PutPotOnPotsRackDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString PourPotLastTimeDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString ComputerTableDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString ComplaintsDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString VipDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString AchievementsText { get; private set; }
        [field: SerializeField] public LocalizedString CoffeeDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString MusicDescriptionText { get; private set; }
        [field: SerializeField] public LocalizedString EducationEndDescriptionText { get; private set; }
    }
}