using UnityEngine;

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
        [field: SerializeField] public float ComplaintsHandleTime { get; private set; }
        [field: SerializeField] public string[] ComplaintDescriptions { get; private set; }
        
        [field: Header("Vip")]
        [field: SerializeField] public int FlowerSellingPriceMultiplier { get; private set; }
        [field: SerializeField] public int MinFlowersInPlayerCollectionCount { get; private set; }
        [field: SerializeField] public float MinVipTime { get; private set; }
        [field: SerializeField] public float MinVipTimeDelta { get; private set; }
        [field: SerializeField] public float MaxVipTime { get; private set; }
        [field: SerializeField] public float MaxVipTimeDelta { get; private set; }
        [field: SerializeField] public float VipHandleTime { get; private set; }
        [field: SerializeField] public string[] VipDescriptions { get; private set; }
        
        [Header("Buying Flower")]
        [SerializeField] private AnimationCurve buyingFlowerCurve;
        [SerializeField] private float buyingFlowerSuccessBorder;
        
        public bool IsCustomerBuyingFlower()
        {
            return buyingFlowerCurve.Evaluate(Random.Range(0, 1f)) >= buyingFlowerSuccessBorder;
        }
    }
}