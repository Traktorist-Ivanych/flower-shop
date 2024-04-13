using UnityEngine;

namespace FlowerShop.Fertilizers
{
    [CreateAssetMenu(
        fileName = "FertilizersSetting", 
        menuName = "Settings/Fertilizers Setting", 
        order = 12)]
    public class FertilizersSetting : ScriptableObject
    {
        [field: SerializeField] public float GrothAcceleratorCoeff { get; private set; }
        [field: SerializeField] public float PrimaryGrothAcceleratorCoeff { get; private set; }
        [field: SerializeField] public float FertilizerTreatingTime { get; private set; }
        [field: SerializeField] public int FertilizersStartUsesNumber { get; private set; }
        [field: SerializeField] public int IncreaseFertilizerAmountIAP { get; private set; }
        [field: SerializeField] public int IncreaseFertilizerAmount { get; private set; }
    }
}