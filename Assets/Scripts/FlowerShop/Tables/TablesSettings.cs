using UnityEngine;

namespace FlowerShop.Tables
{
    [CreateAssetMenu(
        fileName = "TablesSettings", 
        menuName = "Settings/Tables Settings", 
        order = 9)]
    public class TablesSettings : ScriptableObject
    {
        [field: Header("FlowerGrowingTable")]
        [field: Tooltip("Determines what GrowingLvlTime will be set when flower or weed " +
                        "will be planted or increased there growing lvl")]
        [field: SerializeField] public float PrimaryGrowingLvlTime { get; private set; }
        [field: SerializeField] public float UpGrowingLvlTime { get; private set; }
        [field: SerializeField] public float UpGrowingLvlTableLvlTimeDelta { get; private set; }
    }
}