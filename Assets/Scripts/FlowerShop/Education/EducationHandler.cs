using System.Collections;
using System.Collections.Generic;
using FlowerShop.Achievements;
using FlowerShop.Customers.VipAndComplaints;
using FlowerShop.Effects;
using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Settings;
using FlowerShop.Tables;
using Input;
using PlayerControl;
using Saves;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

namespace FlowerShop.Education
{
    public class EducationHandler : MonoBehaviour, ISavableObject
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly CanvasPointer canvasPointer;
        [Inject] private readonly EducationCanvasLiaison educationCanvasLiaison;
        [Inject] private readonly EducationSettings educationSettings;
        [Inject] private readonly ExemplaryStudent exemplaryStudent;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly PlayerInputActions playerInputActions;
        [Inject] private readonly SelectedTableEffect selectedTableEffect;
        [Inject] private readonly VipOrdersHandler vipOrdersHandler;

        [Header("Canvas Scalers")]
        [SerializeField] private Vector3 overrideEndScaleForFertilizersInfoButton;
        [SerializeField] private Vector3 overrideEndScaleForFlowerInfoButton;
        [SerializeField] private Vector3 overrideEndScaleForSeedButton;
        [SerializeField] private Vector3 overrideEndScaleForImprovementButton;
        [SerializeField] private CanvasElementScaler flowersCanvasButton;
        [SerializeField] private CanvasElementScaler cactusSeedButton;
        [SerializeField] private CanvasElementScaler dandelionSeedButton;
        [SerializeField] private CanvasElementScaler growthAcceleratorButton;
        [SerializeField] private CanvasElementScaler growingLvlIncreaserButton;
        [SerializeField] private CanvasElementScaler growerToMaxLvlButton;
        [SerializeField] private CanvasElementScaler improvementButton;
        [SerializeField] private CanvasElementScaler growerToMaxLvlInfoButton;
        [SerializeField] private CanvasElementScaler closeFertilizersInfoPanelButton;
        [SerializeField] private CanvasElementScaler vipOrderPageButton;
        [SerializeField] private CanvasElementScaler flowerInfoCanvasOpenButton;
        [SerializeField] private CanvasElementScaler closeVipPageCanvasButton;
        [SerializeField] private CanvasElementScaler closeComplaintPageCanvasButton;
        [SerializeField] private CanvasElementScaler closeStatisticsPageCanvasButton;
        [SerializeField] private CanvasElementScaler closeAchievementsPageCanvasButton;
        [SerializeField] private CanvasElementScaler closeComputerCanvasButton;
        [SerializeField] private CanvasElementScaler ratingPanel;
        [SerializeField] private CanvasElementScaler settingsButton;
        [SerializeField] private CanvasElementScaler enableHelpButton;
        [SerializeField] private CanvasElementScaler closeSettingsCanvasButton;
        [SerializeField] private CanvasElementScaler closeFlowerInfoCanvasButton;

        [Header("Scene Objects")]
        [SerializeField] private FlowerInfo educationVipOrderFlowerInfo;
        [SerializeField] private SoilPreparationTable soilPreparationTable;
        [SerializeField] private FlowersCrossingTableProcess flowersCrossingTableProcess;
        [SerializeField] private Pot potForPlantWeed;
        [SerializeField] private Pot potForPouring;

        [Header("Transforms For CamvasPointer")]
        [SerializeField] private Transform potsRackTransform;
        [SerializeField] private Transform soilPreparationTableTransform;
        [SerializeField] private Transform seedsTableTransform;
        [SerializeField] private Transform firstGrowingTableTransform;
        [SerializeField] private Transform secondGrowingTableTransform;
        [SerializeField] private Transform thirdGrowingTableTransform;
        [SerializeField] private Transform fourthGrowingTableTransform;
        [SerializeField] private Transform fertilizersTableTransform;
        [SerializeField] private Transform firstCrossingTableTransform;
        [SerializeField] private Transform secondCrossingTableTransform;
        [SerializeField] private Transform crossingTableProcessTransform;
        [SerializeField] private Transform hammerTableTransform;
        [SerializeField] private Transform weedingTableTransform;
        [SerializeField] private Transform collectionTransform;
        [SerializeField] private Transform saleTableTransform;
        [SerializeField] private Transform coffeeTableTransform;
        [SerializeField] private Transform computerTableTransform;
        [SerializeField] private Transform wateringTableTransform;
        [SerializeField] private Transform vipOrdersTableTransform;
        [SerializeField] private Transform customersAccessTransform;
        [SerializeField] private Transform infoTableTransform;
        [SerializeField] private Transform storageTableTransform;

        [Header("Education Sequence")]
        [SerializeField] private List<MonoBehaviour> educationSequence = new();
        
        private int step;
        private bool isFirstLvlClueWasDisplayed;
        private bool isSecondLvlClueWasDisplayed;
        private bool isThirdLvlClueWasDisplayed;

        [field: HideInInspector, SerializeField] public bool IsEducationActive { get; private set; }
        [field: SerializeField] public string UniqueKey { get; private set; }

        public int Step { get => step; }

        private void Awake()
        {
            Load();
        }

        private void Start()
        {
            if (IsEducationActive && step > 0)
            {
                step--;
                if (step == 53)
                {
                    TryBrokenSoilPreparationTableDuringEducation();
                }
                else if (step == 73)
                {
                    TrySetNextStepByPlantingCrossedSeed();
                }
                else
                {
                    CompleteEducationStep();
                    ActivateEffect();
                }
            }
        }

        public bool IsMonoBehaviourCurrentEducationStep(MonoBehaviour verifiableMonoBehaviour)
        {
            if (!IsEducationActive)
            {
                return false;
            }
            return educationSequence[step].Equals(verifiableMonoBehaviour);
        }

        public void TryToDisplayFirstLvlClue()
        {
            if (!isFirstLvlClueWasDisplayed)
            {
                isFirstLvlClueWasDisplayed = true;
                ShowEducationDescription(educationSettings.FirstLvlClue);
                Save();
            }
        }

        public void TryToDisplaySecondLvlClue()
        {
            if (!isSecondLvlClueWasDisplayed)
            {
                isSecondLvlClueWasDisplayed = true;
                ShowEducationDescription(educationSettings.SecondLvlClue);
                Save();
            }
        }

        public void TryToDisplayThirdLvlClue()
        {
            if (!isThirdLvlClueWasDisplayed)
            {
                isThirdLvlClueWasDisplayed = true;
                ShowEducationDescription(educationSettings.ThirdLvlClue);
                Save();
            }
        }

        public void Load()
        {
            EducationHandlerForSaving educationHandlerForLoading =
                SavesHandler.Load<EducationHandlerForSaving>(UniqueKey);

            if (educationHandlerForLoading.IsValuesSaved)
            {
                IsEducationActive = educationHandlerForLoading.IsEducationActive;
                step = educationHandlerForLoading.Step;
                isFirstLvlClueWasDisplayed = educationHandlerForLoading.IsFirstLvlClueWasDisplayed;
                isSecondLvlClueWasDisplayed = educationHandlerForLoading.IsSecondLvlClueWasDisplayed;
                isThirdLvlClueWasDisplayed = educationHandlerForLoading.IsThirdLvlClueWasDisplayed;
            }
            else
            {
                CompleteFirstStep();
            }
        }

        public void Save()
        {
            EducationHandlerForSaving educationHandlerForSaving = 
                new(IsEducationActive, step, isFirstLvlClueWasDisplayed, isSecondLvlClueWasDisplayed, isThirdLvlClueWasDisplayed);
            
            SavesHandler.Save(UniqueKey, educationHandlerForSaving);
        }

        public void TryBrokenSoilPreparationTableDuringEducation()
        {
            if (IsEducationActive && step == 53)
            {
                if (!soilPreparationTable.IsTableBroken)
                {
                    soilPreparationTable.BrokenTable();
                }
                ShowEducationDescriptionAsStep(
                    educationSettings.BrokenTableDescriptionText);
                canvasPointer.DisableCanvasPointer();
            }
        }

        public void TrySetNextStepByPlantingCrossedSeed()
        {
            if (IsEducationActive && step == 73)
            {
                canvasPointer.EnableCanvasPointer(firstGrowingTableTransform);
                SetNextStep();
                ActivateEffect();
                Save();
            }
        }

        public void CompleteEducationStep()
        {
            switch (step)
            {
                case 0:
                    ShowEducationDescriptionAsStep(educationSettings.FirstDescriptionText);
                    break;
                case 1:
                    ShowEducationDescriptionAsStep(educationSettings.MoneyAndShopRatingDescriptionText);
                    playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StopWelcome);
                    break;
                case 2:
                    ShowEducationDescriptionAsStep(educationSettings.FlowersCanvasDescriptionText);
                    break;
                case 3:
                    educationCanvasLiaison.EducationCanvas.enabled = false;
                    playerInputActions.EnableCanvasControlMode();
                    flowersCanvasButton.ActivateEffect();
                    SetNextStep();
                    Save();
                    break;
                case 4: // Open FlowersCanvas
                    flowersCanvasButton.DeactivateEffect();
                    SetNextStep();
                    break;
                case 5: // Close FlowersCanvas
                    ShowEducationDescriptionAsStep(educationSettings.PotsRackDescriptionText);
                    break;
                case 6:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(potsRackTransform);
                    break;
                case 7: // Click on PotsRack
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 8: // Take Pot from PotsRack
                    ShowEducationDescriptionAsStep(educationSettings.SoilPreparationDescriptionText);
                    break;
                case 9:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(soilPreparationTableTransform);
                    break;
                case 10: // Put Pot on SoilPreparationTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 11: // Take Pot with soil in PlayerHands
                    ShowEducationDescriptionAsStep(educationSettings.SeedPlantDescriptionText);
                    break;
                case 12:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(seedsTableTransform);
                    break;
                case 13: // Open SeedCanvas
                    cactusSeedButton.ActivateEffect(overrideEndScaleForSeedButton);
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 14: // Plant Cactus seed
                    cactusSeedButton.DeactivateEffect();
                    SetNextStep();
                    Save();
                    break;
                case 15:
                    ShowEducationDescriptionAsStep(educationSettings.GrowingTableDescriptionText);
                    break;
                case 16:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(firstGrowingTableTransform);
                    break;
                case 17: // Click on GrowingTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 18: // Put Pot on GrowingTable
                    ShowEducationDescriptionAsStep(educationSettings.PlantNextFlowerDescriptionText);
                    break;
                case 19:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(potsRackTransform);
                    break;
                case 20: // Click on PatsRack
                    SetNextStep();
                    break;
                case 21: // Take Pot from PotsRack
                    SetNextStep();
                    ActivateEffect();
                    canvasPointer.EnableCanvasPointer(soilPreparationTableTransform);
                    Save();
                    break;
                case 22: // Click on SoilPreparationTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 23: // Take Pot with Soil in PlayerHands
                    canvasPointer.EnableCanvasPointer(seedsTableTransform);
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 24: // Click on SeedTable
                    dandelionSeedButton.ActivateEffect(overrideEndScaleForSeedButton);
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 25: // Plant Dandelion
                    dandelionSeedButton.DeactivateEffect();
                    SetNextStep();
                    Save();
                    break;
                case 26: // Take planted Dandelion in  PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    canvasPointer.EnableCanvasPointer(secondGrowingTableTransform);
                    Save();
                    break;
                case 27: // Click on GrowingTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 28: // Put Pot on GrowingTable
                    ShowEducationDescriptionAsStep(educationSettings.FertilizersDescriptionText);
                    break;
                case 29: // Put Pot on GrowingTable
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(fertilizersTableTransform);
                    break;
                case 30: // Click on FertilizersTable
                    canvasPointer.DisableCanvasPointer();
                    growerToMaxLvlInfoButton.ActivateEffect(overrideEndScaleForFertilizersInfoButton);
                    SetNextStep();
                    break;
                case 31: // Click on GrowerToMaxLvl Info Button
                    growerToMaxLvlInfoButton.DeactivateEffect();
                    SetNextStep();
                    break;
                case 32: // Click on Close Info Panel
                    growerToMaxLvlButton.ActivateEffect();
                    SetNextStep();
                    break;
                case 33: // Click on GrowerToMaxLvl
                    SetNextStep();
                    growerToMaxLvlButton.DeactivateEffect();
                    break;
                case 34: // Take GrowerToMaxLvl in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    canvasPointer.EnableCanvasPointer(firstGrowingTableTransform);
                    Save();
                    break;
                case 35: // Click on Fist GrowingTable with GrowerToMaxLvl in PlayerHands
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 36: // Treat Pot on Fist GrowingTable
                    canvasPointer.EnableCanvasPointer(secondGrowingTableTransform);
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 37: // Click on Second GrowingTable with GrowerToMaxLvl in PlayerHands
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 38: // Treat Pot on Second GrowingTable
                    ShowEducationDescriptionAsStep(educationSettings.FertilizersEndUsingDescriptionText);
                    break;
                case 39:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(fertilizersTableTransform);
                    break;
                case 40: // Click on FertilizersTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 41: // Put Fertilizer on Table
                    ShowEducationDescriptionAsStep(educationSettings.CrossingTableDescriptionText);
                    break;
                case 42:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(firstGrowingTableTransform);
                    break;
                case 43: // Click on Fist GrowingTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 44: // Take Pot in Player Hands
                    SetNextStep();
                    ActivateEffect();
                    canvasPointer.EnableCanvasPointer(firstCrossingTableTransform);
                    Save();
                    break;
                case 45: // Click on First Crossing Table
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 46: // Put Pot on First Crossing Table
                    SetNextStep();
                    ActivateEffect();
                    canvasPointer.EnableCanvasPointer(secondGrowingTableTransform);
                    Save();
                    break;
                case 47: // Click on Second GrowingTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 48: // Take Pot in Player Hands
                    SetNextStep();
                    ActivateEffect();
                    canvasPointer.EnableCanvasPointer(secondCrossingTableTransform);
                    Save();
                    break;
                case 49: // Click on Second Crossing Table
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 50: // Put Pot on Second Crossing Table
                    ShowEducationDescriptionAsStep(educationSettings.CrossingTableProcessDescriptionText);
                    break;
                case 51:
                    ShowEducationDescriptionAsStep(educationSettings.CrossingTableProcessStartDescriptionText);
                    break;
                case 52:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(crossingTableProcessTransform);
                    break;
                case 53: // Click on CrossingTableProcess
                    // Cause by CrossingTableProcess
                    break;
                case 54:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(hammerTableTransform);
                    break;
                case 55: // Click on RepairAndImprovementTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 56: // Take Hammer in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    canvasPointer.EnableCanvasPointer(soilPreparationTableTransform);
                    Save();
                    break;
                case 57: // Click on SoilPreparationTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 58: // Repair SoilPreparationTable
                    ShowEducationDescriptionAsStep(educationSettings.UpgradeTableDescriptionText);
                    break;
                case 59:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(firstGrowingTableTransform);
                    break;
                case 60: // Click on Growing Table with Hammer in PlayerHands
                    canvasPointer.DisableCanvasPointer();
                    improvementButton.ActivateEffect(overrideEndScaleForImprovementButton);
                    SetNextStep();
                    break;
                case 61: // Click on Upgrade Button
                    improvementButton.DeactivateEffect();
                    SetNextStep();
                    break;
                case 62:
                    ShowEducationDescriptionAsStep(educationSettings.CompleteUpgradingTableDescriptionText);
                    break;
                case 63:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(hammerTableTransform);
                    break;
                case 64: // Click on RepairAndImprovementTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 65: // Put Hammer on RepairAndImprovementTable
                    ShowEducationDescriptionAsStep(educationSettings.SoilForNewSeedDescriptionText);
                    break;
                case 66:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(potsRackTransform);
                    break;
                case 67: // Click on PotsRack
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 68: // Take Pot in PlayerHands
                    canvasPointer.EnableCanvasPointer(soilPreparationTableTransform);
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 69: // Click on SoilPreparationTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 70: // Take Pot in PlayerHands
                    if (flowersCrossingTableProcess.IsSeedCrossing)
                    {
                        ShowEducationDescriptionAsStep(educationSettings.WaitThenPlantNewSeedDescriptionText);
                    }
                    else
                    {
                        ShowEducationDescriptionAsStep(educationSettings.PlantNewSeedDescriptionText);
                    }
                    break;
                case 71:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(crossingTableProcessTransform);
                    break;
                case 72: // Click on FlowersCrossingTableProcess
                    if (!flowersCrossingTableProcess.IsSeedCrossing)
                    {
                        canvasPointer.DisableCanvasPointer();
                        SetNextStep();
                    }
                    break;
                case 73: // Take Pot with new seed in PlayerHands
                    // Cause by CrossingTableProcess
                    break;
                case 74: // Click on FlowerGrowingTable
                    SetNextStep();
                    canvasPointer.DisableCanvasPointer();
                    break;
                case 75: // Put Pot with new seed on GrowingTable
                    ShowEducationDescriptionAsStep(educationSettings.GrowthAcceleratorUsingDescriptionText);
                    break;
                case 76:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(fertilizersTableTransform);
                    break;
                case 77: // Click on FertilizersTable
                    canvasPointer.DisableCanvasPointer();
                    growthAcceleratorButton.ActivateEffect();
                    SetNextStep();
                    break;
                case 78: // Click on GrowthAccelerator Button
                    growthAcceleratorButton.DeactivateEffect();
                    SetNextStep();
                    break;
                case 79: // Take GrowthAccelerator in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    canvasPointer.EnableCanvasPointer(firstGrowingTableTransform);
                    Save();
                    break;
                case 80: // Click on GrowingTable with GrowthAccelerator in PlayerHands
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 81: // Treat Pot by GrowthAccelerator
                    ShowEducationDescriptionAsStep(educationSettings.GrowthAcceleratorEndUsingDescriptionText);
                    break;
                case 82:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(fertilizersTableTransform);
                    break;
                case 83: // Click on FertilizersTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 84: // Put GrowthAccelerator on FertilizersTable
                    ShowEducationDescriptionAsStep(educationSettings.TakePotFromCrossingTableDescriptionText);
                    break;
                case 85:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(firstCrossingTableTransform);
                    break;
                case 86: // Click on First CrossingTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 87: // Take Pot in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    canvasPointer.EnableCanvasPointer(thirdGrowingTableTransform);
                    Save();
                    break;
                case 88: // Click on GrowingTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 89: // Put Pot on GrowingTable
                    canvasPointer.EnableCanvasPointer(secondCrossingTableTransform);
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 90: // Click on Second CrossingTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 91: // Take Pot in PlayerHands
                    canvasPointer.EnableCanvasPointer(fourthGrowingTableTransform);
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 92: // Click on GrowingTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 93: // Put Pot on GrowingTable and plant Weed
                    potForPlantWeed.PlantWeed();
                    ShowEducationDescriptionAsStep(educationSettings.WeedDescriptionText);
                    break;
                case 94:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(weedingTableTransform);
                    break;
                case 95: // Click on HoeTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 96: // Take Hoe in PlayerHands
                    canvasPointer.EnableCanvasPointer(thirdGrowingTableTransform);
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 97: // Click on GrowingTable with Weed in Pot
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 98: // Delete Weed from Pot
                    canvasPointer.EnableCanvasPointer(weedingTableTransform);
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 99: // Click on HoeTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 100: // Pot Hoe on Table
                    ShowEducationDescriptionAsStep(educationSettings.PourPotFirstTimeDescriptionText);
                    break;
                case 101:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(wateringTableTransform);
                    break;
                case 102: // Click on WateringTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 103: // Take WateringCan in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    canvasPointer.EnableCanvasPointer(firstGrowingTableTransform);
                    Save();
                    break;
                case 104: // Click on GrowingTable with Pot that need Water
                    if (potForPouring.IsFlowerNeedWater)
                    {
                        canvasPointer.DisableCanvasPointer();
                        SetNextStep();
                    }
                    break;
                case 105: // Pour Pot
                    SetNextStep();
                    ActivateEffect();
                    canvasPointer.EnableCanvasPointer(wateringTableTransform);
                    Save();
                    break;
                case 106: // Click on WateringTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 107: // Put WateringCan on WateringTable
                    ShowEducationDescriptionAsStep(educationSettings.GrowingLvlIncreaserDescriptionText);
                    break;
                case 108:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(fertilizersTableTransform);
                    break;
                case 109: // Click on FertilizersTable
                    canvasPointer.DisableCanvasPointer();
                    growingLvlIncreaserButton.ActivateEffect();
                    SetNextStep();
                    break;
                case 110: // Click on GrowingLvlIncreaser Button
                    growingLvlIncreaserButton.DeactivateEffect();
                    SetNextStep();
                    break;
                case 111: // Take GrowingLvlIncreaser in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    canvasPointer.EnableCanvasPointer(thirdGrowingTableTransform);
                    Save();
                    break;
                case 112: // Click on First GrowingTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 113: // Use GrowingLvlIncreaser on First GrowingTable
                    canvasPointer.EnableCanvasPointer(fourthGrowingTableTransform);
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 114: // Click on Second GrowingTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 115: // Use GrowingLvlIncreaser on Second GrowingTable
                    canvasPointer.EnableCanvasPointer(fertilizersTableTransform);
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 116: // Click on FertilizersTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 117: // Put GrowingLvlIncreaser on FertilizersTable
                    ShowEducationDescriptionAsStep(educationSettings.PourPotSecondTimeDescriptionText);
                    break;
                case 118:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(wateringTableTransform);
                    break;
                case 119: // Click on WateringTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 120: // Take WateringCan in PlayerHands
                    canvasPointer.EnableCanvasPointer(firstGrowingTableTransform);
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 121: // Click on GrowingTable with Pot to Pour
                    if (potForPouring.IsFlowerNeedWater)
                    {
                        canvasPointer.DisableCanvasPointer();
                        SetNextStep();
                    }
                    break;
                case 122: // PourPot on GrowingTable
                    canvasPointer.EnableCanvasPointer(wateringTableTransform);
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 123: // Click on WateringTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 124: // Put WateringCan on WateringTable
                    ShowEducationDescriptionAsStep(educationSettings.CollectionRoomDescriptionText);
                    break;
                case 125:
                    ShowEducationDescriptionAsStep(educationSettings.PutPotInCollectionRoomDescriptionText);
                    break;
                case 126:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(fourthGrowingTableTransform);
                    break;
                case 127: // Click on GrowingTable with Pot for Collection
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 128: // Take Pot in PlayerHands
                    canvasPointer.EnableCanvasPointer(collectionTransform);
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 129: // Click on TableForCollection
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 130: // Put Pot on TableForCollection
                    ShowEducationDescriptionAsStep(educationSettings.StorageTableRoomDescriptionText);
                    break;
                case 131:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(storageTableTransform);
                    break;
                case 132: // Click on StorageTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 133: // Put Pot on StorageTable
                    ShowEducationDescriptionAsStep(educationSettings.SalesDescriptionText);
                    break;
                case 134:
                    ShowEducationDescriptionAsStep(educationSettings.PutPotOnSaleTableDescriptionText);
                    break;
                case 135:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(thirdGrowingTableTransform);
                    break;
                case 136: // Click on GrowingTable with Pot for Sale
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 137: // Take Pot in PlayerHands
                    canvasPointer.EnableCanvasPointer(saleTableTransform);
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 138: // Click on SaleTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 139: // Put Pot on SaleTable
                    ShowEducationDescriptionAsStep(educationSettings.PutPotOnPotsRackDescriptionText);
                    break;
                case 140:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(potsRackTransform);
                    break;
                case 141: // Click on PotsRack
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 142: // Put Pot on PotsRack
                    ShowEducationDescriptionAsStep(educationSettings.PourPotLastTimeDescriptionText);
                    break;
                case 143:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(wateringTableTransform);
                    break;
                case 144: // Click on WateringTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 145: // Take WateringCan in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    canvasPointer.EnableCanvasPointer(firstGrowingTableTransform);
                    Save();
                    break;
                case 146: // Click on GrowingTable
                    if (potForPouring.IsFlowerNeedWater)
                    {
                        canvasPointer.DisableCanvasPointer();
                        SetNextStep();
                    }
                    break;
                case 147: // Pour Pot
                    canvasPointer.EnableCanvasPointer(wateringTableTransform);
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 148: // Click on WateringTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 149: // Put WateringCan on WateringTable
                    ShowEducationDescriptionAsStep(educationSettings.ComputerTableDescriptionText);
                    break;
                case 150:
                    ShowEducationDescriptionAsStep(educationSettings.ComplaintsDescriptionText);
                    break;
                case 151:
                    ShowEducationDescriptionAsStep(educationSettings.VipDescriptionText);
                    break;
                case 152:
                    vipOrdersHandler.SetEducationVipOrder(educationVipOrderFlowerInfo);
                    ShowEducationDescriptionAsStep(educationSettings.VipOrderDescriptionText);
                    break;
                case 153:
                    canvasPointer.EnableCanvasPointer(computerTableTransform);
                    HideEducationDescriptionAsStep();
                    break;
                case 154: // Click on ComputerTable
                    canvasPointer.DisableCanvasPointer();
                    vipOrderPageButton.ActivateEffect(overrideEndScaleForSeedButton);
                    SetNextStep();
                    break;
                case 155: // Click on Vip Order Canvas
                    flowerInfoCanvasOpenButton.ActivateEffect(overrideEndScaleForFlowerInfoButton);
                    SetNextStep();
                    break;
                case 156: // Click on Flower Info Button
                    vipOrderPageButton.DeactivateEffect();
                    flowerInfoCanvasOpenButton.DeactivateEffect();
                    SetNextStep();
                    break;
                case 157: // Click on Close Flower Info Canvas
                    closeVipPageCanvasButton.ActivateEffect();
                    closeComputerCanvasButton.ActivateEffect();
                    closeComplaintPageCanvasButton.ActivateEffect();
                    closeStatisticsPageCanvasButton.ActivateEffect();
                    closeAchievementsPageCanvasButton.ActivateEffect();
                    closeFlowerInfoCanvasButton.ActivateEffect();
                    SetNextStep();
                    break;
                case 158: // Click on Close Vip Order Button
                    SetNextStep();
                    break;
                case 159: // Click on Close Copmuter Main Page
                    closeComputerCanvasButton.DeactivateEffect();
                    closeVipPageCanvasButton.DeactivateEffect();
                    closeComplaintPageCanvasButton.DeactivateEffect();
                    closeStatisticsPageCanvasButton.DeactivateEffect();
                    closeAchievementsPageCanvasButton.DeactivateEffect();
                    closeFlowerInfoCanvasButton.DeactivateEffect();
                    canvasPointer.EnableCanvasPointer(firstGrowingTableTransform);
                    ActivateEffect();
                    SetNextStep();
                    break;
                case 160: // Click on GrowingTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 161: // Take Pot in Player Hands
                    canvasPointer.EnableCanvasPointer(vipOrdersTableTransform);
                    ActivateEffect();
                    SetNextStep();
                    Save();
                    break;
                case 162: // Click on Vip Order Table
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 163: // Put Pot on Vip Order Table
                    canvasPointer.EnableCanvasPointer(potsRackTransform);
                    ActivateEffect();
                    SetNextStep();
                    Save();
                    break;
                case 164: // Click on PotsRack
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 165: // Put Pot in PotsRack
                    ShowEducationDescriptionAsStep(educationSettings.VipOrderEnableDescriptionText);
                    break;
                case 166:
                    ShowEducationDescriptionAsStep(educationSettings.AchievementsText);
                    break;
                case 167:
                    ShowEducationDescriptionAsStep(educationSettings.MusicDescriptionText);
                    break;
                case 168:
                    ShowEducationDescriptionAsStep(educationSettings.InfoTableDescriptionText);
                    break;
                case 169:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(infoTableTransform);
                    break;
                case 170: // Click on InfoTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 171: // Take InfoBook in Player Hands
                    canvasPointer.EnableCanvasPointer(customersAccessTransform);
                    ActivateEffect();
                    SetNextStep();
                    Save();
                    break;
                case 172: // Click on CustomersAccessTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 173: // Close CustomersAccessTable Info Canvas
                    ShowEducationDescriptionAsStep(educationSettings.RatingPanelDescriptionText);
                    break;
                case 174:
                    ratingPanel.ActivateEffect();
                    HideEducationDescriptionAsStep();
                    break;
                case 175: // Click on Rating Panel
                    ratingPanel.DeactivateEffect();
                    SetNextStep();
                    break;
                case 176: // Close Rating Panel Help
                    canvasPointer.EnableCanvasPointer(infoTableTransform);
                    ActivateEffect();
                    SetNextStep();
                    break;
                case 177: // Click on InfoTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 178: // Put InfoBook on Table
                    ShowEducationDescriptionAsStep(educationSettings.EnableHelpDescriptionText);
                    break;
                case 179:
                    settingsButton.ActivateEffect();
                    HideEducationDescriptionAsStep();
                    break;
                case 180: // Click on Settings Button
                    settingsButton.DeactivateEffect();
                    enableHelpButton.ActivateEffect(overrideEndScaleForSeedButton);
                    SetNextStep();
                    break;
                case 181: // Click on EnableHelpButton
                    enableHelpButton.DeactivateEffect();
                    closeSettingsCanvasButton.ActivateEffect();
                    SetNextStep();
                    break;
                case 182: // Close SettingsCanvas
                    closeSettingsCanvasButton.DeactivateEffect();
                    ShowEducationDescriptionAsStep(educationSettings.CoffeeDescriptionText);
                    break;
                case 183:
                    HideEducationDescriptionAsStep();
                    canvasPointer.EnableCanvasPointer(coffeeTableTransform);
                    break;
                case 184: // Click on CoffeTable
                    canvasPointer.DisableCanvasPointer();
                    SetNextStep();
                    break;
                case 185:
                    ShowEducationDescriptionAsStep(educationSettings.EducationEndDescriptionText);
                    break;
                case 186:
                    playerInputActions.DisableCanvasControlMode();
                    educationCanvasLiaison.EducationCanvas.enabled = false;
                    IsEducationActive = false;
                    ActivateEffect();
                    exemplaryStudent.IncreaseProgress();
                    Save();
                    break;
            }
        }

        private void SetNextStep()
        {
            step++;
        }

        private void CompleteFirstStep()
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartWelcome);
            IsEducationActive = true;
            playerInputActions.EnableCanvasControlMode();
            educationCanvasLiaison.EducationCanvas.enabled = true;
            educationCanvasLiaison.SetEducationText(
                localizedText: educationSettings.WelcomeText);
        }

        private void ShowEducationDescriptionAsStep(LocalizedString educationText)
        {
            ShowEducationDescription(educationText);

            SetNextStep();

            Save();
        }

        private void ShowEducationDescription(LocalizedString educationText)
        {
            playerInputActions.EnableCanvasControlMode();
            educationCanvasLiaison.EducationCanvas.enabled = true;
            educationCanvasLiaison.SetEducationText(
                localizedText: educationText);

            Save();
        }

        private void HideEducationDescriptionAsStep()
        {
            HideEducationDescription();
            SetNextStep();
            ActivateEffect();
            
            Save();
        }

        public void HideEducationDescription()
        {
            playerInputActions.DisableCanvasControlMode();
            educationCanvasLiaison.EducationCanvas.enabled = false;
        }

        private void ActivateEffect()
        {
            StartCoroutine(ActivateEffectWithDelay());
        }

        private IEnumerator ActivateEffectWithDelay()
        {
            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTime);
            selectedTableEffect.ActivateEffectWithoutDelayAtFirst();
        }



        public void Load(EducationHandlerForSaving educationHandlerForLoading)
        {
            if (educationHandlerForLoading.IsValuesSaved)
            {
                IsEducationActive = educationHandlerForLoading.IsEducationActive;
                step = educationHandlerForLoading.Step;
                isFirstLvlClueWasDisplayed = educationHandlerForLoading.IsFirstLvlClueWasDisplayed;
                isSecondLvlClueWasDisplayed = educationHandlerForLoading.IsSecondLvlClueWasDisplayed;
                isThirdLvlClueWasDisplayed = educationHandlerForLoading.IsThirdLvlClueWasDisplayed;
            }
            else
            {
                //CompleteFirstStep();
            }
        }
    }
}