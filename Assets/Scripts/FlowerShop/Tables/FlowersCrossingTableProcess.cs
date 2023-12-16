using System.Collections;
using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using FlowerShop.Settings;
using FlowerShop.Tables.Abstract;
using FlowerShop.Tables.Helpers;
using PlayerControl;
using UniRx;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    [RequireComponent(typeof(TableObjectsRotation))]
    public class FlowersCrossingTableProcess : UpgradableBreakableTable
    {
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly TablesSettings tablesSettings;
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly FlowersContainer flowersContainer;
        [Inject] private readonly PlayerComponents playerComponents;

        [SerializeField] private Transform tablePotTransform;
        [SerializeField] private FlowerName flowerNameEmpty;
    
        [Header("Indicators")]
        [SerializeField] private MeshRenderer crossingAbilityRedIndicator;
        [SerializeField] private MeshRenderer crossingAbilityGreenIndicator;
        [SerializeField] private MeshRenderer crossedFlowerRoomRedIndicator;
        [SerializeField] private MeshRenderer crossedFlowerRoomBlueIndicator;
        [SerializeField] private MeshRenderer crossedFlowerRoomYellowIndicator;
        [SerializeField] private MeshRenderer crossedFlowerRoomGreenIndicator;
    
        [Header("CrossingTables")]
        [SerializeField] private FlowersCrossingTable firstFlowersCrossingTable;
        [SerializeField] private FlowersCrossingTable secondFlowersCrossingTable;
    
        [Header("CrossingTableBlender")]
        [SerializeField] private MeshFilter crossingTableBlenderMeshFilter;
        [SerializeField] private Mesh[] crossingTableBlenderLvlMeshes = new Mesh[2];

        [Header("GrowingRooms")] 
        [SerializeField] private GrowingRoom growingRoomWild;
        [SerializeField] private GrowingRoom growingRoomExotic;
        [SerializeField] private GrowingRoom growingRoomDecorative;

        [HideInInspector, SerializeField] private TableObjectsRotation tableObjectsRotation;
        
        private Pot potForPlanting;
        private FlowerInfo flowerInfoForPlanting;
        private float crossingFlowerTime;
        private float currentCrossingFlowerTime;
        private bool isFlowerReadyForCrossing;
        private bool isCrossingSeedReady;
        
        private readonly CompositeDisposable crossingFlowersTimerCompositeDisposable = new();

        public bool IsSeedCrossing { get; private set; }

        private void OnValidate()
        {
            tableObjectsRotation = GetComponent<TableObjectsRotation>();
        }
        
        private protected override void Start()
        {
            base.Start();
            
            SetCrossingFlowerTime();

            SetActionsBeforeBrokenQuantity(
                repairsAndUpgradesSettings.CrossingTableMinQuantity * (tableLvl + 1),
                repairsAndUpgradesSettings.CrossingTableMaxQuantity * (tableLvl + 1));
        }

        public override void ExecuteClickableAbility()
        {
            if (!playerBusyness.IsPlayerFree || IsSeedCrossing) return;
            
            if (CanPlayerStartFlowersCrossing())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(() => StartCoroutine(StartFlowersCrossing()));
            }
            else if (CanPlayerPlantCrossedSeed())
            {
                SetPlayerDestinationAndOnPlayerArriveAction( () => StartCoroutine(PlantCrossedSeed()));
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer)
            {
                if (IsTableBroken)
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(FixCrossingTable);
                }
                else if (tableLvl < repairsAndUpgradesSettings.MaxUpgradableTableLvl)
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(ShowUpgradeCanvas);
                }
            }
        }

        public void CheckCrossingAbility()
        {
            crossingAbilityRedIndicator.enabled = true;
            crossingAbilityGreenIndicator.enabled = false;

            crossedFlowerRoomRedIndicator.enabled = true;
            crossedFlowerRoomBlueIndicator.enabled = false;
            crossedFlowerRoomYellowIndicator.enabled = false;
            crossedFlowerRoomGreenIndicator.enabled = false;

            if (firstFlowersCrossingTable.IsPotOnCrossingTable && 
                firstFlowersCrossingTable.PotOnTable.FlowerGrowingLvl >= flowersSettings.MaxFlowerGrowingLvl &&
                secondFlowersCrossingTable.IsPotOnCrossingTable &&
                secondFlowersCrossingTable.PotOnTable.FlowerGrowingLvl >= flowersSettings.MaxFlowerGrowingLvl)
            {
                flowerInfoForPlanting = flowersContainer.GetFlowerFromCrossingRecipe(
                    firstFlowersCrossingTable.PotOnTable.PlantedFlowerInfo.FlowerName,
                    secondFlowersCrossingTable.PotOnTable.PlantedFlowerInfo.FlowerName);

                if (flowerInfoForPlanting.FlowerName == flowerNameEmpty)
                {
                    isFlowerReadyForCrossing = false;
                }
                else
                {
                    crossingAbilityRedIndicator.enabled = false;
                    crossedFlowerRoomRedIndicator.enabled = false;

                    isFlowerReadyForCrossing = true;
                    crossingAbilityGreenIndicator.enabled = true;

                    if (flowerInfoForPlanting.GrowingRoom == growingRoomWild)
                    {
                        crossedFlowerRoomYellowIndicator.enabled = true;
                    }
                    else if (flowerInfoForPlanting.GrowingRoom == growingRoomExotic)
                    {
                        crossedFlowerRoomBlueIndicator.enabled = true;
                    }
                    else if (flowerInfoForPlanting.GrowingRoom == growingRoomDecorative)
                    {
                        crossedFlowerRoomGreenIndicator.enabled = true;
                    }
                }
            }
        }

        public override void UpgradeTable()
        {
            base.UpgradeTable();
            SetCrossingFlowerTime();
            crossingTableBlenderMeshFilter.mesh = crossingTableBlenderLvlMeshes[tableLvl - 1];
        }

        private void SetCrossingFlowerTime()
        {
            crossingFlowerTime = tablesSettings.CrossingFlowerTime - tablesSettings.CrossingFlowerLvlTimeDelta * tableLvl;
        }

        private bool CanPlayerStartFlowersCrossing()
        {
            return !IsTableBroken && !isCrossingSeedReady && 
                   playerPickableObjectHandler.IsPickableObjectNull && 
                   isFlowerReadyForCrossing;
        }
        
        private IEnumerator StartFlowersCrossing()
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartCrossingTrigger);

            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            
            IsSeedCrossing = true;
            tableObjectsRotation.StartObjectsRotation();
            
            Observable.EveryUpdate().Subscribe( updateCrossingFlowersTimer =>
            {
                currentCrossingFlowerTime += Time.deltaTime;

                if (currentCrossingFlowerTime >= crossingFlowerTime)
                {
                    currentCrossingFlowerTime = 0;
                    IsSeedCrossing = false;
                    isCrossingSeedReady = true;
                    tableObjectsRotation.PauseObjectsRotation();
                    crossingAbilityGreenIndicator.enabled = false;
                    crossingAbilityRedIndicator.enabled = true;
                    firstFlowersCrossingTable.PotOnTable.CrossFlower();
                    secondFlowersCrossingTable.PotOnTable.CrossFlower();
                    UseBreakableTable();
                    
                    crossingFlowersTimerCompositeDisposable.Clear();
                }
            }).AddTo(crossingFlowersTimerCompositeDisposable);
        }

        private bool CanPlayerPlantCrossedSeed()
        {
            if (!IsTableBroken && isCrossingSeedReady &&
                playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                potForPlanting = currentPot;

                return potForPlanting.IsSoilInsidePot &&
                       potForPlanting.PlantedFlowerInfo.FlowerName == flowerNameEmpty &&
                       potForPlanting.GrowingRoom == flowerInfoForPlanting.GrowingRoom;
            }

            return false;
        }

        private IEnumerator PlantCrossedSeed()
        {
            potForPlanting.PutOnTableAndKeepPlayerBusy(tablePotTransform);

            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            potForPlanting.PlantSeed(flowerInfoForPlanting);
            isCrossingSeedReady = false;
            CheckCrossingAbility();

            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            potForPlanting.TakeInPlayerHandsAndSetPlayerFree();
        }

        private void FixCrossingTable()
        {
            FixBreakableFlowerTable(
                repairsAndUpgradesSettings.CrossingTableMinQuantity * (tableLvl + 1),
                repairsAndUpgradesSettings.CrossingTableMaxQuantity * (tableLvl + 1));
        }
    }
}