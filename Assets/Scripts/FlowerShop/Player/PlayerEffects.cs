using UnityEngine;

namespace FlowerShop.Player
{
    public class PlayerEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem soilPartRight;
        [SerializeField] private ParticleSystem soilPartLeft;
        [SerializeField] private ParticleSystem wateringCanPS;
        [SerializeField] private ParticleSystem[] buildFirst;
        [SerializeField] private ParticleSystem[] buildSecond;

        public void PlaySoilPartRightEffects()
        {
            soilPartRight.Play();
        }

        public void PlaySoilPartLeftEffects()
        {
            soilPartLeft.Play();
        }

        public void PlayWateringCanPS()
        {
            wateringCanPS.Play();
        }
        
        public void StopWateringCanPS()
        {
            wateringCanPS.Stop();
        }

        public void PlayBuildFirstEffects()
        {
            foreach (ParticleSystem buildPS in buildFirst)
            {
                buildPS.Play();
            }
        }
        
        public void PlayBuildSecondEffects()
        {
            foreach (ParticleSystem buildPS in buildSecond)
            {
                buildPS.Play();
            }
        }
    }
}
