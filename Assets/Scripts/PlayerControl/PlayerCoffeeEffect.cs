using PlayerControl.SaveData;
using Saves;
using UniRx;
using UnityEngine;
using Zenject;

namespace PlayerControl
{
    [RequireComponent (typeof(PlayerMoving))]
    public class PlayerCoffeeEffect : MonoBehaviour, ISavableObject
    {
        [Inject] private readonly CyclicalSaver cyclicalSaver;
        [Inject] private readonly PlayerControlSettings playerControlSettings;
        [Inject] private readonly PlayerStatsCanvasLiaison playerStatsCanvasLiaison;
        
        [HideInInspector, SerializeField] private PlayerMoving playerMoving;

        private float currentCoffeeEffectDuration;
        private float currentCoffeeEffectIndicatorFillAmount;
        
        private readonly CompositeDisposable coffeeTimerCompositeDisposable = new();
        
        [field: SerializeField] public string UniqueKey { get; private set; }
        
        public bool IsCoffeeEffectActive { get; private set; }

        private void OnValidate()
        {
            playerMoving = GetComponent<PlayerMoving>();
        }

        private void Awake()
        {
            Load();
        }

        public void StartCoffeeEffect()
        {
            EnableCoffeeEffect();
            
            Save();
        }

        public void Load()
        {
            PlayerCoffeeEffectForSaving playerCoffeeEffectForLoading =
                SavesHandler.Load<PlayerCoffeeEffectForSaving>(UniqueKey);

            if (playerCoffeeEffectForLoading.IsValuesSaved)
            {
                currentCoffeeEffectDuration = playerCoffeeEffectForLoading.CurrentCoffeeEffectDuration;
                EnableCoffeeEffect();
            }
            else
            {
                playerMoving.SetOrdinaryNavAgentSetting();
            }
        }

        public void Save()
        {
            PlayerCoffeeEffectForSaving playerCoffeeEffectForSaving = new(currentCoffeeEffectDuration);
            
            SavesHandler.Save(UniqueKey, playerCoffeeEffectForSaving);
        }

        private void EnableCoffeeEffect()
        {
            IsCoffeeEffectActive = true;
            playerStatsCanvasLiaison.SetCoffeeEffectPanelActive(true);
            playerMoving.SetCoffeeNavAgentSetting();
            cyclicalSaver.CyclicalSaverEvent += Save;

            Observable.EveryUpdate().Subscribe( updateCoffeeEffectTimer =>
            {
                currentCoffeeEffectDuration += Time.deltaTime;
                UpdateCoffeeEffectIndicatorFillAmount();
                
                if (currentCoffeeEffectDuration >= playerControlSettings.CoffeeEffectDurationTime)
                {
                    FinishCoffeeEffect();
                    coffeeTimerCompositeDisposable.Clear();
                }
                
            }).AddTo(coffeeTimerCompositeDisposable);
        }

        private void UpdateCoffeeEffectIndicatorFillAmount()
        {
            currentCoffeeEffectIndicatorFillAmount =
                1 - currentCoffeeEffectDuration / playerControlSettings.CoffeeEffectDurationTime;
                
            playerStatsCanvasLiaison.UpdateCoffeeEffectIndicatorFillAmount(currentCoffeeEffectIndicatorFillAmount);
        }
        
        private void FinishCoffeeEffect()
        {
            IsCoffeeEffectActive = false;
            playerStatsCanvasLiaison.SetCoffeeEffectPanelActive(false);
            currentCoffeeEffectDuration = 0;
            playerMoving.SetOrdinaryNavAgentSetting();
            cyclicalSaver.CyclicalSaverEvent -= Save;
            SavesHandler.DeletePlayerPrefsKey(UniqueKey);
        }
    }
}
