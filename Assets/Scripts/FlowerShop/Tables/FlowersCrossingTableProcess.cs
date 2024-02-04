using System.Collections;
using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Settings;
using FlowerShop.Tables.Abstract;
using FlowerShop.Tables.Helpers;
using PlayerControl;
using Saves;
using UniRx;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    [RequireComponent(typeof(TableObjectsRotation))]
    public class FlowersCrossingTableProcess : UpgradableBreakableTable, ISavableObject
    {
        [Inject] private readonly ReferencesForLoad referencesForLoad;
        [Inject] private readonly CyclicalSaver cyclicalSaver;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly TablesSettings tablesSettings;
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly FlowersContainer flowersContainer;
        [Inject] private readonly PlayerComponents playerComponents;

        [SerializeField] private Transform tablePotTransform;
    
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
        
        [field: SerializeField] public string UniqueKey { get; private set; }
        
        public bool IsSeedCrossing { get; private set; }

        private protected override void OnValidate()
        {
            base.OnValidate();
            
            tableObjectsRotation = GetComponent<TableObjectsRotation>();
        }

        private protected override void Awake()
        {
            base.Awake();

            Load();
        }

        private void Start()
        {
            SetCrossingFlowerTime();
            
            breakableTableBaseComponent.CheckIfTableBroken();

            if (IsSeedCrossing)
            {
                tableObjectsRotation.StartObjectsRotation();
            }
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
            else if (CanPlayerFixTable())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(FixCrossingTable);
            }
            else if (CanPlayerUpgradeTable())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(ShowUpgradeCanvas);
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
                isFlowerReadyForCrossing = true;
            }
            else
            {
                isFlowerReadyForCrossing = false;
            }

            if (isFlowerReadyForCrossing)
            {
                flowerInfoForPlanting = flowersContainer.GetFlowerFromCrossingRecipe(
                    firstFlowersCrossingTable.PotOnTable.PlantedFlowerInfo.FlowerName,
                    secondFlowersCrossingTable.PotOnTable.PlantedFlowerInfo.FlowerName);

                if (flowerInfoForPlanting.FlowerName == flowersSettings.FlowerNameEmpty)
                {
                    isFlowerReadyForCrossing = false;
                }
                else
                {
                    crossingAbilityRedIndicator.enabled = false;

                    isFlowerReadyForCrossing = true;
                    crossingAbilityGreenIndicator.enabled = true;

                    CheckFlowerInfoForPlantingRoomIndicator();
                }
            }
            else if (flowerInfoForPlanting != null && isCrossingSeedReady)
            {
                CheckFlowerInfoForPlantingRoomIndicator();
            }
        }

        public override void UpgradeTableFinish()
        {
            base.UpgradeTableFinish();
            SetCrossingFlowerTime();
            UpgradeCrossingTableBlender();
            SetActionsBeforeBrokenQuantity(
                repairsAndUpgradesSettings.FlowerGrowingTableMinQuantity * (tableLvl + 1),
                repairsAndUpgradesSettings.FlowerGrowingTableMaxQuantity * (tableLvl + 1));
            
            Save();
        }

        public void Load()
        {
            FlowersCrossingTableProcessForSaving flowersCrossingTableProcessForLoading =
                SavesHandler.Load<FlowersCrossingTableProcessForSaving>(UniqueKey);

            if (flowersCrossingTableProcessForLoading.IsValuesSaved)
            {
                tableLvl = flowersCrossingTableProcessForLoading.TableLvl;
                if (tableLvl > 0)
                {
                    UpgradeCrossingTableBlender();
                    LoadLvlMesh();
                }
                
                breakableTableBaseComponent.LoadActionsBeforeBrokenQuantity(
                    flowersCrossingTableProcessForLoading.ActionsBeforeBrokenQuantity);

                currentCrossingFlowerTime = flowersCrossingTableProcessForLoading.CurrentCrossingFlowerTime;

                if (flowersCrossingTableProcessForLoading.IsSeedCrossing)
                {
                    StartFlowersCrossingProcess();
                }
                
                isCrossingSeedReady = flowersCrossingTableProcessForLoading.IsCrossingSeedReady;

                flowerInfoForPlanting =
                    referencesForLoad.GetReference<FlowerInfo>(
                        flowersCrossingTableProcessForLoading.PlantedFlowerInfoUniqueKey);
            }
            else
            {
                ResetFlowerInfoForPlanting();
                SetActionsBeforeBrokenQuantity(
                    repairsAndUpgradesSettings.FlowerGrowingTableMinQuantity * (tableLvl + 1),
                    repairsAndUpgradesSettings.FlowerGrowingTableMaxQuantity * (tableLvl + 1));
            }
        }

        public void Save()
        {
            FlowersCrossingTableProcessForSaving flowersCrossingTableProcessForSaving = 
                new(tableLvl, breakableTableBaseComponent.ActionsBeforeBrokenQuantity, 
                    currentCrossingFlowerTime, isCrossingSeedReady, flowerInfoForPlanting.UniqueKey, IsSeedCrossing);
            
            SavesHandler.Save(UniqueKey, flowersCrossingTableProcessForSaving);
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
            
            isFlowerReadyForCrossing = false;
            tableObjectsRotation.StartObjectsRotation();
            StartFlowersCrossingProcess();
        }

        private void StartFlowersCrossingProcess()
        {
            cyclicalSaver.CyclicalSaverEvent += Save;
            
            IsSeedCrossing = true;
            firstFlowersCrossingTable.PlayCrossingFlowerEffects();
            secondFlowersCrossingTable.PlayCrossingFlowerEffects();
            
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
                    
                    firstFlowersCrossingTable.StopCrossingFlowerEffects();
                    secondFlowersCrossingTable.StopCrossingFlowerEffects();
                    
                    cyclicalSaver.CyclicalSaverEvent -= Save;
                    
                    Save();
                    
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
                       potForPlanting.PlantedFlowerInfo.FlowerName == flowersSettings.FlowerNameEmpty &&
                       potForPlanting.GrowingRoom == flowerInfoForPlanting.GrowingRoom;
            }

            return false;
        }

        private IEnumerator PlantCrossedSeed()
        {
            potForPlanting.PutOnTableAndKeepPlayerBusy(tablePotTransform);

            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            potForPlanting.PlantSeed(flowerInfoForPlanting);
            ResetFlowerInfoForPlanting();
            isCrossingSeedReady = false;
            CheckCrossingAbility();
            
            Save();

            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            potForPlanting.TakeInPlayerHandsAndSetPlayerFree();
        }

        private bool CanPlayerFixTable()
        {
            return playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer &&
                   IsTableBroken;
        }

        private void FixCrossingTable()
        {
            FixBreakableFlowerTable(
                repairsAndUpgradesSettings.CrossingTableMinQuantity * (tableLvl + 1),
                repairsAndUpgradesSettings.CrossingTableMaxQuantity * (tableLvl + 1));
            
            Save();
        }

        private bool CanPlayerUpgradeTable()
        {
            return playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer &&
                   tableLvl < repairsAndUpgradesSettings.MaxUpgradableTableLvl;
        }

        private void UpgradeCrossingTableBlender()
        {
            crossingTableBlenderMeshFilter.mesh = crossingTableBlenderLvlMeshes[tableLvl - 1];
        }

        private void CheckFlowerInfoForPlantingRoomIndicator()
        {
            crossedFlowerRoomRedIndicator.enabled = false;
            
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
            else
            {
                crossedFlowerRoomRedIndicator.enabled = true;
            }
        }

        private void ResetFlowerInfoForPlanting()
        {
            flowerInfoForPlanting = flowersSettings.FlowerInfoEmpty;
        }
    }
}