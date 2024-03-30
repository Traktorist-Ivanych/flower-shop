using System;
using System.Collections.Generic;
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
        [Inject] private readonly ShopRating shopRating;
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
        [field: SerializeField] public List<Transform> ToFlowerPathPoints { get; private set; }
        [field: SerializeField] public List<Transform> OutFlowerPathPoints { get; private set; }
        [field: SerializeField] public List<Transform> FinishWithFlowerPathPoints { get; private set; }
        
        public FlowerInfo FlowerInfoForSale { get; private set; }
        
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

        private protected override void TryInteractWithTable()
        {
            base.TryInteractWithTable();

            if (CanPlayerPutFlowerOnTable())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(PutFlowerOnTable);
            }
        }

        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerPutFlowerOnTable();
        }

        public void SaleFlower()
        {
            isFlowerOnSaleTable = false;
            salableSoilRenderer.enabled = false;
            salableFlowerRenderer.enabled = false;
            shopRating.AddGrade(flowersForSaleCoeffCalculator.CalculateCurrentGrade());
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

        private void OnDrawGizmos()
        {
            Vector3 offset = new Vector3(0.035f,0,0.035f);
            
            if (ToFlowerPathPoints.Count > 1)
            {
                Gizmos.color = Color.blue;
                Vector3[] toFlowerPathPoints = new Vector3[ToFlowerPathPoints.Count];
                for (int i = 0; i < ToFlowerPathPoints.Count; i++)
                {
                    toFlowerPathPoints[i] = ToFlowerPathPoints[i].position;
                }
                Gizmos.DrawLineStrip(toFlowerPathPoints, false);
            }
            
            if (OutFlowerPathPoints.Count > 1)
            {
                
                Gizmos.color = Color.red;
                Vector3[] outFlowerPathPoints = new Vector3[OutFlowerPathPoints.Count + 1];
                for (int i = 0; i < outFlowerPathPoints.Length; i++)
                {
                    if (i == 0)
                    {
                        outFlowerPathPoints[i] = CustomerDestinationTarget.position - offset;
                    }
                    else
                    {
                        outFlowerPathPoints[i] = OutFlowerPathPoints[i-1].position - offset;
                    }
                }
                Gizmos.DrawLineStrip(outFlowerPathPoints, false);
            }
            
            if (FinishWithFlowerPathPoints.Count > 1)
            {
                Gizmos.color = Color.gray;
                Vector3[] finishFlowerPathPoints = new Vector3[FinishWithFlowerPathPoints.Count + 1];
                for (int i = 0; i < finishFlowerPathPoints.Length; i++)
                {
                    if (i == 0)
                    {
                        finishFlowerPathPoints[i] = CustomerDestinationTarget.position - offset * 2;
                    }
                    else
                    {
                        finishFlowerPathPoints[i] = FinishWithFlowerPathPoints[i-1].position - offset * 2;
                    }
                }
                Gizmos.DrawLineStrip(finishFlowerPathPoints, false);
            }
        }
    }
}
