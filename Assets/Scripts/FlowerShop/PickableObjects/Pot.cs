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
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly TablesSettings tablesSettings;
        [Inject] private readonly FlowersContainer flowersContainer;
        
        [HideInInspector, SerializeField] private ObjectMoving objectMoving;
    
        private float upGrowingLvlTime;
        private float currentUpGrowingLvlTime;
        private float growingLvlTimeProgress;
        private int weedGrowingLvl;
        private bool isPotOnGrowingTable;

        [field: SerializeField] public GrowingRoom GrowingRoom { get; private set; }
        [field: SerializeField] public PotObjects PotObjects { get; private set; }

        public FlowerInfo PlantedFlowerInfo { get; private set; }
        public int FlowerGrowingLvl { get; private set; }
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
        }

        private void Update()
        {
            if (IsWeedInPot)
            {
                if (ShouldWeedGrowingLvlIncrease())
                {
                    ResetGrowingLvlTime();
                    weedGrowingLvl++;
                    PotObjects.SetWeedLvlMesh(weedGrowingLvl);
                }
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

        public void CleanPot()
        {
            IsSoilInsidePot = false;
            IsFlowerNeedWater = false;
            IsWeedInPot = false;
            ResetPlantedFlowerInfo();
            ResetWeedGrowingLvl();
            ResetFlowerGrowingLvl();
            ResetGrowingLvlTime();
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

        public void PourFlower()
        {
            IsFlowerNeedWater = false;
            PotObjects.HideWaterIndicator();
            ++FlowerGrowingLvl;
            PotObjects.SetFlowerLvlMesh(PlantedFlowerInfo, FlowerGrowingLvl);

            ((WateringCan)playerPickableObjectHandler.CurrentPickableObject).PourPotWithWateringCan();
        }

        public void CrossFlower()
        {
            --FlowerGrowingLvl;
            PotObjects.SetFlowerLvlMesh(PlantedFlowerInfo, FlowerGrowingLvl);
        }

        private bool ShouldWeedGrowingLvlIncrease()
        {
            return weedGrowingLvl < weedSettings.MaxWeedGrowingLvl && 
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
            currentUpGrowingLvlTime += Time.deltaTime;
            return currentUpGrowingLvlTime >= upGrowingLvlTime;
        }

        private void CalculateGrowingLvlTimeProgress()
        {
            growingLvlTimeProgress = currentUpGrowingLvlTime / upGrowingLvlTime;
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
    }
}
