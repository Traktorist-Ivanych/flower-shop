using System.Collections;
using FlowerShop.Achievements;
using FlowerShop.Effects;
using FlowerShop.Flowers;
using FlowerShop.Help;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Settings;
using FlowerShop.Sounds;
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
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly CyclicalSaver cyclicalSaver;
        [Inject] private readonly FlowersContainer flowersContainer;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly HardworkingBreeder hardworkingBreeder;
        [Inject] private readonly HelpCanvasLiaison helpCanvasLiaison;
        [Inject] private readonly HelpTexts helpTexts;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly RareFlowersHandler rareFlowersHandler;
        [Inject] private readonly ReferencesForLoad referencesForLoad;
        [Inject] private readonly SoundsHandler soundsHandler;
        [Inject] private readonly TablesSettings tablesSettings;

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
        [SerializeField] private Mesh[] firstFlowersCrossingTableLvlMeshes = new Mesh[2];
        [SerializeField] private FlowersCrossingTable secondFlowersCrossingTable;
        [SerializeField] private Mesh[] secondFlowersCrossingTableLvlMeshes = new Mesh[2];
    
        [Header("CrossingTableBlender")]
        [SerializeField] private MeshFilter crossingTableBlenderMeshFilter;
        [SerializeField] private Mesh[] crossingTableBlenderLvlMeshes = new Mesh[2];

        [Header("GrowingRooms")] 
        [SerializeField] private GrowingRoom growingRoomWild;
        [SerializeField] private GrowingRoom growingRoomExotic;
        [SerializeField] private GrowingRoom growingRoomDecorative;

        [HideInInspector, SerializeField] private TableObjectsRotation tableObjectsRotation;
        [HideInInspector, SerializeField] private MeshFilter firstFlowersCrossingTableMeshFilter;
        [HideInInspector, SerializeField] private MeshFilter secondFlowersCrossingTableMeshFilter;
        [HideInInspector, SerializeField] private ActionProgressbar actionProgressbar;

        private Pot potForPlanting;
        private float crossingFlowerTime;
        private float currentCrossingFlowerTime;
        private bool isFlowerReadyForCrossing;
        
        private readonly CompositeDisposable crossingFlowersTimerCompositeDisposable = new();
        
        [field: SerializeField] public string UniqueKey { get; private set; }
        
        public bool IsSeedCrossing { get; private set; } 
        public bool IsCrossingSeedReady { get; private set; }
        public FlowerInfo FlowerInfoForPlanting { get; private set; }

        private protected override void OnValidate()
        {
            base.OnValidate();

            firstFlowersCrossingTableMeshFilter = firstFlowersCrossingTable.GetComponent<MeshFilter>();
            secondFlowersCrossingTableMeshFilter = secondFlowersCrossingTable.GetComponent<MeshFilter>();
            tableObjectsRotation = GetComponent<TableObjectsRotation>();
            actionProgressbar = GetComponentInChildren<ActionProgressbar>();
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
                actionProgressbar.EnableActionProgressbar(crossingFlowerTime, currentCrossingFlowerTime);
            }
        }

        private protected override void TryInteractWithTable()
        {
            base.TryInteractWithTable();

            if (!playerBusyness.IsPlayerFree)
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.PlayerBusy);
                return;
            }
            else if (IsSeedCrossing)
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.SeedCrossing);
                return;
            }
            
            if (CanPlayerStartFlowersCrossing())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(() => StartCoroutine(StartFlowersCrossing()));
            }
            else if (CanPlayerPlantCrossedSeed())
            {
                SetPlayerDestinationAndOnPlayerArriveAction( () => StartCoroutine(PlantCrossedSeed()));
            }
            else if (CanPlayerUseTableInfoCanvas())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(UseTableInfoCanvas);
            }
            else if (CanPlayerFixTable())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(FixCrossingTable);
            }
            else if (CanPlayerUpgradeTable() && !IsSeedCrossing)
            {
                SetPlayerDestinationAndOnPlayerArriveAction(ShowUpgradeCanvas);
            }
            else
            {
                TryToShowHelpCanvas();
            }
        }

        private void TryToShowHelpCanvas()
        {
            if (IsTableBroken)
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.BrokenTable);
            }
            else if (playerPickableObjectHandler.IsPickableObjectNull)
            {
                if (IsCrossingSeedReady)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.SeedAlreadyCrossed);
                }
                else if (!isFlowerReadyForCrossing)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.CrossingTableCanNotStartCrossing);
                }
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                if (!IsCrossingSeedReady)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.SeedNotCrossed);
                }
                else if (currentPot.GrowingRoom != FlowerInfoForPlanting.GrowingRoom)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.MismatchGrowingRoomForCrossingTable);
                }
                else if (!currentPot.IsSoilInsidePot)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.NoSoilInsidePot);
                }
                else if (currentPot.PlantedFlowerInfo != flowersSettings.FlowerInfoEmpty)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.FlowerAlreadyPlanted);
                }
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer)
            {
                if (tableLvl >= repairsAndUpgradesSettings.MaxUpgradableTableLvl)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.TableHasMaxLvl);
                }
            }
            else
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.WrongPickableObject);
            }
        }

        private bool CanPlayerUseTableInfoCanvas()
        {
            return !IsTableBroken && playerPickableObjectHandler.CurrentPickableObject is InfoBook;
        }

        private void UseTableInfoCanvas()
        {
            tableInfoCanvasLiaison.ShowCanvas(tableInfo, growingRoom);
        }

        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerStartFlowersCrossing() || CanPlayerPlantCrossedSeed() || CanPlayerUseTableInfoCanvas() ||
                   CanPlayerFixTable() || (CanPlayerUpgradeTableForSelectableEffect() && !IsSeedCrossing);
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
                FlowerInfo currentFlowerInfo = flowersContainer.GetFlowerFromCrossingRecipe(
                    firstFlowersCrossingTable.PotOnTable.PlantedFlowerInfo.FlowerName,
                    secondFlowersCrossingTable.PotOnTable.PlantedFlowerInfo.FlowerName);

                if (currentFlowerInfo.FlowerName == flowersSettings.FlowerNameEmpty)
                {
                    isFlowerReadyForCrossing = false;
                }
                else
                {
                    crossingAbilityRedIndicator.enabled = false;
                    crossingAbilityGreenIndicator.enabled = true;

                    if (!IsCrossingSeedReady)
                    {
                        FlowerInfoForPlanting = currentFlowerInfo;
                    }

                    CheckFlowerInfoForPlantingRoomIndicator();
                }
            }
            else if (FlowerInfoForPlanting != null && IsCrossingSeedReady)
            {
                CheckFlowerInfoForPlantingRoomIndicator();
            }
        }

        public override void UpgradeTableFinish()
        {
            base.UpgradeTableFinish();
            SetCrossingFlowerTime();
            UpgradeCrossingTableObjects();
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
                    UpgradeCrossingTableObjects();
                    LoadLvlMesh();
                }
                
                breakableTableBaseComponent.LoadActionsBeforeBrokenQuantity(
                    flowersCrossingTableProcessForLoading.ActionsBeforeBrokenQuantity);

                currentCrossingFlowerTime = flowersCrossingTableProcessForLoading.CurrentCrossingFlowerTime;

                if (flowersCrossingTableProcessForLoading.IsSeedCrossing)
                {
                    StartFlowersCrossingProcess();
                }
                
                IsCrossingSeedReady = flowersCrossingTableProcessForLoading.IsCrossingSeedReady;

                FlowerInfoForPlanting =
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
                    currentCrossingFlowerTime, IsCrossingSeedReady, FlowerInfoForPlanting.UniqueKey, IsSeedCrossing);
            
            SavesHandler.Save(UniqueKey, flowersCrossingTableProcessForSaving);
        }

        private void SetCrossingFlowerTime()
        {
            crossingFlowerTime = tablesSettings.CrossingFlowerTime - tablesSettings.CrossingFlowerLvlTimeDelta * tableLvl;
        }

        private bool CanPlayerStartFlowersCrossing()
        {
            return !IsTableBroken && !IsCrossingSeedReady && !IsSeedCrossing &&
                   playerPickableObjectHandler.IsPickableObjectNull && 
                   isFlowerReadyForCrossing;
        }
        
        private IEnumerator StartFlowersCrossing()
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartCrossingTrigger);

            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            
            playerBusyness.SetPlayerFree();
            isFlowerReadyForCrossing = false;
            tableObjectsRotation.StartObjectsRotation();
            selectedTableEffect.ActivateEffectWithDelay();
            StartFlowersCrossingProcess();
            actionProgressbar.EnableActionProgressbar(crossingFlowerTime, currentCrossingFlowerTime);

            educationHandler.TryBrokenSoilPreparationTableDuringEducation();
        }

        private void StartFlowersCrossingProcess()
        {
            cyclicalSaver.CyclicalSaverEvent += Save;
            
            IsSeedCrossing = true;
            firstFlowersCrossingTable.PlayCrossingFlowerEffects();
            secondFlowersCrossingTable.PlayCrossingFlowerEffects();
            soundsHandler.StartPlayingCrossingSound();

            Observable.EveryUpdate().Subscribe( updateCrossingFlowersTimer =>
            {
                
                currentCrossingFlowerTime += Time.deltaTime;

                if (currentCrossingFlowerTime >= crossingFlowerTime)
                {
                    currentCrossingFlowerTime = 0;
                    IsSeedCrossing = false;
                    IsCrossingSeedReady = true;
                    tableObjectsRotation.PauseObjectsRotation();
                    crossingAbilityGreenIndicator.enabled = false;
                    crossingAbilityRedIndicator.enabled = true;
                    firstFlowersCrossingTable.PotOnTable.CrossFlower();
                    secondFlowersCrossingTable.PotOnTable.CrossFlower();
                    UseBreakableTable();
                    
                    firstFlowersCrossingTable.StopCrossingFlowerEffects();
                    secondFlowersCrossingTable.StopCrossingFlowerEffects();
                    soundsHandler.StopPlayingCrossingSound();
                    
                    selectedTableEffect.TryToRecalculateEffect();
                    
                    hardworkingBreeder.IncreaseProgress();
                    
                    cyclicalSaver.CyclicalSaverEvent -= Save;
                    
                    Save();
                    
                    crossingFlowersTimerCompositeDisposable.Clear();
                }
            }).AddTo(crossingFlowersTimerCompositeDisposable);
        }

        private bool CanPlayerPlantCrossedSeed()
        {
            if (!IsTableBroken && IsCrossingSeedReady &&
                playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                potForPlanting = currentPot;

                return potForPlanting.IsSoilInsidePot &&
                       potForPlanting.PlantedFlowerInfo.FlowerName == flowersSettings.FlowerNameEmpty &&
                       potForPlanting.GrowingRoom == FlowerInfoForPlanting.GrowingRoom;
            }

            return false;
        }

        private IEnumerator PlantCrossedSeed()
        {
            potForPlanting.PutOnTableAndKeepPlayerBusy(tablePotTransform);

            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            if (!educationHandler.IsEducationActive && rareFlowersHandler.IsRareFlowerResultOfCrossing())
            {
                potForPlanting.PlantSeed(rareFlowersHandler.GetRareFlower());
            }
            else
            {
                potForPlanting.PlantSeed(FlowerInfoForPlanting);
                flowersContainer.TryToAddAvailableFlowerInfo(FlowerInfoForPlanting);
            }
            ResetFlowerInfoForPlanting();
            IsCrossingSeedReady = false;
            CheckCrossingAbility();
            educationHandler.TrySetNextStepByPlantingCrossedSeed();
            
            Save();

            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            potForPlanting.TakeInPlayerHandsAndSetPlayerFree();
        }

        private void FixCrossingTable()
        {
            FixBreakableFlowerTable(
                repairsAndUpgradesSettings.CrossingTableMinQuantity * (tableLvl + 1),
                repairsAndUpgradesSettings.CrossingTableMaxQuantity * (tableLvl + 1));
        }

        private void UpgradeCrossingTableObjects()
        {
            crossingTableBlenderMeshFilter.mesh = crossingTableBlenderLvlMeshes[tableLvl - 1];
            firstFlowersCrossingTableMeshFilter.mesh = firstFlowersCrossingTableLvlMeshes[tableLvl - 1];
            secondFlowersCrossingTableMeshFilter.mesh = secondFlowersCrossingTableLvlMeshes[tableLvl - 1];   
        }

        private void CheckFlowerInfoForPlantingRoomIndicator()
        {
            crossedFlowerRoomRedIndicator.enabled = false;
            
            if (FlowerInfoForPlanting.GrowingRoom == growingRoomWild)
            {
                crossedFlowerRoomYellowIndicator.enabled = true;
            }
            else if (FlowerInfoForPlanting.GrowingRoom == growingRoomExotic)
            {
                crossedFlowerRoomBlueIndicator.enabled = true;
            }
            else if (FlowerInfoForPlanting.GrowingRoom == growingRoomDecorative)
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
            FlowerInfoForPlanting = flowersSettings.FlowerInfoEmpty;
        }
    }
}