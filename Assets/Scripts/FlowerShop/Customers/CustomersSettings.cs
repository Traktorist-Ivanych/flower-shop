using UnityEngine;

namespace FlowerShop.Customers
{
    [CreateAssetMenu(fileName = "NewCustomersSettings", 
                     menuName = "Settings/Customers Settings", 
                     order = 6)]
    public class CustomersSettings : ScriptableObject
    {
        [field: Header("Moving")]
        [field: SerializeField] public float RemainingDistanceToStartRotation { get; private set; }
        [field: SerializeField] public float AngelToStopRotation { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public float NavAgentSpeed { get; private set; }
        
        [field: Header("Spawn")]
        [field: SerializeField] public float MinSpawnTime { get; private set; }
        [field: SerializeField] public float MinSpawnTimeDelta { get; private set; }
        [field: SerializeField] public float MaxSpawnTime { get; private set; }
        [field: SerializeField] public float MaxSpawnTimeDelta { get; private set; }
        
        [Header("Buying Flower")]
        [SerializeField] private AnimationCurve buyingFlowerCurve;
        [SerializeField] private float buyingFlowerSuccessBorder;
        
        public bool IsCustomerBuyingFlower()
        {
            return buyingFlowerCurve.Evaluate(Random.Range(0, 1f)) >= buyingFlowerSuccessBorder;
        }
    }
}