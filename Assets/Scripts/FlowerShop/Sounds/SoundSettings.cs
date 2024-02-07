using UnityEngine;

namespace FlowerShop.Sounds
{
    [CreateAssetMenu(
        fileName = "SoundSettings", 
        menuName = "Settings/Sound Settings", 
        order = 14)]
    public class SoundSettings : ScriptableObject
    {
        [field: SerializeField] public float VolumeOnStartPlaying { get; private set; }
        [field: SerializeField] public float IncreasingVolumeTime { get; private set; }
        [field: SerializeField] public float DecreasingVolumeTime { get; private set; }
        
        [field: Header("VolumeSettings")]
        [field: SerializeField] public float MaxWateringTablesSoundVolume { get; private set; }
        [field: SerializeField] public float MaxSoilPreparationSoundVolume { get; private set; }
        [field: SerializeField] public float MaxCrossingSoundVolume { get; private set; }
        [field: SerializeField] public float MaxGrowingTableFansSoundVolume { get; private set; }
        [field: SerializeField] public float MaxBrokenTableSoundVolume { get; private set; }
    }
}