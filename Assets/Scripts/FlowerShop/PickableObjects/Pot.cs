using FlowerShop.Fertilizers;
using FlowerShop.Flowers;
using FlowerShop.PickableObjects.Helpers;
using FlowerShop.PickableObjects.Moving;
using FlowerShop.Saves.SaveData;
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
        [Inject] private readonly PlayerPickableObjectHandler playerPickableObjectHandler;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly WeedSettings weedSettings;
        [Inject] private readonly FertilizersSetting fertilizersSetting;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly TablesSettings tablesSettings;
        [Inject] private readonly FlowersContainer flowersContainer;

        [SerializeField] private PotsRack potsRack;
        
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
                ResetGrowingLvlTime();
                weedGrowingLvl++;
                PotObjects.SetWeedLvlMesh(weedGrowingLvl);
            }
            else if (ShouldWaterIndicatorBeDisplayed())
            {
                ResetGrowingLvlTime();
                
                ShowWaterIndicator();
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
            
            ShowSeed(flowerInfoForPlanting);
            
            Save();
        }

        public void PourFlower()
        {
            HideWaterIndicator();
            UpFlowerGrowingLvl();

            ((WateringCan)playerPickableObjectHandler.CurrentPickableObject).PourPotWithWateringCan();
            
            Save();
        }

        public void PlantWeed()
        {
            ResetWeedGrowingLvl();
            ResetGrowingLvlTime();
            
            ShowWeed();
            
            Save();
        }

        public void DeleteWeed()
        {
            IsWeedInPot = false;
            ResetWeedGrowingLvl();
            ResetGrowingLvlTime();
            PotObjects.HideWeed();
            
            Save();
        }

        public void TreatPotByGrothAccelerator()
        {
            SetTreatedGrothAcceleratorCoeff();
            
            Save();
        }

        public void TreatPotByGrowingLvlIncreaser()
        {
            UpFlowerGrowingLvl();
            if (FlowerGrowingLvl >= flowersSettings.MaxFlowerGrowingLvl)
            {
                HideWaterIndicator();
            }
            
            Save();
        }

        public void TreatPotByGrowerToMaxLvl()
        {
            HideWaterIndicator();
            
            FlowerGrowingLvl = flowersSettings.MaxFlowerGrowingLvl;
            PotObjects.SetFlowerLvlMesh(PlantedFlowerInfo, FlowerGrowingLvl);
            
            Save();
        }

        public void CleanPot()
        {
            IsSoilInsidePot = false;
            IsFlowerNeedWater = false;
            IsWeedInPot = false;
            ResetPlantedFlowerInfo();
            ResetWeedGrowingLvl();
            ResetFlowerGrowingLvl();
            ResetGrowingLvlTime();
            ResetGrothAcceleratorParameters();
            PotObjects.HideAllPotObjects();
            
            SavesHandler.DeletePlayerPrefsKey(UniqueKey);
        }

        public void PutOnGrowingTableAndSetPlayerFree(Transform targetTransform, int growingTableLvl)
        {
            PutOnGrowingTableBaseActions(growingTableLvl);
            
            currentUpGrowingLvlTime = upGrowingLvlTime * growingLvlTimeProgress;
            PutOnTableAndSetPlayerFree(targetTransform);
            
            Save();
        }
    
        public void PutOnTableAndSetPlayerFree(Transform targetTransform)
        {
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
            PotForSaving potForSaving = new PotForSaving(IsSoilInsidePot, PlantedFlowerInfo, FlowerGrowingLvl,
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

                if (potForLoading.PlantedFlowerInfo == flowersSettings.FlowerInfoEmpty)
                {
                    ResetPlantedFlowerInfo();
                }
                else
                {
                    ShowSeed(potForLoading.PlantedFlowerInfo);
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
            objectMoving.SetParentAndParentPositionAndRotationOnLoad(playerComponents.PlayerHandsForBigObjectTransform);
            potsRack.RemovePotFromListOnLoad(this);
        }

        public void LoadOnGrowingTable(Transform transformOnTable, int growingTableLvl)
        {
            PutOnGrowingTableBaseActions(growingTableLvl);
            LoadOnTable(transformOnTable);
        }

        public void LoadOnTable(Transform transformOnTable)
        {
            objectMoving.SetParentAndParentPositionAndRotationOnLoad(transformOnTable);
            potsRack.RemovePotFromListOnLoad(this);
        }

        private void PutOnGrowingTableBaseActions(int growingTableLvl)
        {
            cyclicalSaver.CyclicalSaverEvent += Save;
            isPotOnGrowingTable = true;
            upGrowingLvlTime = tablesSettings.UpGrowingLvlTime - tablesSettings.UpGrowingLvlTableLvlTimeDelta * growingTableLvl;
        }

        private void CalculateGrowingLvlTimeProgress()
        {
            growingLvlTimeProgress = currentUpGrowingLvlTime / upGrowingLvlTime;
        }

        private bool ShouldWeedGrowingLvlIncrease()
        {
            return IsWeedInPot && weedGrowingLvl < weedSettings.MaxWeedGrowingLvl && 
                   ShouldGrowingLvlIncrease();
        }

        private bool ShouldWaterIndicatorBeDisplayed()
        {
            return isPotOnGrowingTable && 
                   !IsFlowerNeedWater && 
                   FlowerGrowingLvl < flowersSettings.MaxFlowerGrowingLvl && 
                   ShouldGrowingLvlIncrease();
        }

        private bool ShouldGrowingLvlIncrease()
        {
            currentUpGrowingLvlTime += Time.deltaTime * currentGrothAcceleratorCoeff;
            return currentUpGrowingLvlTime >= upGrowingLvlTime;
        }

        private void HideWaterIndicator()
        {
            IsFlowerNeedWater = false;
            PotObjects.HideWaterIndicator();
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
        }

        private void ShowWaterIndicator()
        {
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
