using UnityEngine;

namespace FlowerShop.Achievements
{
    [CreateAssetMenu(
        fileName = "AchievementsSettings", 
        menuName = "Settings/Achievements Settings", 
        order = 19)]
    public class AchievementsSettings : ScriptableObject
    {
        [Header("Sprites")]
        [field: SerializeField] public Sprite AwardReceived { get; private set; }
        [field: SerializeField] public Sprite AwardAwaitingReceipt { get; private set; }
        [Header("MoneyReward")]
        [field: SerializeField] public int LowMoneyReward { get; private set; }
        [field: SerializeField] public int MediumMoneyReward { get; private set; }
        [field: SerializeField] public int HighMoneyReward { get; private set; }
        [Header("AchievementProgress")]
        [field: SerializeField] public int GreatCollectorMaxProgress { get; private set; }
        [field: SerializeField] public int AllTheBestMaxProgress { get; private set; }
        [field: SerializeField] public int BurnoutMaxProgress { get; private set; }
        [field: SerializeField] public int LetThereBeSoundMaxProgress { get; private set; }
        [field: SerializeField] public int ThereIsNoTimeToWaitMaxProgress { get; private set; }
        [field: SerializeField] public int PlantGrowingPlantMaxProgress { get; private set; }
        [field: SerializeField] public int KnowALotAboutBusinessMaxProgress { get; private set; }
        [field: SerializeField] public int CustomerFocusMaxProgress { get; private set; }
        [field: SerializeField] public int TopPlayerFlowerShopMaxProgress { get; private set; }
        [field: SerializeField] public int TheBestFlowerShopMaxProgress { get; private set; }
        [field: SerializeField] public int LoverOfDecorativeFlowersMaxProgress { get; private set; }
        [field: SerializeField] public int CoffeeLoverMaxProgress { get; private set; }
        [field: SerializeField] public int WildflowerLoverMaxProgress { get; private set; }
        [field: SerializeField] public int LoverOfExoticFlowersMaxProgress { get; private set; }
        [field: SerializeField] public int HandymanMaxProgress { get; private set; }
        [field: SerializeField] public int MusicLoverMaxProgress { get; private set; }
        [field: SerializeField] public int ToCapacityMaxProgress { get; private set; }
        [field: SerializeField] public int DecentCitizenMaxProgress { get; private set; }
        [field: SerializeField] public int ExemplaryStudentMaxProgress { get; private set; }
        [field: SerializeField] public int TakingCareOfPlantsMaxProgress { get; private set; }
        [field: SerializeField] public int DedicationMaxProgress { get; private set; }
        [field: SerializeField] public int WarehouseLogisticsMaxProgress { get; private set; }
        [field: SerializeField] public int SprinterMaxProgress { get; private set; }
        [field: SerializeField] public int HardworkingBreederMaxProgress { get; private set; }
        [field: SerializeField] public int WeedKillerMaxProgress { get; private set; }
        [field: SerializeField] public int AspiringCollectorMaxProgress { get; private set; }
    }
}
