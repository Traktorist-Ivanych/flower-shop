using UnityEngine;

namespace FlowerShop.RepairsAndUpgrades
{
    [CreateAssetMenu(
        fileName = "RepairsAndUpgradesSettings", 
        menuName = "Settings/Repairs And Upgrades Settings", 
        order = 11)]
    public class RepairsAndUpgradesSettings : ScriptableObject
    {
        [field: Header("Improvements")]
        [field: SerializeField] public float TableUpgradeTime { get; private set; }
        [field: SerializeField] public int MaxUpgradableTableLvl { get; private set; }

        [field: Header("Repairs")]
        [field: SerializeField] public float TableRepairTime { get; private set; }
        [field: SerializeField] public int SoilPreparationMinQuantity { get; private set; }
        [field: SerializeField] public int SoilPreparationMaxQuantity { get; private set; }
        [field: SerializeField] public int WateringTableMinQuantity { get; private set; }
        [field: SerializeField] public int WateringTableMaxQuantity { get; private set; }
        [field: SerializeField] public int FlowerGrowingTableMinQuantity { get; private set; }
        [field: SerializeField] public int FlowerGrowingTableMaxQuantity { get; private set; }
        [field: SerializeField] public int CrossingTableMinQuantity { get; private set; }
        [field: SerializeField] public int CrossingTableMaxQuantity { get; private set; }
    }
}