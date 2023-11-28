using UnityEngine;

namespace PlayerControl
{
    [CreateAssetMenu(fileName = "NewPlayerControlSettings", 
                     menuName = "Settings/Player Control", 
                     order = 3)]
    public class PlayerControlSettings : ScriptableObject
    {
        [field: Header("Ordinary Nav Agent Setting")]
        [field: SerializeField] public float PlayerNavAgentSpeed { get; private set; }
        [field: SerializeField] public float PlayerNavAgentAngularSpeed { get; private set; }
        [field: SerializeField] public float PlayerNavAgentAcceleration { get; private set; }
        [field: SerializeField] public float PlayerMovingRotation { get; private set; }
   
        [field: Header("Coffee Nav Agent Setting")]
        [field: SerializeField] public float PlayerNavAgentCoffeeSpeed { get; private set; }
        [field: SerializeField] public float PlayerNavAgentCoffeeAngularSpeed { get; private set; }
        [field: SerializeField] public float PlayerNavAgentCoffeeAcceleration { get; private set; }
        [field: SerializeField] public float PlayerMovingCoffeeRotation { get; private set; }
    }
}
