using UnityEngine;

namespace FlowerShop.Tables
{
    [CreateAssetMenu(
        fileName = "TablesSettings", 
        menuName = "Settings/Tables Settings", 
        order = 9)]
    public class TablesSettings : ScriptableObject
    {
        [field: Header("Flowers Growing Table")]
        [field: Tooltip("Determines what GrowingLvlTime will be set when flower or weed " +
                        "will be planted or increased there growing lvl")]
        [field: SerializeField] public float PrimaryGrowingLvlTime { get; private set; }
        [field: SerializeField] public float UpGrowingLvlTime { get; private set; }
        [field: SerializeField] public float UpGrowingLvlTableLvlTimeDelta { get; private set; }
        [field: SerializeField] public int FansEnableLvl { get; private set; }
        
        
        [field: Header("Watering Table")]
        [field: SerializeField] public int WateringsNumber { get; private set; }
        [field: SerializeField] public int WateringsNumberLvlDelta { get; private set; }
        [field: SerializeField] public float TimeForReplenishWateringCanWithOneWatering { get; private set; }
        
        [field: Header("Soil Preparation Table")]
        [field: SerializeField] public float SoilPreparationTime { get; private set; }
        [field: SerializeField] public float SoilPreparationLvlTimeDelta { get; private set; }

        [field: Header("Flowers Crossing Table")]
        [field: SerializeField] public float CrossingFlowerTime { get; private set; }
        [field: SerializeField] public float CrossingFlowerLvlTimeDelta { get; private set; }
        
        [field: Header("Pots Rack")]
        [field: SerializeField] public int PotsCountAvailableOnStart { get; private set; }
        [field: Tooltip("Determines number, by which count of available pots will increase, " +
                        "when table will be upgrade by 1 lvl")]
        [field: SerializeField] public int PotsCountAvailableOnUpgradeDelta { get; private set; }
    }
}