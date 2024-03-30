using System.Collections;
using System.Collections.Generic;
using FlowerShop.Effects;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Settings;
using FlowerShop.Tables;
using Input;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Education
{
    public class EducationHandler : MonoBehaviour, ISavableObject
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly EducationCanvasLiaison educationCanvasLiaison;
        [Inject] private readonly EducationSettings educationSettings;
        [Inject] private readonly PlayerInputActions playerInputActions;
        [Inject] private readonly SelectedTableEffect selectedTableEffect;

        [SerializeField] private SoilPreparationTable soilPreparationTable;
        [SerializeField] private FlowersCrossingTableProcess flowersCrossingTableProcess;
        [SerializeField] private Pot potForPlantWeed;
        [SerializeField] private Pot potForPouring;
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
                    educationSettings.BrokenTableDescriptionText,
                    educationSettings.BrokenTableDescriptionCoordinates);
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
                    ShowEducationDescriptionAsStep(
                        educationSettings.FirstDescriptionText,
                        educationSettings.FirstDescriptionCoordinates);
                    break;
                case 1:
                    ShowEducationDescriptionAsStep(
                        educationSettings.MoneyAndShopRatingDescriptionText, 
                        educationSettings.MoneyAndShopRatingDescriptionCoordinates);
                    break;
                case 2:
                    ShowEducationDescriptionAsStep(
                        educationSettings.FlowersCanvasDescriptionText, 
                        educationSettings.FlowersCanvasDescriptionCoordinates);
                    break;
                case 3:
                    educationCanvasLiaison.EducationCanvas.enabled = false;
                    SetNextStep();
                    Save();
                    break;
                case 4: // Open FlowersCanvas
                    SetNextStep();
                    break;
                case 5: // Close FlowersCanvas
                    ShowEducationDescriptionAsStep(
                        educationSettings.PotsRackDescriptionText,
                        educationSettings.PotsRackDescriptionCoordinates);
                    break;
                case 6:
                    HideEducationDescriptionAsStep();
                    break;
                case 7: // Click on PotsRack
                    SetNextStep();
                    break;
                case 8: // Take Pot from PotsRack
                    ShowEducationDescriptionAsStep(
                        educationSettings.SoilPreparationDescriptionText,
                        educationSettings.SoilPreparationDescriptionCoordinates);
                    break;
                case 9:
                    HideEducationDescriptionAsStep();
                    break;
                case 10: // Put Pot on SoilPreparationTable
                    SetNextStep();
                    break;
                case 11: // Take Pot with soil in PlayerHands
                    ShowEducationDescriptionAsStep(
                        educationSettings.SeedPlantDescriptionText,
                        educationSettings.SeedPlantDescriptionCoordinates);
                    break;
                case 12:
                    HideEducationDescriptionAsStep();
                    break;
                case 13: // Open SeedCanvas
                    SetNextStep();
                    break;
                case 14: // Plant Cactus seed
                    SetNextStep();
                    Save();
                    break;
                case 15:
                    ShowEducationDescriptionAsStep(
                        educationSettings.GrowingTableDescriptionText,
                        educationSettings.GrowingTableDescriptionCoordinates);
                    break;
                case 16:
                    HideEducationDescriptionAsStep();
                    break;
                case 17: // Click on GrowingTable
                    SetNextStep();
                    break;
                case 18: // Put Pot on GrowingTable
                    ShowEducationDescriptionAsStep(
                        educationSettings.PlantNextFlowerDescriptionText,
                        educationSettings.PlantNextFlowerDescriptionCoordinates);
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
                    SetNextStep();
                    break;
                case 25: // Plant Dandelion
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
                    ShowEducationDescriptionAsStep(
                        educationSettings.FertilizersDescriptionText,
                        educationSettings.FertilizersDescriptionCoordinates);
                    break;
                case 29: // Put Pot on GrowingTable
                    HideEducationDescriptionAsStep();
                    break;
                case 30: // Click on FertilizersTable
                    SetNextStep();
                    break;
                case 31: // Click on GrowerToMaxLvl
                    SetNextStep();
                    break;
                case 32: // Take GrowerToMaxLvl in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 33: // Click on Fist GrowingTable with GrowerToMaxLvl in PlayerHands
                    SetNextStep();
                    break;
                case 34: // Treat Pot on Fist GrowingTable
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 35: // Click on Second GrowingTable with GrowerToMaxLvl in PlayerHands
                    SetNextStep();
                    break;
                case 36: // Treat Pot on Second GrowingTable
                    ShowEducationDescriptionAsStep(
                        educationSettings.FertilizersEndUsingDescriptionText,
                        educationSettings.FertilizersEndUsingDescriptionCoordinates);
                    break;
                case 37:
                    HideEducationDescriptionAsStep();
                    break;
                case 38: // Click on FertilizersTable
                    SetNextStep();
                    break;
                case 39: // Put Fertilizer on Table
                    ShowEducationDescriptionAsStep(
                        educationSettings.CrossingTableDescriptionText,
                        educationSettings.CrossingTableDescriptionCoordinates);
                    break;
                case 40:
                    HideEducationDescriptionAsStep();
                    break;
                case 41: // Click on Fist GrowingTable
                    SetNextStep();
                    break;
                case 42: // Take Pot in Player Hands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 43: // Click on First Crossing Table
                    SetNextStep();
                    break;
                case 44: // Put Pot on First Crossing Table
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 45: // Click on Second GrowingTable
                    SetNextStep();
                    break;
                case 46: // Take Pot in Player Hands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 47: // Click on Second Crossing Table
                    SetNextStep();
                    break;
                case 48: // Put Pot on Second Crossing Table
                    ShowEducationDescriptionAsStep(
                        educationSettings.CrossingTableProcessDescriptionText,
                        educationSettings.CrossingTableProcessDescriptionCoordinates);
                    break;
                case 49:
                    ShowEducationDescriptionAsStep(
                        educationSettings.CrossingTableProcessStartDescriptionText,
                        educationSettings.CrossingTableProcessStartDescriptionCoordinates);
                    break;
                case 50:
                    HideEducationDescriptionAsStep();
                    break;
                case 51: // Click on CrossingTableProcess
                    // Cause by CrossingTableProcess
                    break;
                case 52:
                    HideEducationDescriptionAsStep();
                    break;
                case 53: // Click on RepairAndImprovementTable
                    SetNextStep();
                    break;
                case 54: // Take Hammer in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 55: // Click on SoilPreparationTable
                    SetNextStep();
                    break;
                case 56: // Repair SoilPreparationTable
                    ShowEducationDescriptionAsStep(
                        educationSettings.UpgradeTableDescriptionText,
                        educationSettings.UpgradeTableDescriptionCoordinates);
                    break;
                case 57:
                    HideEducationDescriptionAsStep();
                    break;
                case 58: // Click on Growing Table with Hammer in PlayerHands
                    SetNextStep();
                    break;
                case 59: // Click on Upgrade Button
                    SetNextStep();
                    break;
                case 60:
                    ShowEducationDescriptionAsStep(
                        educationSettings.CompleteUpgradingTableDescriptionText,
                        educationSettings.CompleteUpgradingTableDescriptionCoordinates);
                    break;
                case 61:
                    HideEducationDescriptionAsStep();
                    break;
                case 62: // Click on RepairAndImprovementTable
                    SetNextStep();
                    break;
                case 63: // Put Hammer on RepairAndImprovementTable
                    ShowEducationDescriptionAsStep(
                        educationSettings.SoilForNewSeedDescriptionText,
                        educationSettings.SoilForNewSeedDescriptionCoordinates);
                    break;
                case 64:
                    HideEducationDescriptionAsStep();
                    break;
                case 65: // Click on PotsRack
                    SetNextStep();
                    break;
                case 66: // Take Pot in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 67: // Click on SoilPreparationTable
                    SetNextStep();
                    break;
                case 68: // Take Pot in PlayerHands
                    if (flowersCrossingTableProcess.IsSeedCrossing)
                    {
                        ShowEducationDescriptionAsStep(
                            educationSettings.WaitThenPlantNewSeedDescriptionText,
                            educationSettings.WaitThenPlantNewSeedDescriptionCoordinates);
                    }
                    else
                    {
                        ShowEducationDescriptionAsStep(
                            educationSettings.PlantNewSeedDescriptionText,
                            educationSettings.PlantNewSeedDescriptionCoordinates);
                    }
                    break;
                case 69:
                    HideEducationDescriptionAsStep();
                    break;
                case 70: // Click on FlowersCrossingTableProcess
                    if (!flowersCrossingTableProcess.IsSeedCrossing)
                    {
                        SetNextStep();
                    }
                    break;
                case 71: // Take Pot with new seed in PlayerHands
                    // Cause by CrossingTableProcess
                    break;
                case 72: // Click on FlowerGrowingTable
                    SetNextStep();
                    break;
                case 73: // Put Pot with new seed on GrowingTable
                    ShowEducationDescriptionAsStep(
                        educationSettings.GrowthAcceleratorUsingDescriptionText,
                        educationSettings.GrowthAcceleratorUsingDescriptionCoordinates);
                    break;
                case 74:
                    HideEducationDescriptionAsStep();
                    break;
                case 75: // Click on FertilizersTable
                    SetNextStep();
                    break;
                case 76: // Click on GrowthAccelerator Button
                    SetNextStep();
                    break;
                case 77: // Take GrowthAccelerator in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 78: // Click on GrowingTable with GrowthAccelerator in PlayerHands
                    SetNextStep();
                    break;
                case 79: // Treat Pot by GrowthAccelerator
                    ShowEducationDescriptionAsStep(
                        educationSettings.GrowthAcceleratorEndUsingDescriptionText,
                        educationSettings.GrowthAcceleratorEndUsingDescriptionCoordinates);
                    break;
                case 80:
                    HideEducationDescriptionAsStep();
                    break;
                case 81: // Click on FertilizersTable
                    SetNextStep();
                    break;
                case 82: // Put GrowthAccelerator on FertilizersTable
                    ShowEducationDescriptionAsStep(
                        educationSettings.TakePotFromCrossingTableDescriptionText,
                        educationSettings.TakePotFromCrossingTableDescriptionCoordinates);
                    break;
                case 83:
                    HideEducationDescriptionAsStep();
                    break;
                case 84: // Click on First CrossingTable
                    SetNextStep();
                    break;
                case 85: // Take Pot in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 86: // Click on GrowingTable
                    SetNextStep();
                    break;
                case 87: // Put Pot on GrowingTable
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 88: // Click on Second CrossingTable
                    SetNextStep();
                    break;
                case 89: // Take Pot in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 90: // Click on GrowingTable
                    SetNextStep();
                    break;
                case 91: // Put Pot on GrowingTable and plant Weed
                    potForPlantWeed.PlantWeed();
                    ShowEducationDescriptionAsStep(
                        educationSettings.WeedDescriptionText,
                        educationSettings.WeedDescriptionCoordinates);
                    break;
                case 92:
                    HideEducationDescriptionAsStep();
                    break;
                case 93: // Click on HoeTable
                    SetNextStep();
                    break;
                case 94: // Take Hoe in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 95: // Click on GrowingTable with Weed in Pot
                    SetNextStep();
                    break;
                case 96: // Delete Weed from Pot
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 97: // Click on HoeTable
                    SetNextStep();
                    break;
                case 98: // Pot Hoe on Table
                    ShowEducationDescriptionAsStep(
                        educationSettings.PourPotFirstTimeDescriptionText,
                        educationSettings.PourPotFirstTimeDescriptionCoordinates);
                    break;
                case 99:
                    HideEducationDescriptionAsStep();
                    break;
                case 100: // Click on WateringTable
                    SetNextStep();
                    break;
                case 101: // Take WateringCan in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 102: // Click on GrowingTable with Pot that need Water
                    if (potForPouring.IsFlowerNeedWater)
                    {
                        SetNextStep();
                    }
                    break;
                case 103: // Pour Pot
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 104: // Click on WateringTable
                    SetNextStep();
                    break;
                case 105: // Put WateringCan on WateringTable
                    ShowEducationDescriptionAsStep(
                        educationSettings.GrowingLvlIncreaserDescriptionText,
                        educationSettings.GrowingLvlIncreaserDescriptionCoordinates);
                    break;
                case 106:
                    HideEducationDescriptionAsStep();
                    break;
                case 107: // Click on FertilizersTable
                    SetNextStep();
                    break;
                case 108: // Click on GrowingLvlIncreaser Button
                    SetNextStep();
                    break;
                case 109: // Take GrowingLvlIncreaser in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 110: // Click on First GrowingTable
                    SetNextStep();
                    break;
                case 111: // Use GrowingLvlIncreaser on First GrowingTable
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 112: // Click on Second GrowingTable
                    SetNextStep();
                    break;
                case 113: // Use GrowingLvlIncreaser on Second GrowingTable
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 114: // Click on FertilizersTable
                    SetNextStep();
                    break;
                case 115: // Put GrowingLvlIncreaser on FertilizersTable
                    ShowEducationDescriptionAsStep(
                        educationSettings.PourPotSecondTimeDescriptionText,
                        educationSettings.PourPotSecondTimeDescriptionCoordinates);
                    break;
                case 116:
                    HideEducationDescriptionAsStep();
                    break;
                case 117: // Click on WateringTable
                    SetNextStep();
                    break;
                case 118: // Take WateringCan in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 119: // Click on GrowingTable with Pot to Pour
                    if (potForPouring.IsFlowerNeedWater)
                    {
                        SetNextStep();
                    }
                    break;
                case 120: // PourPot on GrowingTable
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 121: // Click on WateringTable
                    SetNextStep();
                    break;
                case 122: // Put WateringCan on WateringTable
                    ShowEducationDescriptionAsStep(
                        educationSettings.CollectionRoomDescriptionText,
                        educationSettings.CollectionRoomDescriptionCoordinates);
                    break;
                case 123:
                    ShowEducationDescriptionAsStep(
                        educationSettings.PutPotInCollectionRoomDescriptionText,
                        educationSettings.PutPotInCollectionRoomDescriptionCoordinates);
                    break;
                case 124:
                    HideEducationDescriptionAsStep();
                    break;
                case 125: // Click on GrowingTable with Pot for Collection
                    SetNextStep();
                    break;
                case 126: // Take Pot in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 127: // Click on TableForCollection
                    SetNextStep();
                    break;
                case 128: // Put Pot on TableForCollection
                    ShowEducationDescriptionAsStep(
                        educationSettings.StorageTableRoomDescriptionText,
                        educationSettings.StorageTableDescriptionCoordinates);
                    break;
                case 129:
                    HideEducationDescriptionAsStep();
                    break;
                case 130: // Click on StorageTable
                    SetNextStep();
                    break;
                case 131: // Put Pot on StorageTable
                    ShowEducationDescriptionAsStep(
                        educationSettings.SalesDescriptionText,
                        educationSettings.SalesDescriptionCoordinates);
                    break;
                case 132:
                    ShowEducationDescriptionAsStep(
                        educationSettings.PutPotOnSaleTableDescriptionText,
                        educationSettings.PutPotOnSaleTableDescriptionCoordinates);
                    break;
                case 133:
                    HideEducationDescriptionAsStep();
                    break;
                case 134: // Click on GrowingTable with Pot for Sale
                    SetNextStep();
                    break;
                case 135: // Take Pot in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 136: // Click on SaleTable
                    SetNextStep();
                    break;
                case 137: // Put Pot on SaleTable
                    ShowEducationDescriptionAsStep(
                        educationSettings.PutPotOnPotsRackDescriptionText,
                        educationSettings.PutPotOnPotsRackDescriptionCoordinates);
                    break;
                case 138:
                    HideEducationDescriptionAsStep();
                    break;
                case 139: // Click on PotsRack
                    SetNextStep();
                    break;
                case 140: // Put Pot on PotsRack
                    ShowEducationDescriptionAsStep(
                        educationSettings.PourPotLastTimeDescriptionText,
                        educationSettings.PourPotLastTimeDescriptionCoordinates);
                    break;
                case 141:
                    HideEducationDescriptionAsStep();
                    break;
                case 142: // Click on WateringTable
                    SetNextStep();
                    break;
                case 143: // Take WateringCan in PlayerHands
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 144: // Click on GrowingTable
                    if (potForPouring.IsFlowerNeedWater)
                    {
                        SetNextStep();
                    }
                    break;
                case 145: // Pour Pot
                    SetNextStep();
                    ActivateEffect();
                    Save();
                    break;
                case 146: // Click on WateringTable
                    SetNextStep();
                    break;
                case 147: // Put WateringCan on WateringTable
                    ShowEducationDescriptionAsStep(
                        educationSettings.ComputerTableDescriptionText,
                        educationSettings.ComputerTableDescriptionCoordinates);
                    break;
                case 148:
                    ShowEducationDescriptionAsStep(
                        educationSettings.ComplaintsDescriptionText,
                        educationSettings.ComplaintsDescriptionCoordinates);
                    break;
                case 149:
                    ShowEducationDescriptionAsStep(
                        educationSettings.VipDescriptionText,
                        educationSettings.VipDescriptionCoordinates);
                    break;
                case 150:
                    ShowEducationDescriptionAsStep(
                        educationSettings.CoffeeDescriptionText,
                        educationSettings.CoffeeDescriptionCoordinates);
                    break;
                case 151:
                    ShowEducationDescriptionAsStep(
                        educationSettings.MusicDescriptionText,
                        educationSettings.MusicDescriptionCoordinates);
                    break;
                case 152:
                    ShowEducationDescriptionAsStep(
                        educationSettings.EducationEndDescriptionText,
                        educationSettings.EducationEndDescriptionCoordinates);
                    break;
                case 153:
                    playerInputActions.DisableCanvasControlMode();
                    educationCanvasLiaison.EducationCanvas.enabled = false;
                    IsEducationActive = false;
                    ActivateEffect();
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
            IsEducationActive = true;
            playerInputActions.EnableCanvasControlMode();
            educationCanvasLiaison.EducationCanvas.enabled = true;
            educationCanvasLiaison.SetEducationText(
                text: educationSettings.WelcomeText, 
                textPanelCoordinates: educationSettings.WelcomeCoordinates);
        }

        private void ShowEducationDescriptionAsStep(string educationText, Vector2 educationTextCoordinates)
        {
            playerInputActions.EnableCanvasControlMode();
            educationCanvasLiaison.EducationCanvas.enabled = true;
            educationCanvasLiaison.SetEducationText(
                text: educationText, 
                textPanelCoordinates: educationTextCoordinates);
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