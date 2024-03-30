using PlayerControl;
using UniRx;
using UnityEngine;
using Zenject;

namespace FlowerShop.Effects
{
    public class SelectedTableEffect : MonoBehaviour
    {
        [Inject] private readonly EffectsSettings effectsSettings;
        [Inject] private readonly PlayerBusyness playerBusyness;
        
        public delegate void SelectedTableCheck();
        public event SelectedTableCheck SelectedTableCheckEvent;
        
        private float currentFillCoef;
        private float currentTimeToActiveEffect;
        private bool isEffectActive;
        private bool isValueIncrease = true;
        
        private readonly CompositeDisposable startEffectCompositeDisposable = new();
        private readonly int fillCoeff = Shader.PropertyToID("_FillCoeff");

        private void Start()
        {
            ActivateEffectWithDelay();
        }

        private void Update()
        {
            if (isEffectActive)
            {
                if (isValueIncrease && currentFillCoef >= 1)
                {
                    isValueIncrease = false;
                }
                else if (!isValueIncrease && currentFillCoef <= 0)
                {
                    isValueIncrease = true;
                }

                if (isValueIncrease)
                {
                    currentFillCoef += Time.deltaTime;
                }
                else
                {
                    currentFillCoef -= Time.deltaTime;
                }
            }
            else
            {
                if (currentFillCoef > 0)
                {
                    isValueIncrease = false;
                    currentFillCoef -= Time.deltaTime;
                }
            }
            
            effectsSettings.SelectableMaterial.SetFloat(fillCoeff, currentFillCoef);
        }

        public void ActivateEffectWithDelay()
        {
            ResetCurrentTimeToActiveEffect();
            
            Observable.EveryUpdate().Subscribe( updateCoffeeEffectTimer =>
            {
                currentTimeToActiveEffect += Time.deltaTime;

                if (currentTimeToActiveEffect >= effectsSettings.SelectedEffectDisplayingTimeDelay)
                {
                    if (!isEffectActive)
                    {
                        isEffectActive = true;
                        SelectedTableCheckEvent?.Invoke();
                    }
                    
                    ResetCurrentTimeToActiveEffect();
                    startEffectCompositeDisposable.Clear();
                }
            }).AddTo(startEffectCompositeDisposable);
        }

        public void ActivateEffectWithoutDelay()
        {
            isEffectActive = true;
            SelectedTableCheckEvent?.Invoke();
            ResetCurrentTimeToActiveEffect();
            startEffectCompositeDisposable.Clear();
        }
        
        public void ActivateEffectWithoutDelayAtFirst()
        {
            currentFillCoef = 0;
            isValueIncrease = true;
            
            ActivateEffectWithoutDelay();
        }

        public void TryToRecalculateEffect()
        {
            if (playerBusyness.IsPlayerFree &&
                isEffectActive && currentTimeToActiveEffect <= 0)
            {
                SelectedTableCheckEvent?.Invoke();
            }
        }

        public void DeactivateEffect()
        {
            isEffectActive = false;
            ResetCurrentTimeToActiveEffect();
            startEffectCompositeDisposable.Clear();
        }

        private void ResetCurrentTimeToActiveEffect()
        {
            currentTimeToActiveEffect = 0;
        }
    }
}
