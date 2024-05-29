using UnityEngine;

namespace FlowerShop.Achievements
{
    public class TopPlayerFlowerShop : Achievement
    {
        [SerializeField] private ParticleSystem[] sparklersPS;

        private void Start()
        {
            achievementMaxProgress = achievementsSettings.TopPlayerFlowerShopMaxProgress;
            UpdateScrollbar();

            if (isAwardReceived)
            {
                PlaySparklersPS();
            }
        }

        private protected override void OnEnable()
        {
            base.OnEnable();

            OnAwardReceivedEvent += PlaySparklersPS;
        }

        private protected override void OnDisable()
        {
            base.OnDisable();

            OnAwardReceivedEvent -= PlaySparklersPS;
        }

        private void PlaySparklersPS()
        {
            foreach (ParticleSystem sparklerPS in sparklersPS)
            {
                sparklerPS.Play();
            }
        }
    }
}