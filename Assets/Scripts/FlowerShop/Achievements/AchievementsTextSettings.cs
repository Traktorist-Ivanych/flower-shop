using UnityEngine;

namespace FlowerShop.Achievements
{
    [CreateAssetMenu(
        fileName = "AchievementsTextSettings", 
        menuName = "Settings/Achievements Text Settings", 
        order = 18)]
    public class AchievementsTextSettings : ScriptableObject
    {
        [field: SerializeField] public string GreatCollector { get; private set; }
        [field: SerializeField] public string GreatCollectorDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string AllTheBest { get; private set; }
        [field: SerializeField] public string AllTheBestDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string Burnout { get; private set; }
        [field: SerializeField] public string BurnoutDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string LetThereBeSound { get; private set; }
        [field: SerializeField] public string LetThereBeSoundDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string ThereIsNoTimeToWait { get; private set; }
        [field: SerializeField] public string ThereIsNoTimeToWaitDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string PlantGrowingPlant { get; private set; }
        [field: SerializeField] public string PlantGrowingPlantDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string KnowALotAboutBusiness { get; private set; }
        [field: SerializeField] public string KnowALotAboutBusinessDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string CustomerFocus { get; private set; }
        [field: SerializeField] public string CustomerFocusDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string TopPlayerFlowerShop { get; private set; }
        [field: SerializeField] public string TopPlayerFlowerShopDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string TheBestFlowerShop { get; private set; }
        [field: SerializeField] public string TheBestFlowerShopDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string LoverOfDecorativeFlowers { get; private set; }
        [field: SerializeField] public string LoverOfDecorativeFlowersDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string CoffeeLover { get; private set; }
        [field: SerializeField] public string CoffeeLoverDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string WildflowerLover { get; private set; }
        [field: SerializeField] public string WildflowerLoverDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string LoverOfExoticFlowers { get; private set; }
        [field: SerializeField] public string LoverOfExoticFlowersDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string Handyman { get; private set; }
        [field: SerializeField] public string HandymanDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string MusicLover { get; private set; }
        [field: SerializeField] public string MusicLoverDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string ToCapacity { get; private set; }
        [field: SerializeField] public string ToCapacityDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string DecentCitizen { get; private set; }
        [field: SerializeField] public string DecentCitizenDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string ExemplaryStudent { get; private set; }
        [field: SerializeField] public string ExemplaryStudentDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string TakingCareOfPlants { get; private set; }
        [field: SerializeField] public string TakingCareOfPlantsDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string Dedication { get; private set; }
        [field: SerializeField] public string DedicationDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string WarehouseLogistics { get; private set; }
        [field: SerializeField] public string WarehouseLogisticsDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string Sprinter { get; private set; }
        [field: SerializeField] public string SprinterDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string HardworkingBreeder { get; private set; }
        [field: SerializeField] public string HardworkingBreederDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string WeedKiller { get; private set; }
        [field: SerializeField] public string WeedKillerDescription { get; private set; }
        [field: Space]
        [field: SerializeField] public string AspiringCollector { get; private set; }
        [field: SerializeField] public string AspiringCollectorDescription { get; private set; }
    }
}
