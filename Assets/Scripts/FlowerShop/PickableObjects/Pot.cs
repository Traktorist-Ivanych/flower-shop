using FlowerShop.Fertilizers;
using FlowerShop.Flowers;
using FlowerShop.PickableObjects.Helpers;
using FlowerShop.PickableObjects.Moving;
using FlowerShop.Tables;
using FlowerShop.Weeds;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.PickableObjects
{
    [RequireComponent(typeof(PotObjects))]
    [RequireComponent(typeof(ObjectMoving))]
    public class Pot : MonoBehaviour, IPickableObject
    {
        [Inject] private readonly PlayerPickableObjectHandler playerPickableObjectHandler;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly WeedSettings weedSettings;
        [Inject] private readonly FertilizersSetting fertilizersSetting;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly TablesSettings tablesSettings;
        [Inject] private readonly FlowersContainer flowersContainer;
        
        [HideInInspector, SerializeField] private ObjectMoving objectMoving;

        private float currentGrothAcceleratorCoeff;
        private float upGrowingLvlTime;
        private float currentUpGrowingLvlTime;
        private float growingLvlTimeProgress;
        private int weedGrowingLvl;
        private bool isPotOnGrowingTable;

        [field: SerializeField] public GrowingRoom GrowingRoom { get; private set; }
        [field: SerializeField] public PotObjects PotObjects { get; private set; }

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

        private void Start()
        {
            upGrowingLvlTime = tablesSettings.UpGrowingLvlTime;
            ResetPlantedFlowerInfo();
            CalculateGrowingLvlTimeProgress();
            ResetGrothAcceleratorParameters();
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
                IsFlowerNeedWater = true;
                PotObjects.ShowWaterIndicator();
            }
        }

        public void FillPotWithSoil()
        {
            IsSoilInsidePot = true;
            PotObjects.ShowSoil();
        }

        public void PlantSeed(FlowerInfo flowerInfoForPlanting)
        {
            PlantedFlowerInfo = flowerInfoForPlanting;
            ResetFlowerGrowingLvl();
            ResetGrowingLvlTime();
            PotObjects.SetFlowerLvlMesh(PlantedFlowerInfo, FlowerGrowingLvl);
            PotObjects.ShowFlower();
        }

        public void PourFlower()
        {
            HideWaterIndicator();
            UpFlowerGrowingLvl();

            ((WateringCan)playerPickableObjectHandler.CurrentPickableObject).PourPotWithWateringCan();
        }

        public void PlantWeed()
        {
            IsWeedInPot = true;
            ResetWeedGrowingLvl();
            ResetGrowingLvlTime();
            PotObjects.ShowWeed();
            PotObjects.SetWeedLvlMesh(weedGrowingLvl);
        }

        public void DeleteWeed()
        {
            IsWeedInPot = false;
            ResetWeedGrowingLvl();
            ResetGrowingLvlTime();
            PotObjects.HideWeed();
        }

        public void TreatPotByGrothAccelerator()
        {
            IsPotTreatedByGrothAccelerator = true;
            currentGrothAcceleratorCoeff = fertilizersSetting.GrothAcceleratorCoeff;
        }

        public void TreatPotByGrowingLvlIncreaser()
        {
            UpFlowerGrowingLvl();
            if (FlowerGrowingLvl >= flowersSettings.MaxFlowerGrowingLvl)
            {
                HideWaterIndicator();
            }
        }

        public void TreatPotByGrowerToMaxLvl()
        {
            HideWaterIndicator();
            
            FlowerGrowingLvl = flowersSettings.MaxFlowerGrowingLvl;
            PotObjects.SetFlowerLvlMesh(PlantedFlowerInfo, FlowerGrowingLvl);
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
        }

        public void TakeInPlayerHandsFromGrowingTableAndSetPlayerFree()
        {
            isPotOnGrowingTable = false;
            CalculateGrowingLvlTimeProgress();
            TakeInPlayerHandsAndSetPlayerFree();
        }

        public void TakeInPlayerHandsAndSetPlayerFree()
        {
            playerPickableObjectHandler.CurrentPickableObject = this;
            
            objectMoving.MoveObject(
                targetFinishTransform: playerComponents.PlayerHandsForBigObjectTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.TakeBigObjectTrigger, 
                setPlayerFree: true);
        }

        public void PutOnGrowingTableAndSetPlayerFree(Transform targetTransform, int growingTableLvl)
        {
            isPotOnGrowingTable = true;
            upGrowingLvlTime = tablesSettings.UpGrowingLvlTime - tablesSettings.UpGrowingLvlTableLvlTimeDelta * growingTableLvl;
            currentUpGrowingLvlTime = upGrowingLvlTime * growingLvlTimeProgress;
            PutOnTableAndSetPlayerFree(targetTransform);
        }

        public void PutOnTableAndKeepPlayerBusy(Transform targetTransform)
        {
            objectMoving.MoveObject(
                targetFinishTransform: targetTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.GiveBigObjectTrigger, 
                setPlayerFree: false);
        }
    
        public void PutOnTableAndSetPlayerFree(Transform targetTransform)
        {
            objectMoving.MoveObject(
                targetFinishTransform: targetTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.GiveBigObjectTrigger, 
                setPlayerFree: true); 
        }

        public void CrossFlower()
        {
            --FlowerGrowingLvl;
            PotObjects.SetFlowerLvlMesh(PlantedFlowerInfo, FlowerGrowingLvl);
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

        private void CalculateGrowingLvlTimeProgress()
        {
            growingLvlTimeProgress = currentUpGrowingLvlTime / upGrowingLvlTime;
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
