using UnityEngine;

namespace FlowerShop.FlowerSales
{
    [CreateAssetMenu(
        fileName = "FlowersForSaleCoeffCalculatorSettings", 
        menuName = "Settings/Flowers For Sale Coeff Calculator Settings", 
        order = 8)]
    public class FlowersForSaleCoeffCalculatorSettings : ScriptableObject
    {
        [field: Tooltip("Number of all flowers, offered for sale, to get highest coefficient")]
        [field: SerializeField] public float AllFlowersForSale { get; private set; }
        
        [field: Tooltip("Number of unique flowers, offered for sale, to get highest coefficient")]
        [field: SerializeField] public float UniqueFlowersForSale { get; private set; }
    }
}