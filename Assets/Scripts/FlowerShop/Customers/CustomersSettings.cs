using FlowerShop.Flowers;
using UnityEngine;
using UnityEngine.Localization;

namespace FlowerShop.Customers
{
    [CreateAssetMenu(fileName = "NewCustomersSettings", 
                     menuName = "Settings/Customers Settings", 
                     order = 6)]
    public class CustomersSettings : ScriptableObject
    {
        [field: Header("Moving")]
        [field: SerializeField] public float MinRemainingDistanceBetweenPathPoints { get; private set; }
        [field: SerializeField] public float RemainingDistanceToStartRotation { get; private set; }
        [field: SerializeField] public float AngelToStopRotation { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public float NavAgentSpeed { get; private set; }
        
        [field: Header("Spawn")]
        [field: SerializeField] public float MinSpawnTime { get; private set; }
        [field: SerializeField] public float MinSpawnTimeDelta { get; private set; }
        [field: SerializeField] public float MaxSpawnTime { get; private set; }
        [field: SerializeField] public float MaxSpawnTimeDelta { get; private set; }
        [field: SerializeField] public float MinTimeBetweenSpawn { get; private set; }
        
        [field: Header("Complaints")]
        [field: SerializeField] public float MinComplaintsTime { get; private set; }
        [field: SerializeField] public float MaxComplaintsTime { get; private set; }
        [field: SerializeField] public LocalizedString[] LocalizedComplaintDescriptions { get; private set; }
        [field: SerializeField] public string[] ComplaintDescriptions { get; private set; }
        
        [field: Header("Vip")]
        [field: SerializeField] public float MaxPriceMultiplerGradesBorder { get; private set; }
        [field: SerializeField] public float MiddlePriceMultiplerGradesBorder { get; private set; }
        [field: SerializeField] public int MinGradesCountForMaxPriceMultipler { get; private set; }
        [field: SerializeField] public float MinPriceMultipler { get; private set; }
        [field: SerializeField] public float MiddlePriceMultipler { get; private set; }
        [field: SerializeField] public float MaxPriceMultipler { get; private set; }
        [field: SerializeField] public int MinFlowersInPlayerCollectionCount { get; private set; }
        [field: SerializeField] public float AverageGradeInfluence { get; private set; }
        [field: SerializeField] public float FlowersInPlayerCollectionInfluence { get; private set; }
        [field: SerializeField] public float MinVipTime { get; private set; }
        [field: SerializeField] public float MinVipTimeDelta { get; private set; }
        [field: SerializeField] public float MaxVipTime { get; private set; }
        [field: SerializeField] public float MaxVipTimeDelta { get; private set; }
        [field: SerializeField] public LocalizedString[] LocalizedVipDescriptions { get; private set; }
        [field: SerializeField] public string[] VipDescriptions { get; private set; }

        [field: Header("Complaints and Vip")]
        [field: SerializeField] public float CompletingOrderTimeMain { get; private set; }
        [field: SerializeField] public float CompletingOrderTimeForFlowerLvl { get; private set; }
        [field: SerializeField] public FlowerInfo InactiveOrder { get; private set; }

        [Header("Buying Flower")]
        [SerializeField] private AnimationCurve buyingFlowerCurve;
        [SerializeField] private float buyingFlowerSuccessBorder;
        
        public bool IsCustomerBuyingFlower()
        {
            float currentTime = Random.Range(0, 1f);
            return buyingFlowerCurve.Evaluate(currentTime) >= buyingFlowerSuccessBorder;
        }
    }
}