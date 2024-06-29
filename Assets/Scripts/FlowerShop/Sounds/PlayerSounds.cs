using DG.Tweening;
using UnityEngine;
using Zenject;

namespace FlowerShop.Sounds
{
    public class PlayerSounds : MonoBehaviour
    {
        [Inject] private readonly SoundSettings soundSettings;
        
        [Header("AudioClips")]
        [SerializeField] private AudioClip[] playerSteps;
        [SerializeField] private AudioClip[] weeding;
        [SerializeField] private AudioClip[] hammerHits;
        [SerializeField] private AudioClip takeInPlayerHands;
        [SerializeField] private AudioClip giveFromPlayerHands;
        [SerializeField] private AudioClip watering;
        [SerializeField] private AudioClip startCoffeeMachine;
        [SerializeField] private AudioClip drinkCoffee;

        [Header("AudioSources")] 
        [SerializeField] private AudioSource playerStepsAudioSource;
        [SerializeField] private AudioSource effectsSingleAudioSource;
        [SerializeField] private AudioSource effectsLoopAudioSource;

        public void PlayStepAudio()
        {
            int currentStepAudio = Random.Range(0, playerSteps.Length);
            playerStepsAudioSource.PlayOneShot(playerSteps[currentStepAudio]);
        }

        public void PlayWeedingSound()
        {
            int currentStepAudio = Random.Range(0, weeding.Length);
            effectsSingleAudioSource.PlayOneShot(weeding[currentStepAudio]);
        }

        public void PlayHammerHitAudio()
        {
            int currentHammerHitAudio = Random.Range(0, hammerHits.Length);
            effectsSingleAudioSource.PlayOneShot(hammerHits[currentHammerHitAudio]);
        }

        public void PlayTakeInPlayerHandsSound()
        {
            effectsSingleAudioSource.PlayOneShot(takeInPlayerHands);
        }
        
        public void PlayGiveFromPlayerHandsSound()
        {
            effectsSingleAudioSource.PlayOneShot(giveFromPlayerHands);
        }

        public void PlayStartCoffeeMachineAudio()
        {
            effectsSingleAudioSource.PlayOneShot(startCoffeeMachine);
        }
        
        public void PlayDrinkCoffeeAudio()
        {
            effectsSingleAudioSource.PlayOneShot(drinkCoffee);
        }

        public void StartPlayingWateringSound()
        {
            effectsLoopAudioSource.clip = watering;
            effectsLoopAudioSource.volume = soundSettings.VolumeOnStartPlaying;
            effectsLoopAudioSource.Play();
            effectsLoopAudioSource.DOFade(endValue: 1, duration: soundSettings.IncreasingVolumeTime);
        }

        public void StopPlayingWateringSound()
        {
            effectsLoopAudioSource.DOFade(
                    endValue: soundSettings.VolumeOnStartPlaying, 
                    duration: soundSettings.DecreasingVolumeTime).
                OnComplete(effectsLoopAudioSource.Stop);
        }
    }
}
