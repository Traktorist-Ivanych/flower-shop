using UnityEngine;
using UnityEngine.Localization;

namespace FlowerShop.FlowersSale
{
    [CreateAssetMenu(
        fileName = "FlowersForSaleCoeffCalculatorSettings", 
        menuName = "Settings/Flowers For Sale Coeff Calculator Settings", 
        order = 8)]
    public class FlowersForSaleCoeffCalculatorSettings : ScriptableObject
    {
        [field: SerializeField] public int MinShopGrade { get; private set; }
        [field: SerializeField] public int MaxShopGrade { get; private set; }
        [field: SerializeField] public int MaxUniqueFlowersForSale { get; private set; }
        [field: SerializeField] public LocalizedString FractionalSeparationSign { get; private set; }
    }
}