using UnityEngine;

namespace FlowerShop.Weeds
{
    [CreateAssetMenu(
        fileName = "WeedSettings", 
        menuName = "Settings/Weed Settings", 
        order = 10)]
    public class WeedSettings : ScriptableObject
    {
        [field: SerializeField] public int MaxWeedGrowingLvl { get; private set; }
        
        [field: Tooltip("Determines what level will be set when weed will be planted")]
        [field: SerializeField] public int PrimaryWeedGrowingLvl { get; private set; }
    }
}