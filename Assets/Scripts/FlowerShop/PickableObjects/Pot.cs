using FlowerShop.Education;
using FlowerShop.Effects;
using FlowerShop.Fertilizers;
using FlowerShop.Flowers;
using FlowerShop.PickableObjects.Helpers;
using FlowerShop.PickableObjects.Moving;
using FlowerShop.Saves.SaveData;
using FlowerShop.Sounds;
using FlowerShop.Tables;
using FlowerShop.Weeds;
using PlayerControl;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.PickableObjects
{
    [RequireComponent(typeof(PotObjects))]
    [RequireComponent(typeof(ObjectMoving))]
    public class Pot : MonoBehaviour, IPickableObject, ISavableObject
    {
        [Inject] private readonly CyclicalSaver cyclicalSaver;
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly FertilizersSetting fertilizersSetting;
        [Inject] private readonly FertilizersTable fertilizersTable;
        [Inject] private readonly FlowersContainer flowersContainer;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly PlayerPickableObjectHandler playerPickableObjectHandler;
        [Inject] private readonly ReferencesForLoad referencesForLoad;
        [Inject] private readonly SelectedTableEffect selectedTableEffect;
        [Inject] private readonly SoundsHandler soundsHandler;
        [Inject] private readonly TablesSettings tablesSettings;
        [Inject] private readonly WeedSettings weedSettings;

        [SerializeField] private PotsRack potsRack;
        [SerializeField] private WateringTable wateringTable;
        [SerializeField] private WeedingTable weedingTable;
        
        [HideInInspector, SerializeField] private ObjectMoving objectMoving;

        private float currentGrothAcceleratorCoeff;
        private float upGrowingLvlTime;
        private float currentUpGrowingLvlTime;
        private float growingLvlTimeProgress;
        private int weedGrowingLvl;
        private bool isPotOnGrowingTable;

        [field: SerializeField] public string UniqueKey { get; private set; }
        [field: SerializeField] public GrowingRoom GrowingRoom { get; private set; }
        [field: HideInInspector, SerializeField] public PotObjects PotObjects { get; private set; }

        public FlowerInfo PlantedFlowerInfo { get; private set; }
        public int FlowerGrowingLvl { get; private set; }
        public bool IsPotTreatedByGrothAccelerator { get; private set; }
        public bool IsSoilInsidePot { get; private set; }
        public bool IsFlowerNeedWater { get; private set; }
        public bool IsWeedInPot { get; private set; }

        private void OnValidate()
        {
            objectMoving = GetComponent<ObjectMoving>();
            PotObjects = GetComponent<PotObjects>();
        }
        
        private void Awake()
        {
            Load();
        }

        private void Start()
        {
            if (upGrowingLvlTime == 0)
            {
                upGrowingLvlTime = tablesSettings.UpGrowingLvlTime;
            }
        }

        private void Update()
        {
            if (ShouldWeedGrowingLvlIncrease())
            {
                if (weedGrowingLvl < weedSettings.MaxWeedGrowingLvl)
                {
                    weedGrowingLvl++;
                    PotObjects.SetWeedLvlMesh(weedGrowingLvl);
                }
                ResetGrowingLvlTime();
                PotObjects.PlayWeedEffects();
                soundsHandler.PlayWeedPlantedAudio();
            }
            else if (ShouldWaterIndicatorBeDisplayed())
            {
                ResetGrowingLvlTime();
                ShowWaterIndicator();
                selectedTableEffect.TryToRecalculateEffect();
            }
        }

        public void FillPotWithSoil()
        {
            ShowSoil();
            
            Save();
        }

        public void PlantSeed(FlowerInfo flowerInfoForPlanting)
        {
            ResetFlowerGrowingLvl();
            ResetGrowingLvlTime();
            
            soundsHandler.PlaySeedPlantedAudio();
            ShowSeed(flowerInfoForPlanting);
            PotObjects.PlaySeedPlantedEffects();
            selectedTableEffect.TryToRecalculateEffect();
            
            Save();
        }

        public void PourFlower()
        {
            PourFlowerBase();
            UpFlowerGrowingLvl();
            PotObjects.PlaySeedGrowingEffects();
            soundsHandler.PlaySeedWateringAudio();
            
            ((WateringCan)playerPickableObjectHandler.CurrentPickableObject).PourPotWithWateringCan();
            
            Save();
        }

        public void PlantWeed()
        {
            ResetWeedGrowingLvl();
            ResetGrowingLvlTime();
            
            ShowWeed();
            PotObjects.PlayWeedEffects();
            soundsHandler.PlayWeedPlantedAudio();
            
            weedingTable.IncreaseFlowersThatNeedWeedingQuantity();
            selectedTableEffect.TryToRecalculateEffect();
            
            Save();
        }

        public void DeleteWeed()
        {
            weedingTable.DecreaseFlowersThatNeedWeedingQuantity();
            IsWeedInPot = false;
            ResetWeedGrowingLvl();
            ResetGrowingLvlTime();
            PotObjects.HideWeed();
            
            Save();
        }

        public void TreatPotByGrothAccelerator()
        {
            SetTreatedGrothAcceleratorCoeff();
            PotObjects.PlaySeedGrowingEffects();
            
            Save();
        }

        public void TreatPotByGrowingLvlIncreaser()
        {
            UpFlowerGrowingLvl();
            if (FlowerGrowingLvl >= flowersSettings.MaxFlowerGrowingLvl)
            {
                PourFlowerBase();
            }
            PotObjects.PlaySeedGrowingEffects();
            
            Save();
        }

        public void TreatPotByGrowerToMaxLvl()
        {
            PourFlowerBase();
            PotObjects.PlaySeedGrowingEffects();
            
            FlowerGrowingLvl = flowersSettings.MaxFlowerGrowingLvl;
            PotObjects.SetFlowerLvlMesh(PlantedFlowerInfo, FlowerGrowingLvl);
            
            Save();
        }

        public void CleanPot()
        {
            if (IsFlowerNeedWater)
            {
                wateringTable.DecreaseFlowersThatNeedWaterQuantity();
            }
            if (IsWeedInPot)
            {
                weedingTable.DecreaseFlowersThatNeedWeedingQuantity();
            }
            IsSoilInsidePot = false;
            IsFlowerNeedWater = false;
            IsWeedInPot = false;
            ResetPlantedFlowerInfo();
            ResetWeedGrowingLvl();
            ResetFlowerGrowingLvl();
            ResetGrowingLvlTime();
            ResetGrothAcceleratorParameters();
            PotObjects.HideAllPotObjects();
            selectedTableEffect.ActivateEffectWithDelay();
            fertilizersTable.RemoveActivePot(this);

            if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
            {
                educationHandler.CompleteEducationStep();
            }
            
            SavesHandler.DeletePlayerPrefsKey(UniqueKey);
        }

        public void PutOnGrowingTableAndSetPlayerFree(Transform targetTransform, int growingTableLvl)
        {
            PutOnGrowingTableBaseActions(growingTableLvl);
            CalculateCurrentUpGrowingLvlTimeWithGrowingLvlTimeProgress();
            PutOnTableAndSetPlayerFree(targetTransform);
            
            Save();
        }
    
        public void PutOnTableAndSetPlayerFree(Transform targetTransform)
        {
            selectedTableEffect.ActivateEffectWithDelay();
            
            objectMoving.MoveObject(
                targetFinishTransform: targetTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.GiveBigObjectTrigger, 
                setPlayerFree: true); 
        }

        public void PutOnTableAndKeepPlayerBusy(Transform targetTransform)
        {
            objectMoving.MoveObject(
                targetFinishTransform: targetTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.GiveBigObjectTrigger, 
                setPlayerFree: false);
        }

        public void TakeInPlayerHandsFromGrowingTableAndSetPlayerFree()
        {
            cyclicalSaver.CyclicalSaverEvent -= Save;
            isPotOnGrowingTable = false;
            CalculateGrowingLvlTimeProgress();
            TakeInPlayerHandsAndSetPlayerFree();
            
            Save();
        }

        public void TakeInPlayerHandsAndSetPlayerFree()
        {
            playerPickableObjectHandler.CurrentPickableObject = this;
            selectedTableEffect.ActivateEffectWithDelay();
            
            objectMoving.MoveObject(
                targetFinishTransform: playerComponents.PlayerHandsForBigObjectTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.TakeBigObjectTrigger, 
                setPlayerFree: true);
        }

        public void CrossFlower()
        {
            --FlowerGrowingLvl;
            PotObjects.SetFlowerLvlMesh(PlantedFlowerInfo, FlowerGrowingLvl);
            
            Save();
        }

        public void Save()
        {
            PotForSaving potForSaving = new PotForSaving(IsSoilInsidePot, PlantedFlowerInfo.UniqueKey, FlowerGrowingLvl,
                currentUpGrowingLvlTime, IsFlowerNeedWater, IsPotTreatedByGrothAccelerator, IsWeedInPot, weedGrowingLvl);
            
            SavesHandler.Save(UniqueKey ,potForSaving);
        }

        public void Load()
        {
            PotForSaving potForLoading = SavesHandler.Load<PotForSaving>(UniqueKey);

            if (potForLoading.IsValuesSaved)
            {
                FlowerGrowingLvl = potForLoading.FlowerGrowingLvl;
                currentUpGrowingLvlTime = potForLoading.CurrentUpGrowingLvlTime;
                IsPotTreatedByGrothAccelerator = potForLoading.IsPotTreatedByGrothAccelerator;
                weedGrowingLvl = potForLoading.WeedGrowingLvl;
                
                if (potForLoading.IsSoilInsidePot)
                {
                    ShowSoil();
                }

                PlantedFlowerInfo =
                    referencesForLoad.GetReference<FlowerInfo>(potForLoading.PlantedFlowerInfoUniqueKey);

                if (PlantedFlowerInfo != flowersSettings.FlowerInfoEmpty)
                {
                    ShowSeed(PlantedFlowerInfo);
                }

                if (potForLoading.IsFlowerNeedWater)
                {
                    ShowWaterIndicator();
                }
                
                if (potForLoading.IsWeedInPot)
                {
                    ShowWeed();
                }

                if (potForLoading.IsPotTreatedByGrothAccelerator)
                {
                    SetTreatedGrothAcceleratorCoeff();
                }
                else
                {
                    ResetGrothAcceleratorParameters();
                }
            }
            else
            {
                ResetPlantedFlowerInfo();
                ResetGrothAcceleratorParameters(); 
            }
        }

        public void LoadInPlayerHands()
        {
            objectMoving.SetParentAndParentPositionAndRotation(playerComponents.PlayerHandsForBigObjectTransform);
            potsRack.RemovePotFromListOnLoad(this);
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.LoadToHold);
            selectedTableEffect.ActivateEffectWithDelay();
        }

        public void LoadOnGrowingTable(Transform transformOnTable, int growingTableLvl)
        {
            PutOnGrowingTableBaseActions(growingTableLvl);
            LoadOnTable(transformOnTable);
        }

        public void LoadOnTable(Transform transformOnTable)
        {
            objectMoving.SetParentAndParentPositionAndRotation(transformOnTable);
            potsRack.RemovePotFromListOnLoad(this);
        }

        public void HideWaterIndicator()
        {
            PotObjects.HideWaterIndicator();
        }

        public void CalculateUpGrowingLvlTimeOnTableUpgrade(int growingTableLvl)
        {
            CalculateGrowingLvlTimeProgress();
            CalculateUpGrowingLvlTime(growingTableLvl);
            CalculateCurrentUpGrowingLvlTimeWithGrowingLvlTimeProgress();
        }

        public bool CanPotBeTreated()
        {
            return FlowerGrowingLvl < flowersSettings.MaxFlowerGrowingLvl && 
                   !IsPotTreatedByGrothAccelerator;
        }

        private void PutOnGrowingTableBaseActions(int growingTableLvl)
        {
            cyclicalSaver.CyclicalSaverEvent += Save;
            isPotOnGrowingTable = true;
            CalculateUpGrowingLvlTime(growingTableLvl);
        }

        private void CalculateUpGrowingLvlTime(int growingTableLvl)
        {
            upGrowingLvlTime = tablesSettings.UpGrowingLvlTime - tablesSettings.UpGrowingLvlTableLvlTimeDelta * growingTableLvl;
        }

        private void CalculateGrowingLvlTimeProgress()
        {
            growingLvlTimeProgress = currentUpGrowingLvlTime / upGrowingLvlTime;
        }

        private void CalculateCurrentUpGrowingLvlTimeWithGrowingLvlTimeProgress()
        {
            currentUpGrowingLvlTime = upGrowingLvlTime * growingLvlTimeProgress;
        }

        private bool ShouldWeedGrowingLvlIncrease()
        {
            return IsWeedInPot && ShouldGrowingLvlIncrease();
        }

        private bool ShouldWaterIndicatorBeDisplayed()
        {
            return !IsWeedInPot &&
                   isPotOnGrowingTable && 
                   !IsFlowerNeedWater && 
                   FlowerGrowingLvl < flowersSettings.MaxFlowerGrowingLvl && 
                   ShouldGrowingLvlIncrease();
        }

        private bool ShouldGrowingLvlIncrease()
        {
            currentUpGrowingLvlTime += Time.deltaTime * currentGrothAcceleratorCoeff;
            return currentUpGrowingLvlTime >= upGrowingLvlTime;
        }

        private void PourFlowerBase()
        {
            if (IsFlowerNeedWater)
            {
                wateringTable.DecreaseFlowersThatNeedWaterQuantity();
            }
            
            IsFlowerNeedWater = false;
            HideWaterIndicator();
        }

        private void UpFlowerGrowingLvl()
        {
            ++FlowerGrowingLvl;
            PotObjects.SetFlowerLvlMesh(PlantedFlowerInfo, FlowerGrowingLvl);
        }

        private void ShowSoil()
        {
            IsSoilInsidePot = true;
            PotObjects.ShowSoil();
        }

        private void ShowSeed(FlowerInfo flowerInfoForPlanting)
        {
            PlantedFlowerInfo = flowerInfoForPlanting;
            PotObjects.SetFlowerLvlMesh(PlantedFlowerInfo, FlowerGrowingLvl);
            PotObjects.ShowFlower();
            fertilizersTable.AddActivePot(this);
        }

        private void ShowWaterIndicator()
        {
            wateringTable.IncreaseFlowersThatNeedWaterQuantity();
            IsFlowerNeedWater = true;
            PotObjects.ShowWaterIndicator();
        }

        private void ShowWeed()
        {
            IsWeedInPot = true;
            PotObjects.ShowWeed();
            PotObjects.SetWeedLvlMesh(weedGrowingLvl);
        }

        private void SetTreatedGrothAcceleratorCoeff()
        {
            IsPotTreatedByGrothAccelerator = true;
            currentGrothAcceleratorCoeff = fertilizersSetting.GrothAcceleratorCoeff;
        }

        private void ResetGrowingLvlTime()
        {
            currentUpGrowingLvlTime = tablesSettings.PrimaryGrowingLvlTime;
        }

        private void ResetWeedGrowingLvl()
        {
            weedGrowingLvl = weedSettings.PrimaryWeedGrowingLvl;
        }

        private void ResetFlowerGrowingLvl()
        {
            FlowerGrowingLvl = flowersSettings.PrimaryFlowerGrowingLvl;
        }

        private void ResetPlantedFlowerInfo()
        {
            PlantedFlowerInfo = flowersContainer.EmptyFlowerInfo;
        }

        private void ResetGrothAcceleratorParameters()
        {
            IsPotTreatedByGrothAccelerator = false;
            currentGrothAcceleratorCoeff = fertilizersSetting.PrimaryGrothAcceleratorCoeff;
        }
    }
}
