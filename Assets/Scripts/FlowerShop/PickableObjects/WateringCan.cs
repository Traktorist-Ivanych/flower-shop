using FlowerShop.Education;
using FlowerShop.Effects;
using FlowerShop.Flowers;
using FlowerShop.PickableObjects.Moving;
using FlowerShop.Saves.SaveData;
using FlowerShop.Tables;
using PlayerControl;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.PickableObjects
{
    [RequireComponent(typeof(ObjectMoving))]
    public class WateringCan : MonoBehaviour, IPickableObject, ISavableObject
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly PlayerPickableObjectHandler playerPickableObjectHandler;
        [Inject] private readonly TablesSettings tablesSettings;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly SelectedTableEffect selectedTableEffect;
    
        [SerializeField] private Transform wateringCanIndicatorTransform;
        [SerializeField] private Mesh[] wateringCanLvlMeshes = new Mesh[2];

        [HideInInspector, SerializeField] private ObjectMoving objectMoving;
        [HideInInspector, SerializeField] private MeshFilter wateringCanMeshFilter;
        [HideInInspector, SerializeField] private MeshRenderer wateringCanIndicatorMeshRenderer;
    
        private int maxWateringsNumber;
        private int wateringCanLvl;

        [field: SerializeField] public GrowingRoom GrowingRoom { get; private set; }
        [field: SerializeField] public string UniqueKey { get; private set; }
        
        public int CurrentWateringsNumber { get; private set; }

        private void OnValidate()
        {
            objectMoving = GetComponent<ObjectMoving>();
            wateringCanMeshFilter = GetComponent<MeshFilter>();
            wateringCanIndicatorMeshRenderer = wateringCanIndicatorTransform.GetComponent<MeshRenderer>();
        }

        private void Awake()
        {
            Load();
        }

        public void TakeInPlayerHandsAndSetPlayerFree()
        {
            playerPickableObjectHandler.CurrentPickableObject = this;
            wateringCanIndicatorMeshRenderer.enabled = true;
            UpdateWateringCanIndicator();
            selectedTableEffect.ActivateEffectWithDelay();
            
            objectMoving.MoveObject(
                targetFinishTransform: playerComponents.PlayerHandsForBigObjectTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.TakeBigObjectTrigger, 
                setPlayerFree: true);
        }

        public void PutOnTableAndSetPlayerFree(Transform targetTransform) 
        {
            selectedTableEffect.ActivateEffectWithDelay();
            wateringCanIndicatorMeshRenderer.enabled = false;
            
            objectMoving.MoveObject(
                targetFinishTransform: targetTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.GiveBigObjectTrigger, 
                setPlayerFree: true);
        }

        public void PourPotWithWateringCan()
        {
            CurrentWateringsNumber--;
            UpdateWateringCanIndicator();
            selectedTableEffect.ActivateEffectWithDelay();

            if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
            {
                educationHandler.CompleteEducationStep();
            }
            
            Save();
        }

        public bool IsWateringCanNeedForReplenish()
        {
            return CurrentWateringsNumber < maxWateringsNumber;
        }

        public float ReplenishWateringCanTime()
        {
            return tablesSettings.TimeForReplenishWateringCanWithOneWatering * (maxWateringsNumber - CurrentWateringsNumber);
        }

        public void ReplenishWateringCan()
        {
            SetCurrentToMaxWateringsNumber();
            
            Save();
        }

        public void UpgradeWateringCan(int tableLvl)
        {
            SetWateringCanLvl(tableLvl);
            SetCurrentToMaxWateringsNumber();
            
            Save();
        }

        private void SetWateringCanLvl(int nextWateringCanLvl)
        {
            wateringCanLvl = nextWateringCanLvl;
            CalculateMaxWateringsNumber();
            if (wateringCanLvl > 0)
            {
                wateringCanMeshFilter.mesh = wateringCanLvlMeshes[wateringCanLvl - 1];
            }
        }

        public void LoadInPlayerHands()
        {
            objectMoving.SetParentAndParentPositionAndRotation(playerComponents.PlayerHandsForBigObjectTransform);
            playerPickableObjectHandler.CurrentPickableObject = this;
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.LoadToHold);
            wateringCanIndicatorMeshRenderer.enabled = true;
            UpdateWateringCanIndicator();
            selectedTableEffect.ActivateEffectWithDelay();
        }

        public void Load()
        {
            WateringCanForSaving wateringCanForLoading = SavesHandler.Load<WateringCanForSaving>(UniqueKey);
            
            if (wateringCanForLoading.IsValuesSaved)
            {
                CurrentWateringsNumber = wateringCanForLoading.CurrentWateringsNumber;
                SetWateringCanLvl(wateringCanForLoading.WateringCanLvl);
            }
            else
            {
                CalculateMaxWateringsNumber();
                SetCurrentToMaxWateringsNumber();
            }
        }

        public void Save()
        {
            WateringCanForSaving wateringCanForSaving = new(CurrentWateringsNumber, wateringCanLvl);
            
            SavesHandler.Save(UniqueKey, wateringCanForSaving);
        }
        private void SetCurrentToMaxWateringsNumber()
        {
            CurrentWateringsNumber = maxWateringsNumber;
        }

        private void CalculateMaxWateringsNumber()
        {
            maxWateringsNumber = tablesSettings.WateringsNumber + tablesSettings.WateringsNumberLvlDelta * wateringCanLvl;
        }

        private void UpdateWateringCanIndicator()
        {
            float indicatorScaleZ = (float)CurrentWateringsNumber / maxWateringsNumber;
            if (indicatorScaleZ <= 0.025f)
            {
                wateringCanIndicatorMeshRenderer.enabled = false;
            }
            else
            {
                wateringCanIndicatorTransform.localScale = new Vector3(1, 1, indicatorScaleZ);
            }
        }
    }
}
