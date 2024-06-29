using UnityEngine;

namespace FlowerShop.Sounds
{
    public class CustomersSounds : MonoBehaviour
    {
        [SerializeField] private AudioClip[] customerSteps;
        [SerializeField] private AudioClip takeInHands;
        [SerializeField] private AudioClip hmmAudioClip;
        [SerializeField] private AudioClip yesAudioClip;
        [SerializeField] private AudioClip noAudioClip;

        [SerializeField] private AudioSource customerStepsAudioSource;
        [SerializeField] private AudioSource customerSingleAudioSource;

        public void PlayCustomerStepAudio()
        {
            int currentStepAudio = Random.Range(0, customerSteps.Length);
            customerStepsAudioSource.PlayOneShot(customerSteps[currentStepAudio]);
        }

        public void PlayTakeInHandsAudio()
        {
            customerSingleAudioSource.PlayOneShot(takeInHands);
        }

        public void PlayHmmAudio()
        {
            customerSingleAudioSource.PlayOneShot(hmmAudioClip);
        }
        
        public void PlayYesAudio()
        {
            customerSingleAudioSource.PlayOneShot(yesAudioClip);
        }
        
        public void PlayNoAudio()
        {
            customerSingleAudioSource.PlayOneShot(noAudioClip);
        }
    }
}
