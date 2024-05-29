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

        [field: Header("Passers")]
        [field: SerializeField] public float SpawnPasserDelay { get; private set; }
        [field: SerializeField] public float MinRemainingDistanceBetweenPathPoints { get; private set; }

        [field: Header("Cars")]
        [field: SerializeField] public float CarMovingTime { get; private set; }
        [field: SerializeField] public float CarSpawnMinTime { get; private set; }
        [field: SerializeField] public float CarSpawnMaxTime { get; private set; }
    }
}