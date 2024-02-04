using UnityEngine;

namespace FlowerShop.Weeds
{
    [CreateAssetMenu(
        fileName = "WeedSettings", 
        menuName = "Settings/Weed Settings", 
        order = 10)]
    public class WeedSettings : ScriptableObject
    {
        [Header("Planting Weed Chance")]
        [SerializeField] private AnimationCurve weedPlanting;
        [SerializeField] private float weedPlantingSuccessBorder;
        
        [field: Header("Growing Lvl")]
        [field: SerializeField] public int MaxWeedGrowingLvl { get; private set; }
        
        [field: Tooltip("Determines what level will be set when weed will be planted")]
        [field: SerializeField] public int PrimaryWeedGrowingLvl { get; private set; }
        
        [field: Header("Times")]
        [field: SerializeField] public float MinWeedPlantTime { get; private set; }
        [field: SerializeField] public float MinWeedPlantTimeLvlDelta { get; private set; }
        [field: SerializeField] public float MaxWeedPlantTime { get; private set; }
        [field: SerializeField] public float MaxWeedPlantTimeLvlDelta { get; private set; }
        [field: SerializeField] public float WeedingTime { get; private set; }
        [field: SerializeField] public float WeedingTimeLvlDelta { get; private set; }
        
        public bool ShouldWeedBePlanting()
        {
            return weedPlanting.Evaluate(Random.Range(0, 1f)) >= weedPlantingSuccessBorder;
        }
    }
}