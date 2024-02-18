using UnityEngine;

namespace FlowerShop.Environment
{
    [CreateAssetMenu(fileName = "EnvironmentSettings", 
        menuName = "Settings/Environment Settings", 
        order = 15)]
    public class EnvironmentSettings : ScriptableObject
    {
        [field: Header("Doors")]
        [field: SerializeField] public float ClosedDoorAngle { get; private set; }
        [field: SerializeField] public float MinDistanceToOpenAutomaticDoors { get; private set; }
    }
}