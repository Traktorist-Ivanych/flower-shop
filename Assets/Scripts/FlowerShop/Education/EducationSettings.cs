using UnityEngine;

namespace FlowerShop.Education
{
    [CreateAssetMenu(
        fileName = "EducationSettings", 
        menuName = "Settings/Education Settings", 
        order = 17)]
    public class EducationSettings : ScriptableObject
    {
        [field: Header("Texts")]
        [field: SerializeField] public Vector2 WelcomeCoordinates { get; private set; }
        [field: SerializeField, Multiline] public string WelcomeText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 FirstDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string FirstDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 MoneyAndShopRatingDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string MoneyAndShopRatingDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 FlowersCanvasDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string FlowersCanvasDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 PotsRackDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string PotsRackDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 SoilPreparationDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string SoilPreparationDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 SeedPlantDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string SeedPlantDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 GrowingTableDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string GrowingTableDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 PlantNextFlowerDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string PlantNextFlowerDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 FertilizersDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string FertilizersDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 FertilizersEndUsingDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string FertilizersEndUsingDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 CrossingTableDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string CrossingTableDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 CrossingTableProcessDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string CrossingTableProcessDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 CrossingTableProcessStartDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string CrossingTableProcessStartDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 BrokenTableDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string BrokenTableDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 UpgradeTableDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string UpgradeTableDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 CompleteUpgradingTableDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string CompleteUpgradingTableDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 SoilForNewSeedDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string SoilForNewSeedDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 WaitThenPlantNewSeedDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string WaitThenPlantNewSeedDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 PlantNewSeedDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string PlantNewSeedDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 GrowthAcceleratorUsingDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string GrowthAcceleratorUsingDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 GrowthAcceleratorEndUsingDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string GrowthAcceleratorEndUsingDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 TakePotFromCrossingTableDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string TakePotFromCrossingTableDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 WeedDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string WeedDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 PourPotFirstTimeDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string PourPotFirstTimeDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 GrowingLvlIncreaserDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string GrowingLvlIncreaserDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 PourPotSecondTimeDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string PourPotSecondTimeDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 CollectionRoomDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string CollectionRoomDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 PutPotInCollectionRoomDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string PutPotInCollectionRoomDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 StorageTableDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string StorageTableRoomDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 SalesDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string SalesDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 PutPotOnSaleTableDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string PutPotOnSaleTableDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 PutPotOnPotsRackDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string PutPotOnPotsRackDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 PourPotLastTimeDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string PourPotLastTimeDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 ComputerTableDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string ComputerTableDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 ComplaintsDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string ComplaintsDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 VipDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string VipDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 CoffeeDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string CoffeeDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 MusicDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string MusicDescriptionText { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 EducationEndDescriptionCoordinates { get; private set; }
        [field: SerializeField] public string EducationEndDescriptionText { get; private set; }
    }
}