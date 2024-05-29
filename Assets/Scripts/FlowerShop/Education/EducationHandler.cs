using System.Collections;
using System.Collections.Generic;
using FlowerShop.Achievements;
using FlowerShop.Effects;
using FlowerShop.Fertilizers;
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
        [Inject] private readonly EducationCanvasLiaison educationCanvasLiaison;
        [Inject] private readonly EducationSettings educationSettings;
        [Inject] private readonly ExemplaryStudent exemplaryStudent;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly PlayerInputActions playerInputActions;
        [Inject] private readonly SelectedTableEffect selectedTableEffect;

        [Header("Canvas Scalers")]
        [SerializeField] private CanvasElementScaler flowersCanvasButton;
        [SerializeField] private CanvasElementScaler cactusSeedButton;
        [SerializeField] private CanvasElementScaler dandelionSeedButton;
        [SerializeField] private CanvasElementScaler growthAcceleratorButton;
        [SerializeField] private CanvasElementScaler growingLvlIncreaserButton;
        [SerializeField] private CanvasElementScaler growerToMaxLvlButton;
        [SerializeField] private CanvasElementScaler improvementButton;
        [SerializeField] private CanvasElementScaler growerToMaxLvlInfoButton;
        [SerializeField] private CanvasElementScaler closeFertilizersInfoPanelButton;

        [Header("Scene Objects")]
        [SerializeField] private SoilPreparationTable soilPreparationTable;
        [SerializeField] private FlowersCrossingTableProcess flowersCrossingTableProcess;
        [SerializeField] private Pot potForPlantWeed;
        [SerializeField] private Pot potForPouring;

        [Header("Education Sequence")]
        [SerializeField] private List<MonoBehaviour> educationSequence = new();
        
        private int step;
        
        [field: HideInInspector, SerializeField] public bool IsEducationActive { get; private set; }
        [field: SerializeField] public string UniqueKey { get; private set; }

        private void Awake()
        {
            Load();
        }

        private void Start()
        {
            if (IsEducationActive && step > 0)
            {
                step--;
                if (step == 51)
                {
                    TryBrokenSoilPreparationTableDuringEducation();
                }
                else if (step == 71)
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

        public void Load()
        {
            EducationHandlerForSaving educationHandlerForLoading =
                SavesHandler.Load<EducationHandlerForSaving>(UniqueKey);

            if (educationHandlerForLoading.IsValuesSaved)
            {
                IsEducationActive = educationHandlerForLoading.IsEducationActive;
                step = educationHandlerForLoading.Step;
            }
            else
            {
                //CompleteFirstStep();
            }
        }

        public void Save()
        {
            EducationHandlerForSaving educationHandlerForSaving = new(IsEducationActive, step);
            
            SavesHandler.Save(UniqueKey, educationHandlerForSaving);
        }

        public void TryBrokenSoilPreparationTableDuringEducation()
        {
            if (IsEducationActive && step == 51)
            {
                if (!soilPreparationTable.IsTableBroken)
                {
                    soilPreparationTable.BrokenTable();
                }
                ShowEducationDescriptionAsStep(
                    educationSettings.BrokenTableDescriptionText);
            }
        }

        public void TrySetNextStepByPlantingCrossedSeed()
        {
            if (IsEducationActive && step == 71)
            {
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
                    break;
                case 7: // Click on PotsRack
                    SetNextStep();
                    break;
                case 8: // Take Pot from PotsRack
                    ShowEducationDescriptionAsStep(educationSettings.SoilPreparationDescriptionText);
                    break;
                case 9:
                    HideEducationDescriptionAsStep();
                    break;
                case 10: // Put Pot on SoilPreparationTable
                    SetNextStep();
                    break;
                case 11: // Take Pot with soil in PlayerHands
                    ShowEducationDescriptionAsStep(educationSettings.SeedPlantDescriptionText);
                    break;
                case 12:
                    HideEducationDescriptionAsStep();
                    break;
                case 13: // Open SeedCanvas
                    cactusSeedButton.ActivateEffect();
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
                    break;
                case 17: // Click on GrowingTable
                    SetNextStep();
                    break;
                case 18: // Put Pot on GrowingTable
                    ShowEducationDescriptionAsStep(educationSettings.PlantNextFlowerDescriptionText);
                    break;
                case 19:
                    HideEducationDescriptionAsStep();
                    break;
                case 20: // Click on PatsRack
                    SetNextStep();
                    break;
                case 21: // Take Pot from PotsRack
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 22: // Click on SoilPreparationTable
                    SetNextStep();
                    break;
                case 23: // Take Pot with Soil in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 24: // Click on SeedTable
                    dandelionSeedButton.ActivateEffect();
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
                    Save();
                    break;
                case 27: // Click on GrowingTable
                    SetNextStep();
                    break;
                case 28: // Put Pot on GrowingTable
                    ShowEducationDescriptionAsStep(educationSettings.FertilizersDescriptionText);
                    break;
                case 29: // Put Pot on GrowingTable
                    HideEducationDescriptionAsStep();
                    break;
                case 30: // Click on FertilizersTable
                    growerToMaxLvlInfoButton.ActivateEffect();
                    SetNextStep();
                    break;
                case 31: // Click on GrowerToMaxLvl Info Button
                    growerToMaxLvlInfoButton.DeactivateEffect();
                    closeFertilizersInfoPanelButton.ActivateEffect();
                    SetNextStep();
                    break;
                case 32: // Click on Close Info Panel
                    closeFertilizersInfoPanelButton.DeactivateEffect();
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
                    Save();
                    break;
                case 35: // Click on Fist GrowingTable with GrowerToMaxLvl in PlayerHands
                    SetNextStep();
                    break;
                case 36: // Treat Pot on Fist GrowingTable
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 37: // Click on Second GrowingTable with GrowerToMaxLvl in PlayerHands
                    SetNextStep();
                    break;
                case 38: // Treat Pot on Second GrowingTable
                    ShowEducationDescriptionAsStep(educationSettings.FertilizersEndUsingDescriptionText);
                    break;
                case 39:
                    HideEducationDescriptionAsStep();
                    break;
                case 40: // Click on FertilizersTable
                    SetNextStep();
                    break;
                case 41: // Put Fertilizer on Table
                    ShowEducationDescriptionAsStep(educationSettings.CrossingTableDescriptionText);
                    break;
                case 42:
                    HideEducationDescriptionAsStep();
                    break;
                case 43: // Click on Fist GrowingTable
                    SetNextStep();
                    break;
                case 44: // Take Pot in Player Hands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 45: // Click on First Crossing Table
                    SetNextStep();
                    break;
                case 46: // Put Pot on First Crossing Table
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 47: // Click on Second GrowingTable
                    SetNextStep();
                    break;
                case 48: // Take Pot in Player Hands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 49: // Click on Second Crossing Table
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
                    break;
                case 53: // Click on CrossingTableProcess
                    // Cause by CrossingTableProcess
                    break;
                case 54:
                    HideEducationDescriptionAsStep();
                    break;
                case 55: // Click on RepairAndImprovementTable
                    SetNextStep();
                    break;
                case 56: // Take Hammer in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 57: // Click on SoilPreparationTable
                    SetNextStep();
                    break;
                case 58: // Repair SoilPreparationTable
                    ShowEducationDescriptionAsStep(educationSettings.UpgradeTableDescriptionText);
                    break;
                case 59:
                    HideEducationDescriptionAsStep();
                    break;
                case 60: // Click on Growing Table with Hammer in PlayerHands
                    improvementButton.ActivateEffect();
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
                    break;
                case 64: // Click on RepairAndImprovementTable
                    SetNextStep();
                    break;
                case 65: // Put Hammer on RepairAndImprovementTable
                    ShowEducationDescriptionAsStep(educationSettings.SoilForNewSeedDescriptionText);
                    break;
                case 66:
                    HideEducationDescriptionAsStep();
                    break;
                case 67: // Click on PotsRack
                    SetNextStep();
                    break;
                case 68: // Take Pot in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 69: // Click on SoilPreparationTable
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
                    break;
                case 72: // Click on FlowersCrossingTableProcess
                    if (!flowersCrossingTableProcess.IsSeedCrossing)
                    {
                        SetNextStep();
                    }
                    break;
                case 73: // Take Pot with new seed in PlayerHands
                    // Cause by CrossingTableProcess
                    break;
                case 74: // Click on FlowerGrowingTable
                    SetNextStep();
                    break;
                case 75: // Put Pot with new seed on GrowingTable
                    ShowEducationDescriptionAsStep(educationSettings.GrowthAcceleratorUsingDescriptionText);
                    break;
                case 76:
                    HideEducationDescriptionAsStep();
                    break;
                case 77: // Click on FertilizersTable
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
                    Save();
                    break;
                case 80: // Click on GrowingTable with GrowthAccelerator in PlayerHands
                    SetNextStep();
                    break;
                case 81: // Treat Pot by GrowthAccelerator
                    ShowEducationDescriptionAsStep(educationSettings.GrowthAcceleratorEndUsingDescriptionText);
                    break;
                case 82:
                    HideEducationDescriptionAsStep();
                    break;
                case 83: // Click on FertilizersTable
                    SetNextStep();
                    break;
                case 84: // Put GrowthAccelerator on FertilizersTable
                    ShowEducationDescriptionAsStep(educationSettings.TakePotFromCrossingTableDescriptionText);
                    break;
                case 85:
                    HideEducationDescriptionAsStep();
                    break;
                case 86: // Click on First CrossingTable
                    SetNextStep();
                    break;
                case 87: // Take Pot in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 88: // Click on GrowingTable
                    SetNextStep();
                    break;
                case 89: // Put Pot on GrowingTable
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 90: // Click on Second CrossingTable
                    SetNextStep();
                    break;
                case 91: // Take Pot in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 92: // Click on GrowingTable
                    SetNextStep();
                    break;
                case 93: // Put Pot on GrowingTable and plant Weed
                    potForPlantWeed.PlantWeed();
                    ShowEducationDescriptionAsStep(educationSettings.WeedDescriptionText);
                    break;
                case 94:
                    HideEducationDescriptionAsStep();
                    break;
                case 95: // Click on HoeTable
                    SetNextStep();
                    break;
                case 96: // Take Hoe in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 97: // Click on GrowingTable with Weed in Pot
                    SetNextStep();
                    break;
                case 98: // Delete Weed from Pot
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 99: // Click on HoeTable
                    SetNextStep();
                    break;
                case 100: // Pot Hoe on Table
                    ShowEducationDescriptionAsStep(educationSettings.PourPotFirstTimeDescriptionText);
                    break;
                case 101:
                    HideEducationDescriptionAsStep();
                    break;
                case 102: // Click on WateringTable
                    SetNextStep();
                    break;
                case 103: // Take WateringCan in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 104: // Click on GrowingTable with Pot that need Water
                    if (potForPouring.IsFlowerNeedWater)
                    {
                        SetNextStep();
                    }
                    break;
                case 105: // Pour Pot
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 106: // Click on WateringTable
                    SetNextStep();
                    break;
                case 107: // Put WateringCan on WateringTable
                    ShowEducationDescriptionAsStep(educationSettings.GrowingLvlIncreaserDescriptionText);
                    break;
                case 108:
                    HideEducationDescriptionAsStep();
                    break;
                case 109: // Click on FertilizersTable
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
                    Save();
                    break;
                case 112: // Click on First GrowingTable
                    SetNextStep();
                    break;
                case 113: // Use GrowingLvlIncreaser on First GrowingTable
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 114: // Click on Second GrowingTable
                    SetNextStep();
                    break;
                case 115: // Use GrowingLvlIncreaser on Second GrowingTable
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 116: // Click on FertilizersTable
                    SetNextStep();
                    break;
                case 117: // Put GrowingLvlIncreaser on FertilizersTable
                    ShowEducationDescriptionAsStep(educationSettings.PourPotSecondTimeDescriptionText);
                    break;
                case 118:
                    HideEducationDescriptionAsStep();
                    break;
                case 119: // Click on WateringTable
                    SetNextStep();
                    break;
                case 120: // Take WateringCan in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 121: // Click on GrowingTable with Pot to Pour
                    if (potForPouring.IsFlowerNeedWater)
                    {
                        SetNextStep();
                    }
                    break;
                case 122: // PourPot on GrowingTable
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 123: // Click on WateringTable
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
                    break;
                case 127: // Click on GrowingTable with Pot for Collection
                    SetNextStep();
                    break;
                case 128: // Take Pot in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 129: // Click on TableForCollection
                    SetNextStep();
                    break;
                case 130: // Put Pot on TableForCollection
                    ShowEducationDescriptionAsStep(educationSettings.StorageTableRoomDescriptionText);
                    break;
                case 131:
                    HideEducationDescriptionAsStep();
                    break;
                case 132: // Click on StorageTable
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
                    break;
                case 136: // Click on GrowingTable with Pot for Sale
                    SetNextStep();
                    break;
                case 137: // Take Pot in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 138: // Click on SaleTable
                    SetNextStep();
                    break;
                case 139: // Put Pot on SaleTable
                    ShowEducationDescriptionAsStep(educationSettings.PutPotOnPotsRackDescriptionText);
                    break;
                case 140:
                    HideEducationDescriptionAsStep();
                    break;
                case 141: // Click on PotsRack
                    SetNextStep();
                    break;
                case 142: // Put Pot on PotsRack
                    ShowEducationDescriptionAsStep(educationSettings.PourPotLastTimeDescriptionText);
                    break;
                case 143:
                    HideEducationDescriptionAsStep();
                    break;
                case 144: // Click on WateringTable
                    SetNextStep();
                    break;
                case 145: // Take WateringCan in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 146: // Click on GrowingTable
                    if (potForPouring.IsFlowerNeedWater)
                    {
                        SetNextStep();
                    }
                    break;
                case 147: // Pour Pot
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 148: // Click on WateringTable
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
                    ShowEducationDescriptionAsStep(educationSettings.AchievementsText);
                    break;
                case 153:
                    ShowEducationDescriptionAsStep(educationSettings.CoffeeDescriptionText);
                    break;
                case 154:
                    HideEducationDescriptionAsStep();
                    break;
                case 155: // Click on CoffeTable
                    SetNextStep();
                    break;
                case 156:
                    ShowEducationDescriptionAsStep(educationSettings.MusicDescriptionText);
                    break;
                case 157:
                    ShowEducationDescriptionAsStep(educationSettings.EducationEndDescriptionText);
                    break;
                case 158:
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
            playerInputActions.EnableCanvasControlMode();
            educationCanvasLiaison.EducationCanvas.enabled = true;
            educationCanvasLiaison.SetEducationText(
                localizedText: educationText);
            SetNextStep();

            Save();
        }

        private void HideEducationDescriptionAsStep()
        {
            playerInputActions.DisableCanvasControlMode();
            educationCanvasLiaison.EducationCanvas.enabled = false;
            SetNextStep();
            ActivateEffect();
            
            Save();
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
    }
}