using DG.Tweening;
using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using FlowerShop.Settings;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.FlowerSales
{
    public class FlowerSaleTable : FlowerTable
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly FlowerSaleTablesForCustomers flowerSaleTablesForCustomers;
        [Inject] private readonly FlowersForSaleCoeffCalculator flowersForSaleCoeffCalculator;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly PlayerMoney playerMoney;
        
        [field: SerializeField] public Transform TargetToLookAt { get; private set; }
        [field: SerializeField] public Transform TablePotTransform { get; private set; }
        [field: SerializeField] public Transform CustomerDestinationTarget { get; private set; }
        [field: HideInInspector, SerializeField] public MeshFilter SalableSoilMeshFilter { get; private set; }
        [field: HideInInspector, SerializeField] public MeshFilter SalableFlowerMeshFilter { get; private set; }
        
        [SerializeField] private MeshRenderer salableSoilRenderer;
        [SerializeField] private MeshRenderer salableFlowerRenderer;
        
        [HideInInspector, SerializeField] private Transform salableSoilTransform;
        
        private Pot potForSale;
        private bool isFlowerOnSaleTable;

        public FlowerInfo FlowerInfoForSale { get; private set; }
        
        private void OnValidate()
        {
            SalableSoilMeshFilter = salableSoilRenderer.GetComponent<MeshFilter>();
            SalableFlowerMeshFilter = salableFlowerRenderer.GetComponent<MeshFilter>();
            salableSoilTransform = salableSoilRenderer.GetComponent<Transform>();
        }

        public override void ExecuteClickableAbility()
        {
            if (playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                potForSale = currentPot;
                
                if (CanFlowerBePuttedOnFlowerSaleTable())
                {
                    SetPlayerDestination();
                }
            }
        }

        public override void ExecutePlayerAbility()
        {
            FlowerInfoForSale = potForSale.PlantedFlowerInfo;
            salableSoilRenderer.enabled = true;
            SalableSoilMeshFilter.mesh = potForSale.PotObjects.SoilMeshFilter.mesh;
            salableFlowerRenderer.enabled = true;
            SalableFlowerMeshFilter.mesh = potForSale.PotObjects.FlowerMeshFilter.mesh;
            potForSale.CleanPot();
            
            salableSoilTransform.SetPositionAndRotation(
                position: playerComponents.PlayerHandsForBigObjectTransform.position,
                rotation: playerComponents.PlayerHandsForBigObjectTransform.rotation);

            salableSoilTransform.DOJump(
                endValue: TablePotTransform.position, 
                jumpPower: actionsWithTransformSettings.PickableObjectDoTweenJumpPower, 
                numJumps: actionsWithTransformSettings.DefaultDoTweenJumpsNumber, 
                duration: actionsWithTransformSettings.MovingPickableObjectTime)
                .OnComplete(() => playerBusyness.SetPlayerFree());

            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.ThrowTrigger);

            isFlowerOnSaleTable = true;
            
            flowerSaleTablesForCustomers.AddSaleTableWithFlower(this);
            flowersForSaleCoeffCalculator.AddFlowerSaleTableWithFLowerInList(this);
        }

        public void SaleFlower()
        {
            salableSoilRenderer.enabled = false;
            salableFlowerRenderer.enabled = false;
            flowersForSaleCoeffCalculator.RemoveFlowerSaleTableWithoutFlowerFromList(this);
            isFlowerOnSaleTable = false;
            playerMoney.AddPlayerMoney(FlowerInfoForSale.FlowerSellingPrice);
        }

        private bool CanFlowerBePuttedOnFlowerSaleTable()
        {
           return playerBusyness.IsPlayerFree && 
                  !isFlowerOnSaleTable && 
                  potForSale.FlowerGrowingLvl >= flowersSettings.MaxFlowerGrowingLvl && 
                  !potForSale.IsWeedInPot;
        }
    }
}
