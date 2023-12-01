using FlowerShop.Flowers;
using FlowerShop.PickableObjects.Helpers;
using FlowerShop.PickableObjects.Moving;
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
        [Inject] private readonly GameConfiguration gameConfiguration;
        [Inject] private readonly FlowersContainer flowersContainer;

        [field: SerializeField] public GrowingRoom GrowingRoom { get; private set; }
    
        [HideInInspector, SerializeField] private ObjectMoving objectMoving;
    
        private FlowerInfo plantedFlowerInfo;
        private PotObjects potObjects;
        private float upGrowingLvlTime;
        private float currentUpGrowingLvlTime;
        private int flowerGrowingLvl;
        private int weedGrowingLvl;
        private bool isSoilInsidePot;
        private bool isPotOnGrowingTable;
        private bool isFlowerNeedWater;
        private bool isWeedInPot;

        public PotObjects PotObjects 
        { 
            get => potObjects; 
        }

        public FlowerInfo PlantedFlowerInfo
        {
            get => plantedFlowerInfo;
        }

        public int FlowerGrowingLvl
        {
            get => flowerGrowingLvl;
        }

        public bool IsSoilInsidePot
        { 
            get => isSoilInsidePot;
        }

        public bool IsFlowerNeedWater
        {
            get => isFlowerNeedWater;
        }

        public bool IsWeedInPot
        {
            get => isWeedInPot;
        }

        private void OnValidate()
        {
            objectMoving = GetComponent<ObjectMoving>();
        }

        private void Start()
        {
            // move to awake because we don't need to recashing it on disable/enable
            potObjects = GetComponent<PotObjects>();
            upGrowingLvlTime = gameConfiguration.UpGrowingLvlTime;
            plantedFlowerInfo = flowersContainer.EmptyFlowerInfo;
        }

        private void Update()
        {
            if (isWeedInPot)
            {
                // 3 should be in settings
                if (weedGrowingLvl < 3 && ShouldGrowingLvlIncrease())
                {
                    currentUpGrowingLvlTime = 0; 
                    weedGrowingLvl++;
                    potObjects.SetWeedLvlMesh(weedGrowingLvl);
                }
            }
            else if (isPotOnGrowingTable && !isFlowerNeedWater && flowerGrowingLvl < 3 && ShouldGrowingLvlIncrease())
            {
                currentUpGrowingLvlTime = 0;
                isFlowerNeedWater = true;
                potObjects.ShowWaterIndicator();
            }
        }

        private bool ShouldGrowingLvlIncrease()
        {
            currentUpGrowingLvlTime += Time.deltaTime;
            return currentUpGrowingLvlTime >= upGrowingLvlTime;
        }

        public void FillPotWithSoil()
        {
            isSoilInsidePot = true;
            potObjects.ShowSoil();
        }

        public void PlantSeed(FlowerInfo flowerInfoForPlanting)
        {
            plantedFlowerInfo = flowerInfoForPlanting;
            flowerGrowingLvl = 0;
            potObjects.SetFlowerLvlMesh(plantedFlowerInfo, flowerGrowingLvl);
            potObjects.ShowFlower();
        }

        public void PlantWeed()
        {
            isWeedInPot = true;
            weedGrowingLvl = 1;
            currentUpGrowingLvlTime = 0;
            potObjects.SetWeedLvlMesh(weedGrowingLvl);
            potObjects.ShowWeed();
        }

        public void DeleteWeed()
        {
            isWeedInPot = false;
            weedGrowingLvl = 0;
            currentUpGrowingLvlTime = 0;
            potObjects.HideWeed();
        }

        public void CleanPot()
        {
            isSoilInsidePot = false;
            isFlowerNeedWater = false;
            isWeedInPot = false;
            plantedFlowerInfo = flowersContainer.EmptyFlowerInfo;
            weedGrowingLvl = 0;
            flowerGrowingLvl = 0;
            currentUpGrowingLvlTime = 0;
            potObjects.HideAllPotObjects();
        }

        public void TakeInPlayerHandsFromGrowingTableAndSetPlayerFree()
        {
            isPotOnGrowingTable = false;
            TakeInPlayerHandsAndSetPlayerFree();
        }

        public void TakeInPlayerHandsAndSetPlayerFree()
        {
            playerPickableObjectHandler.CurrentPickableObject = this;
            objectMoving.MoveObject(targetFinishTransform: playerComponents.PlayerHandsForBigObjectTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.TakeBigObjectTrigger, 
                setPlayerFree: true);
        }

        public void PutOnGrowingTableAndSetPlayerFree(Transform targetTransform, int growingTableLvl)
        {
            isPotOnGrowingTable = true;
            if (currentUpGrowingLvlTime > 0)
            {
                float currentGrowingLvlTimeCoeff = currentUpGrowingLvlTime / upGrowingLvlTime;
                upGrowingLvlTime = gameConfiguration.UpGrowingLvlTime - gameConfiguration.UpGrowingLvlTableLvlTimeDelta * growingTableLvl;
                currentUpGrowingLvlTime *= currentGrowingLvlTimeCoeff;
            }
            else
            {
                upGrowingLvlTime = gameConfiguration.UpGrowingLvlTime - gameConfiguration.UpGrowingLvlTableLvlTimeDelta * growingTableLvl;
            }
            PutOnTableAndSetPlayerFree(targetTransform);
        }

        public void PutOnTableAndKeepPlayerBusy(Transform targetTransform)
        {
            objectMoving.MoveObject(targetFinishTransform: targetTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.GiveBigObjectTrigger, 
                setPlayerFree: false);
        }
    
        public void PutOnTableAndSetPlayerFree(Transform targetTransform)
        {
            objectMoving.MoveObject(targetFinishTransform: targetTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.GiveBigObjectTrigger, 
                setPlayerFree: true); 
        }

        public void PourFlower()
        {
            isFlowerNeedWater = false;
            potObjects.HideWaterIndicator();
            ++flowerGrowingLvl;
            potObjects.SetFlowerLvlMesh(plantedFlowerInfo, flowerGrowingLvl);

            ((WateringCan)playerPickableObjectHandler.CurrentPickableObject).PourPotWithWateringCan();
        }

        public void CrossFlower()
        {
            --flowerGrowingLvl;
            potObjects.SetFlowerLvlMesh(plantedFlowerInfo, flowerGrowingLvl);
        }
    }
}
