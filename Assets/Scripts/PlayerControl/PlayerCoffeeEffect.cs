using UniRx;
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
        
        private readonly CompositeDisposable coffeeTimerCompositeDisposable = new();
        
        public bool IsCoffeeEffectActive { get; private set; }

        private void OnValidate()
        {
            playerMoving = GetComponent<PlayerMoving>();
        }

        public void StartCoffeeEffect()
        {
            IsCoffeeEffectActive = true;
            playerMoving.SetCoffeeNavAgentSetting();

            Observable.EveryUpdate().Subscribe( updateCoffeeEffectTimer =>
            {
                currentCoffeeEffectDuration += Time.deltaTime;
                
                if (currentCoffeeEffectDuration >= playerControlSettings.CoffeeEffectDurationTime)
                {
                    currentCoffeeEffectDuration = 0;
                    FinishCoffeeEffect();
                    coffeeTimerCompositeDisposable.Clear();
                }
                
            }).AddTo(coffeeTimerCompositeDisposable);
        }

        private void FinishCoffeeEffect()
        {
            IsCoffeeEffectActive = false;
            playerMoving.SetOrdinaryNavAgentSetting();
        }
    }
}
