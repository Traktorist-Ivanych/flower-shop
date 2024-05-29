using System.Collections.Generic;
using DG.Tweening;
using FlowerShop.Customers;
using FlowerShop.Customers.VipAndComplaints;
using FlowerShop.Flowers;
using FlowerShop.FlowersSale;
using FlowerShop.Help;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Settings;
using FlowerShop.Sounds;
using FlowerShop.Tables.Abstract;
using FlowerShop.Tables.Interfaces;
using PlayerControl;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class VipOrdersTable : Table, ISavableObject, ISpecialSaleTable
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly VipOrdersHandler vipOrdersHandler;
        [Inject] private readonly CustomersSpawner customersSpawner;
        [Inject] private readonly FlowersForSaleCoeffCalculatorSettings flowersForSaleCoeffCalculatorSettings;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly HelpCanvasLiaison helpCanvasLiaison;
        [Inject] private readonly HelpTexts helpTexts;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly PlayerMoney playerMoney;
        [Inject] private readonly ReferencesForLoad referencesForLoad;
        [Inject] private readonly ShopRating shopRating;
        [Inject] private readonly SoundsHandler soundsHandler;
        
        [SerializeField] private MeshRenderer soilRenderer;
        [SerializeField] private MeshRenderer flowerRenderer;
        
        [HideInInspector, SerializeField] private Transform soilTransform;
        
        private FlowerInfo vipOrderFlowerInfo;
        private Pot potForVipOrder;
        private bool isPotOnTable;
        private string flowerInfoOnTableUniqueKey;
        
        [field: SerializeField] public Transform TablePotTransform { get; private set; }
        [field: HideInInspector, SerializeField] public MeshFilter SoilMeshFilter { get; private set; }
        [field: HideInInspector, SerializeField] public MeshFilter FlowerMeshFilter { get; private set; }
        [field: SerializeField] public string UniqueKey { get; private set; }
        [field: SerializeField] public List<Transform> ToFlowerPathPoints { get; private set; }
        [field: SerializeField] public List<Transform> FinishWithFlowerPathPoints { get; private set; }
        
        private void OnValidate()
        {
            SoilMeshFilter = soilRenderer.GetComponent<MeshFilter>();
            FlowerMeshFilter = flowerRenderer.GetComponent<MeshFilter>();
            soilTransform = soilRenderer.GetComponent<Transform>();
        }

        private void Awake()
        {
            Load();
        }

        private protected override void TryInteractWithTable()
        {
            base.TryInteractWithTable();

            if (CanPlayerUseComplaintsTable())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(UseComplaintsTable);
            }
            else if (CanPlayerUseTableInfoCanvas())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(UseTableInfoCanvas);
            }
            else
            {
                TryToShowHelpCanvas();
            }
        }

        private void TryToShowHelpCanvas()
        {
            if (!playerBusyness.IsPlayerFree)
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.PlayerBusy);
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                if (isPotOnTable)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.TableAlreadyHasPot);
                }
                else if (!vipOrdersHandler.IsVipOrderActive)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.NoAvailableVipOrders);
                }
                else if (currentPot.PlantedFlowerInfo == flowersSettings.FlowerInfoEmpty)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.NoFlowerPlanted);
                }
                else if (!currentPot.PlantedFlowerInfo.Equals(vipOrdersHandler.VipOrderFlowerInfo))
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.WrongFlower);
                }
                else if (currentPot.FlowerGrowingLvl < flowersSettings.MaxFlowerGrowingLvl)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.FlowerDidNotGrow);
                }
                else if (currentPot.IsWeedInPot)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.WeedInPot);
                }
            }
            else if (playerPickableObjectHandler.IsPickableObjectNull)
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.EmptyHands);
            }
            else
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.WrongPickableObject);
            }
        }

        public void ExecuteSpecialSale()
        {
            playerMoney.AddPlayerMoney(vipOrderFlowerInfo.FlowerSellingPrice * 
                                       vipOrdersHandler.CurrentVipOrderPriceMultipler);
            soundsHandler.PlayAddMoneyAudio();
            shopRating.AddGrade(flowersForSaleCoeffCalculatorSettings.MaxShopGrade);

            isPotOnTable = false;
            soilRenderer.enabled = false;
            flowerRenderer.enabled = false;
            
            vipOrderFlowerInfo = flowersSettings.FlowerInfoEmpty;
            flowerInfoOnTableUniqueKey = flowersSettings.FlowerInfoEmpty.UniqueKey;
            
            SavesHandler.DeletePlayerPrefsKey(UniqueKey);
        }

        public void Load()
        {
            FlowerInfoReferenceForSaving flowerInfoReferenceForLoading =
                SavesHandler.Load<FlowerInfoReferenceForSaving>(UniqueKey);

            if (flowerInfoReferenceForLoading.IsValuesSaved)
            {
                flowerInfoOnTableUniqueKey = flowerInfoReferenceForLoading.FlowerInfoOnTableUniqueKey;

                vipOrderFlowerInfo = referencesForLoad.GetReference<FlowerInfo>(flowerInfoOnTableUniqueKey);

                if (vipOrderFlowerInfo != flowersSettings.FlowerInfoEmpty)
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

        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerUseComplaintsTable() || CanPlayerUseTableInfoCanvas();
        }

        private bool CanPlayerUseComplaintsTable()
        {
            if (playerBusyness.IsPlayerFree && playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                potForVipOrder = currentPot;
                
                return !isPotOnTable && !potForVipOrder.IsWeedInPot &&
                       potForVipOrder.PlantedFlowerInfo.Equals(vipOrdersHandler.VipOrderFlowerInfo) &&
                       potForVipOrder.FlowerGrowingLvl >= flowersSettings.MaxFlowerGrowingLvl;
            }
            
            return false;
        }

        private void UseComplaintsTable()
        {
            vipOrderFlowerInfo = potForVipOrder.PlantedFlowerInfo;
            potForVipOrder.CleanPot();
            flowerInfoOnTableUniqueKey = vipOrderFlowerInfo.UniqueKey;
            
            SetFlowerOnSaleTable();
            vipOrdersHandler.CompleteVipOrder();

            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.ThrowTrigger);
            
            soilTransform.SetPositionAndRotation(
                position: playerComponents.PlayerHandsForBigObjectTransform.position,
                rotation: playerComponents.PlayerHandsForBigObjectTransform.rotation);

            soilTransform.DOJump(
                    endValue: TablePotTransform.position, 
                    jumpPower: actionsWithTransformSettings.PickableObjectDoTweenJumpPower, 
                    numJumps: actionsWithTransformSettings.DefaultDoTweenJumpsNumber, 
                    duration: actionsWithTransformSettings.MovingPickableObjectTime)
                .OnComplete(() => playerBusyness.SetPlayerFree());

            Save();
        }
        
        private void SetFlowerOnSaleTable()
        {
            isPotOnTable = true;
            customersSpawner.AddSpecialSaleTable(this);
            
            soilRenderer.enabled = true;
            SoilMeshFilter.mesh = vipOrderFlowerInfo.FlowerSoilMesh;
            flowerRenderer.enabled = true;
            FlowerMeshFilter.mesh = vipOrderFlowerInfo.GetFlowerLvlMesh(flowersSettings.MaxFlowerGrowingLvl);
        }

        private bool CanPlayerUseTableInfoCanvas()
        {
            return playerBusyness.IsPlayerFree && playerPickableObjectHandler.CurrentPickableObject is InfoBook;
        }

        private void UseTableInfoCanvas()
        {
            tableInfoCanvasLiaison.ShowCanvas(tableInfo, growingRoom);
        }
    }
}