using System.Collections;
using FlowerShop.Achievements;
using FlowerShop.Education;
using FlowerShop.Effects;
using FlowerShop.PickableObjects;
using FlowerShop.PickableObjects.Moving;
using FlowerShop.Saves.SaveData;
using FlowerShop.Sounds;
using PlayerControl;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Fertilizers
{
    [RequireComponent(typeof(ObjectMoving))]
    public abstract class Fertilizer : MonoBehaviour, IPickableObject, ISavableObject
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly FertilizersCanvasLiaison fertilizersCanvasLiaison;
        [Inject] private readonly FertilizersSetting fertilizersSetting;
        [Inject] private readonly PlayerBusyness playerBusyness;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly PlayerPickableObjectHandler playerPickableObjectHandler;
        [Inject] private readonly SelectedTableEffect selectedTableEffect;
        [Inject] private readonly SoundsHandler soundsHandler;
        [Inject] private readonly ThereIsNoTimeToWait thereIsNoTimeToWait;

        [SerializeField] private ParticleSystem treatEffect;
        
        [HideInInspector, SerializeField] private ObjectMoving objectMoving;

        private protected Pot potForTreating;
        
        [field: SerializeField] public string UniqueKey { get; private set; }
        
        public int AvailableUsesNumber { get; private set; }
        public bool IsFertilizerInPlayerHands { get; private set; }

        private void Awake()
        {
            Load();
        }
        
        private void OnValidate()
        {
            objectMoving = GetComponent<ObjectMoving>();
        }

        private void Start()
        {
            fertilizersCanvasLiaison.UpdateFertilizersAvailableUsesNumber();
        }

        public IEnumerator PotTreating(Pot transmittedPotForTreating)
        {
            potForTreating = transmittedPotForTreating;
            treatEffect.Play();
            
            yield return new WaitForSeconds(fertilizersSetting.FertilizerTreatingTime);

            FinishTreatingPot();
        }

        public void TakeInPlayerHandsAndSetPlayerFree()
        {
            IsFertilizerInPlayerHands = true;
            playerPickableObjectHandler.CurrentPickableObject = this;
            selectedTableEffect.ActivateEffectWithDelay();
            objectMoving.MoveObject(
                targetFinishTransform: playerComponents.PlayerHandsForCoffeeTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.TakeLittleObjectTrigger, 
                setPlayerFree: true);
        }

        public void PutOnTableAndSetPlayerFree(Transform targetTransform)
        {
            IsFertilizerInPlayerHands = false;
            playerPickableObjectHandler.ResetPickableObject();
            selectedTableEffect.ActivateEffectWithDelay();
            objectMoving.MoveObject(
                targetFinishTransform: targetTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.GiveLittleObjectTrigger, 
                setPlayerFree: true);
        }

        public void IncreaseAvailableUsesNumber(int quantity)
        {
            AvailableUsesNumber += quantity;
            fertilizersCanvasLiaison.UpdateFertilizersAvailableUsesNumber();
            
            Save();
        }

        public void LoadInPlayerHands()
        {
            IsFertilizerInPlayerHands = true;
            objectMoving.SetParentAndParentPositionAndRotation(playerComponents.PlayerHandsForCoffeeTransform);
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.LoadToHoldLittleObject);
            selectedTableEffect.ActivateEffectWithDelay();
        }

        public void Load()
        {
            FertilizerForSaving fertilizerForLoading = SavesHandler.Load<FertilizerForSaving>(UniqueKey);

            if (fertilizerForLoading.IsValuesSaved)
            {
                AvailableUsesNumber = fertilizerForLoading.AvailableUsesNumber;
            }
            else
            {
                AvailableUsesNumber = fertilizersSetting.FertilizersStartUsesNumber;
            }
        }

        public void Save()
        {
            FertilizerForSaving fertilizerForSaving = new(AvailableUsesNumber);
            
            SavesHandler.Save(UniqueKey, fertilizerForSaving);
        }

        private protected virtual void FinishTreatingPot()
        {
            AvailableUsesNumber--;
            fertilizersCanvasLiaison.UpdateFertilizersAvailableUsesNumber();
            playerBusyness.SetPlayerFree();
            soundsHandler.PlayFertilizerTreatAudio();
            
            if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
            {
                educationHandler.CompleteEducationStep();
            }
            
            thereIsNoTimeToWait.IncreaseProgress();
            
            Save();
        }
    }
}
