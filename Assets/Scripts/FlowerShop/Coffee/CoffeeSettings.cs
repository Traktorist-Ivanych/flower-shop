using UnityEngine;

namespace FlowerShop.Coffee
{
    [CreateAssetMenu(fileName = "NewCoffeeSettings", 
                     menuName = "Settings/Coffee Settings", 
                     order = 5)]
    public class CoffeeSettings : ScriptableObject
    {        
        [field: Tooltip("Time, it takes for CoffeeLiquid move from empty to full position or conversely. " +
                        "Important: depends on PlayerAnimation!")]
        [field: SerializeField] public float CoffeeLiquidMovingTime { get; private set; }
        
        [field: Tooltip("Time, it takes for CoffeeGrinder rotate before coffee machine starting to fill coffee cup.")]
        [field: SerializeField] public float CoffeeGrinderRotationDuration { get; private set; }
        
        [field: Tooltip("Time delay before Player start drinking coffee. " +
                        "Important: depends on PlayerAnimation!")]
        [field: SerializeField] public float StartDrinkingCoffeeTimeDelay { get; private set; }
    }
}