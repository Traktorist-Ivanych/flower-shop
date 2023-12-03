using UnityEngine;
using Zenject;

namespace PlayerControl
{
    [RequireComponent (typeof(PlayerMoving))]
    public class PlayerCoffeeEffect : MonoBehaviour
    {
        [Inject] private readonly PlayerControlSettings playerControlSettings;
        
        [HideInInspector, SerializeField] private PlayerMoving playerMoving;

        private float currentCoffeeEffectDuration;

        public bool IsCoffeeEffectActive { get; private set; }

        private void OnValidate()
        {
            playerMoving = GetComponent<PlayerMoving>();
        }

        private void Update()
        {
            if (IsCoffeeEffectActive)
            {
                currentCoffeeEffectDuration += Time.deltaTime;

                if (currentCoffeeEffectDuration >= playerControlSettings.CoffeeEffectDurationTime)
                {
                    currentCoffeeEffectDuration = 0;
                    FinishCoffeeEffect();
                }
            }
        }

        public void StartCoffeeEffect()
        {
            IsCoffeeEffectActive = true;
            playerMoving.SetCoffeeNavAgentSetting();
        }

        private void FinishCoffeeEffect()
        {
            IsCoffeeEffectActive = false;
            playerMoving.SetOrdinaryNavAgentSetting();
        }
    }
}
