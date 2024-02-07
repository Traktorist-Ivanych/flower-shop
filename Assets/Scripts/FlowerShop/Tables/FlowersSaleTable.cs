using DG.Tweening;
using FlowerShop.Flowers;
using FlowerShop.FlowersSale;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Settings;
using FlowerShop.Sounds;
using FlowerShop.Tables.Abstract;
using PlayerControl;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class FlowersSaleTable : Table, ISavableObject
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly FlowersForSaleCoeffCalculator flowersForSaleCoeffCalculator;
        [Inject] private readonly FlowersSaleTablesForCustomers flowersSaleTablesForCustomers;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly PlayerMoney playerMoney;
        [Inject] private readonly ReferencesForLoad referencesForLoad;
        [Inject] private readonly SoundsHandler soundsHandler;

        [SerializeField] private MeshRenderer salableSoilRenderer;
        [SerializeField] private MeshRenderer salableFlowerRenderer;
        
        [HideInInspector, SerializeField] private Transform salableSoilTransform;
        
        private Pot potForSale;
        private bool isFlowerOnSaleTable;
        private string flowerInfoOnTableUniqueKey;
        
        [field: SerializeField] public Transform TablePotTransform { get; private set; }
        [field: SerializeField] public Transform CustomerDestinationTarget { get; private set; }
        [field: HideInInspector, SerializeField] public MeshFilter SalableSoilMeshFilter { get; private set; }
        [field: HideInInspector, SerializeField] public MeshFilter SalableFlowerMeshFilter { get; private set; }
        [field: SerializeField] public string UniqueKey { get; private set; }
        
        public FlowerInfo FlowerInfoForSale { get; private set; }
        public Transform TargetToLookAt => targetToLookAt;
        
        private void OnValidate()
        {
            SalableSoilMeshFilter = salableSoilRenderer.GetComponent<MeshFilter>();
            SalableFlowerMeshFilter = salableFlowerRenderer.GetComponent<MeshFilter>();
            salableSoilTransform = salableSoilRenderer.GetComponent<Transform>();
        }

        private void Awake()
        {
            Load();
        }

        public override void ExecuteClickableAbility()
        {
            if (CanPlayerPutFlowerOnTable())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(PutFlowerOnTable);
            }
        }

        public void SaleFlower()
        {
            isFlowerOnSaleTable = false;
            salableSoilRenderer.enabled = false;
            salableFlowerRenderer.enabled = false;
            flowersForSaleCoeffCalculator.RemoveFlowerSaleTableWithoutFlowerFromList(this);
            playerMoney.AddPlayerMoney(FlowerInfoForSale.FlowerSellingPrice);
            soundsHandler.PlayAddMoneyAudio();
            
            ResetFlowerInfoOnTable();
            
            SavesHandler.DeletePlayerPrefsKey(UniqueKey);
        }

        public void Load()
        {
            FlowerInfoReferenceForSaving flowerInfoReferenceForLoading =
                SavesHandler.Load<FlowerInfoReferenceForSaving>(UniqueKey);

            if (flowerInfoReferenceForLoading.IsValuesSaved)
            {
                flowerInfoOnTableUniqueKey = flowerInfoReferenceForLoading.FlowerInfoOnTableUniqueKey;

                FlowerInfoForSale = referencesForLoad.GetReference<FlowerInfo>(flowerInfoOnTableUniqueKey);

                if (FlowerInfoForSale != flowersSettings.FlowerInfoEmpty)
                {
                    SetFlowerOnSaleTable();
                }
            }
        }

        public void Save()
        {
            FlowerInfoReferenceForSaving flowerInfoReferenceForSaving = new(flowerInfoOnTableUniqueKey);
            
            SavesHandler.Save(UniqueKey, flowerInfoReferenceForSaving);
        }

        private bool CanPlayerPutFlowerOnTable()
        {
            if (playerBusyness.IsPlayerFree && !isFlowerOnSaleTable && 
                playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                potForSale = currentPot;
                
                return potForSale.FlowerGrowingLvl >= flowersSettings.MaxFlowerGrowingLvl && 
                       !potForSale.IsWeedInPot;
            }

            return false;
        }

        private void PutFlowerOnTable()
        {
            FlowerInfoForSale = potForSale.PlantedFlowerInfo;
            potForSale.CleanPot();
            flowerInfoOnTableUniqueKey = FlowerInfoForSale.UniqueKey;
            
            SetFlowerOnSaleTable();

            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.ThrowTrigger);
            
            salableSoilTransform.SetPositionAndRotation(
                position: playerComponents.PlayerHandsForBigObjectTransform.position,
                rotation: playerComponents.PlayerHandsForBigObjectTransform.rotation);

            salableSoilTransform.DOJump(
                    endValue: TablePotTransform.position, 
                    jumpPower: actionsWithTransformSettings.PickableObjectDoTweenJumpPower, 
                    numJumps: actionsWithTransformSettings.DefaultDoTweenJumpsNumber, 
                    duration: actionsWithTransformSettings.MovingPickableObjectTime)
                .OnComplete(() => playerBusyness.SetPlayerFree());
            
            Save();
        }

        private void SetFlowerOnSaleTable()
        {
            isFlowerOnSaleTable = true;
            
            salableSoilRenderer.enabled = true;
            SalableSoilMeshFilter.mesh = FlowerInfoForSale.FlowerSoilMesh;
            salableFlowerRenderer.enabled = true;
            SalableFlowerMeshFilter.mesh = FlowerInfoForSale.GetFlowerLvlMesh(flowersSettings.MaxFlowerGrowingLvl);
            
            flowersSaleTablesForCustomers.AddSaleTableWithFlower(this);
            flowersForSaleCoeffCalculator.AddSaleTableWithFLowerInList(this);
        }

        private void ResetFlowerInfoOnTable()
        {
            FlowerInfoForSale = flowersSettings.FlowerInfoEmpty;
            flowerInfoOnTableUniqueKey = flowersSettings.FlowerInfoEmpty.UniqueKey;
        }
    }
}
