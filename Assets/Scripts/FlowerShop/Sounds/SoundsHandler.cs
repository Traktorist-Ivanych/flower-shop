using DG.Tweening;
using UnityEngine;
using Zenject;

namespace FlowerShop.Sounds
{
    public class SoundsHandler : MonoBehaviour
    {
        [Inject] private readonly SoundSettings soundSettings;

        [Header("AudioClips")]
        [SerializeField] private AudioClip uIButton;
        [SerializeField] private AudioClip computerTableIndicator;
        [SerializeField] private AudioClip openTrashCan;
        [SerializeField] private AudioClip closeTrashCan;
        [SerializeField] private AudioClip openDoor;
        [SerializeField] private AudioClip closeDoor;
        [SerializeField] private AudioClip upgradeFinish;
        [SerializeField] private AudioClip seedPlanted;
        [SerializeField] private AudioClip seedWatering;
        [SerializeField] private AudioClip weedPlanted;
        [SerializeField] private AudioClip fertilizerTreat;
        [SerializeField] private AudioClip addMoney;
        [SerializeField] private AudioClip takeMoney;
        [SerializeField] private AudioClip fillCoffeeCup;
        [SerializeField] private AudioClip automaticDoorOpenClose;

        [Header("AudioSources")]
        [SerializeField] private AudioSource uIAudioSource;
        [SerializeField] private AudioSource effectsSingleAudioSource;
        [SerializeField] private AudioSource wateringTablesAudioSource;
        [SerializeField] private AudioSource soilPreparationAudioSource;
        [SerializeField] private AudioSource crossingAudioSource;
        [SerializeField] private AudioSource growingTableFansAudioSource;
        [SerializeField] private AudioSource brokenTableAudioSource;
        [SerializeField] private AudioSource coffeeGrinderAudioSource;

        private int currentCrossingSoundsActive;
        private int currentGrowingTablesWithFansActive;
        private int currentBrokenTablesActive;

        public void PlayUIButtonAudio()
        {
            uIAudioSource.PlayOneShot(uIButton);
        }

        public void PlayComputerTableIndicator()
        {
            effectsSingleAudioSource.PlayOneShot(computerTableIndicator);
        }

        public void StartPlayingReplenishWateringCanAudio()
        {
            StartPlayAudioSource(wateringTablesAudioSource, soundSettings.MaxWateringTablesSoundVolume);
        }

        public void StopPlayingReplenishWateringCanAudio()
        {
            StopPlayAudioSource(wateringTablesAudioSource);
        }

        public void StartPlayingSoilPreparationAudio()
        {
            StartPlayAudioSource(soilPreparationAudioSource, soundSettings.MaxSoilPreparationSoundVolume);
        }
        
        public void StopPlayingSoilPreparationAudio()
        {
            StopPlayAudioSource(soilPreparationAudioSource);
        }
        
        public void StartPlayingCoffeeGrinderAudio()
        {
            StartPlayAudioSource(coffeeGrinderAudioSource, soundSettings.MaxCoffeeGrinderSoundVolume);
        }
        
        public void StopPlayingCoffeeGrinderAudio()
        {
            StopPlayAudioSource(coffeeGrinderAudioSource);
        }

        public void PlayFillCoffeeCupAudio()
        {
            effectsSingleAudioSource.PlayOneShot(fillCoffeeCup);
        }

        public void PlayOpenTrashCanAudio()
        {
            effectsSingleAudioSource.PlayOneShot(openTrashCan);
        }

        public void PlayCloseTrashCanAudio()
        {
            effectsSingleAudioSource.PlayOneShot(closeTrashCan);
        }

        public void PlayOpenDoorAudio()
        {
            effectsSingleAudioSource.PlayOneShot(openDoor);
        }
        
        public void PlayCloseDoorAudio()
        {
            effectsSingleAudioSource.PlayOneShot(closeDoor);
        }
        
        public void PlayOpenCloseAutomaticDoorAudio()
        {
            effectsSingleAudioSource.PlayOneShot(automaticDoorOpenClose);
        }

        public void PlayUpgradeFinishAudio()
        {
            effectsSingleAudioSource.PlayOneShot(upgradeFinish);
        }
        
        public void PlaySeedPlantedAudio()
        {
            effectsSingleAudioSource.PlayOneShot(seedPlanted);
        }
        
        public void PlaySeedWateringAudio()
        {
            effectsSingleAudioSource.PlayOneShot(seedWatering);
        }
        
        public void PlayWeedPlantedAudio()
        {
            effectsSingleAudioSource.PlayOneShot(weedPlanted);
        }

        public void PlayFertilizerTreatAudio()
        {
            effectsSingleAudioSource.PlayOneShot(fertilizerTreat);
        }

        public void PlayAddMoneyAudio()
        {
            effectsSingleAudioSource.PlayOneShot(addMoney);
        }
        
        public void PlayTakeMoneyAudio()
        {
            effectsSingleAudioSource.PlayOneShot(takeMoney);
        }

        public void StartPlayingCrossingSound()
        {
            if (currentCrossingSoundsActive == 0)
            {
                StartPlayAudioSource(crossingAudioSource, soundSettings.MaxCrossingSoundVolume);
            }
            currentCrossingSoundsActive++;
        }
        
        public void StopPlayingCrossingSound()
        {
            currentCrossingSoundsActive--;
            if (currentCrossingSoundsActive == 0)
            {
                StopPlayAudioSource(crossingAudioSource);
            }
        }

        public void StartPlayingGrowingTableFansAudio()
        {
            if (currentGrowingTablesWithFansActive == 0)
            {
                StartPlayAudioSource(growingTableFansAudioSource, soundSettings.MaxGrowingTableFansSoundVolume);
            }
            currentGrowingTablesWithFansActive++;
        }

        public void StopPlayingGrowingTableFansAudio()
        {
            currentGrowingTablesWithFansActive--;
            if (currentGrowingTablesWithFansActive == 0)
            {
                StopPlayAudioSource(growingTableFansAudioSource);
            }
        }

        public void StartPlayingBrokenTableAudio()
        {
            if (currentBrokenTablesActive == 0)
            {
                StartPlayAudioSource(brokenTableAudioSource, soundSettings.MaxBrokenTableSoundVolume);
            }
            currentBrokenTablesActive++;
        }
        
        public void StopPlayingBrokenTableAudio()
        {
            currentBrokenTablesActive--;
            if (currentBrokenTablesActive == 0)
            {
                StopPlayAudioSource(brokenTableAudioSource);
            }
        }

        private void StartPlayAudioSource(AudioSource targetAudioSource, float maxVolume)
        {
            targetAudioSource.volume = soundSettings.VolumeOnStartPlaying;
            targetAudioSource.Play();
            targetAudioSource.DOFade(endValue: maxVolume, duration: soundSettings.IncreasingVolumeTime);
        }

        private void StopPlayAudioSource(AudioSource targetAudioSource)
        {
            targetAudioSource.DOFade(
                    endValue: soundSettings.VolumeOnStartPlaying, 
                    duration: soundSettings.IncreasingVolumeTime).
                OnComplete(targetAudioSource.Stop); 
        }
    }
}
