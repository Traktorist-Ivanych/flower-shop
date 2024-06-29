using UnityEngine;
using UnityEngine.Localization;

namespace FlowerShop.Help
{
    [CreateAssetMenu(
        fileName = "HelpTexts",
        menuName = "Settings/Help Texts",
        order = 20)]
    public class HelpTexts : ScriptableObject
    {
        [field: SerializeField] public float HeightCanvasFieldsForHelpText { get; private set; }
        [field: SerializeField] public float HeightCanvasFieldsForTableInfoText { get; private set; }
        [field: SerializeField] public Sprite EnableSprite { get; private set; }
        [field: SerializeField] public Sprite DisableSprite { get; private set; }
        [field: SerializeField] public LocalizedString PlayerBusy {  get; private set; }
        [field: SerializeField] public LocalizedString TableAlreadyHasPot { get; private set; }
        [field: SerializeField] public LocalizedString MismatchGrowingRoom { get; private set; }
        [field: SerializeField] public LocalizedString BrokenTable { get; private set; }
        [field: SerializeField] public LocalizedString NoFlowerPlanted { get; private set; }
        [field: SerializeField] public LocalizedString FlowerAlreadyGrown { get; private set; }
        [field: SerializeField] public LocalizedString EmptyTable { get; private set; }
        [field: SerializeField] public LocalizedString FlowerDoesNotNeedWatering { get; private set; }
        [field: SerializeField] public LocalizedString EmptyWateringCan { get; private set; }
        [field: SerializeField] public LocalizedString NoWeed { get; private set; }
        [field: SerializeField] public LocalizedString FertilizersAreOut { get; private set; }
        [field: SerializeField] public LocalizedString FlowerAlreadyProcessed { get; private set; }
        [field: SerializeField] public LocalizedString TableHasMaxLvl { get; private set; }
        [field: SerializeField] public LocalizedString HandsFull { get; private set; }
        [field: SerializeField] public LocalizedString CoffeeEffectAlreadyActive { get; private set; }
        [field: SerializeField] public LocalizedString WeedInPot { get; private set; }
        [field: SerializeField] public LocalizedString WrongFlower { get; private set; }
        [field: SerializeField] public LocalizedString FlowerDidNotGrow { get; private set; }
        [field: SerializeField] public LocalizedString EmptyHands { get; private set; }
        [field: SerializeField] public LocalizedString WrongPickableObject { get; private set; }
        [field: SerializeField] public LocalizedString IncreaseNumberOfFlowersForSale { get; private set; }
        [field: SerializeField] public LocalizedString SeedCrossing { get; private set; }
        [field: SerializeField] public LocalizedString SeedAlreadyCrossed { get; private set; }
        [field: SerializeField] public LocalizedString CrossingTableCanNotStartCrossing { get; private set; }
        [field: SerializeField] public LocalizedString SeedNotCrossed { get; private set; }
        [field: SerializeField] public LocalizedString NoSoilInsidePot { get; private set; }
        [field: SerializeField] public LocalizedString MismatchGrowingRoomForCrossingTable { get; private set; }
        [field: SerializeField] public LocalizedString FlowerAlreadyInCollection { get; private set; }
        [field: SerializeField] public LocalizedString MusicPowerOff { get; private set; }
        [field: SerializeField] public LocalizedString FlowerAlreadyPlanted { get; private set; }
        [field: SerializeField] public LocalizedString NotEnoughMoney { get; private set; }
        [field: SerializeField] public LocalizedString NotEnoughPots { get; private set; }
        [field: SerializeField] public LocalizedString PotNotEmpty { get; private set; }
        [field: SerializeField] public LocalizedString PotEmpty { get; private set; }
        [field: SerializeField] public LocalizedString FlowerNotRare { get; private set; }
        [field: SerializeField] public LocalizedString WateringCanReplenish { get; private set; }
        [field: SerializeField] public LocalizedString CoffeeEffectPurchased { get; private set; }
        [field: SerializeField] public LocalizedString NoAvailableVipOrders { get; private set; }
        [field: SerializeField] public LocalizedString NoAvailableComplaintOrders { get; private set; }
        [field: SerializeField] public LocalizedString RatingHelpText { get; private set; }
        [field: SerializeField] public LocalizedString InconsistencyFlowerAndCrossingTableLvl { get; private set; }
        [field: SerializeField] public LocalizedString InconsistencyFlowerAndGrowingTableLvl { get; private set; }
    }
}
