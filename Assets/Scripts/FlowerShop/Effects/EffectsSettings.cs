using UnityEngine;

namespace FlowerShop.Effects
{
    [CreateAssetMenu(fileName = "EffectsSettings", 
        menuName = "Settings/Effects Settings", 
        order = 16)]
    public class EffectsSettings : ScriptableObject
    {
        [field: Header("Time")]
        [field: SerializeField] public float SelectedEffectDisplayingTimeDelay { get; private set; }
        [field: SerializeField] public float ClickableEffectDuration { get; private set; }
        
        [field: Header("Materials")]
        [field: SerializeField] public Material EnvironmentMaterial { get; private set; }
        [field: SerializeField] public Material SelectableMaterial { get; private set; }
        [field: SerializeField] public Material SuccessMaterial { get; private set; }
        [field: SerializeField] public Material FailMaterial { get; private set; }
    }
}