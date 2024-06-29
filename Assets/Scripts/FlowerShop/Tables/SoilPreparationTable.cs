using System.Collections;
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
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    [RequireComponent(typeof(TableObjectsRotation))]
    public class SoilPreparationTable : UpgradableBreakableTable, ISavableObject
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly HelpCanvasLiaison helpCanvasLiaison;
        [Inject] private readonly HelpTexts helpTexts;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly PlayerMoney playerMoney;
        [Inject] private readonly SoundsHandler soundsHandler;
        [Inject] private readonly TablesSettings tablesSettings;
    
        [SerializeField] private Transform potOnTableTransform;
        [SerializeField] private MeshRenderer[] gearsMeshRenderers;

        [HideInInspector, SerializeField] private TableObjectsRotation tableObjectsRotation;
        [HideInInspector, SerializeField] private ActionProgressbar actionProgressbar;

        private Pot potToSoilPreparation;
        private float soilPreparationTime;
        
        [field: SerializeField] public string UniqueKey { get; private set; }
        
        public int TableLvl => tableLvl;

        private protected override void OnValidate()
        {
            base.OnValidate();
            
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
            SetSoilPreparationTime();
            
            breakableTableBaseComponent.CheckIfTableBroken();
        }

        private protected override void TryInteractWithTable()
        {
            base.TryInteractWithTable();

            if (playerBusyness.IsPlayerFree)
            {
                if (CanPlayerStartSoilPreparation())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(() => StartCoroutine(SoilPreparation()));
                }
                else if (CanPlayerFixTable())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(FixSoilPreparationTable); 
                }
                else if (CanPlayerUpgradeTable())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(ShowUpgradeCanvas);
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
            else
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.PlayerBusy);
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
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.EmptyHands);
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                if (currentPot.GrowingRoom != growingRoom)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.MismatchGrowingRoom);
                }
                else if (currentPot.IsSoilInsidePot)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.PotNotEmpty);
                }
                else if (playerMoney.CurrentPlayerMoney < flowersSettings.SoilPrice)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.NotEnoughMoney);
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

        public override void UpgradeTableFinish()
        {
            base.UpgradeTableFinish();
            SetSoilPreparationTime();
            gearsMeshRenderers[tableLvl].enabled = true;
            
            SetActionsBeforeBrokenQuantity(
                repairsAndUpgradesSettings.SoilPreparationMaxQuantity * (tableLvl + 1),
                repairsAndUpgradesSettings.SoilPreparationMaxQuantity * (tableLvl + 1));
            
            Save();
        }

        public void BrokenTable()
        {
            ForciblyBrokenTable();
            
            Save();
        }

        public void Save()
        {
            SoilPreparationTableForSaving soilPreparationTableForSaving =
                new(TableLvl, breakableTableBaseComponent.ActionsBeforeBrokenQuantity);
            
            SavesHandler.Save(UniqueKey, soilPreparationTableForSaving);
        }

        public void Load()
        {
            SoilPreparationTableForSaving soilPreparationTableForLoading =
                SavesHandler.Load<SoilPreparationTableForSaving>(UniqueKey);

            if (soilPreparationTableForLoading.IsValuesSaved)
            {
                tableLvl = soilPreparationTableForLoading.TableLvl;
                if (tableLvl > 0)
                {
                    for (int i = 0; i <= tableLvl; i++)
                    {
                        gearsMeshRenderers[i].enabled = true;
                    }
                    LoadLvlMesh();
                }
                
                breakableTableBaseComponent.LoadActionsBeforeBrokenQuantity(
                    soilPreparationTableForLoading.ActionsBeforeBrokenQuantity);
            }
            else
            {
                SetActionsBeforeBrokenQuantity(
                    repairsAndUpgradesSettings.SoilPreparationMaxQuantity * (tableLvl + 1),
                    repairsAndUpgradesSettings.SoilPreparationMaxQuantity * (tableLvl + 1));
            }
        }

        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerStartSoilPreparation() || CanPlayerFixTable() || 
                   CanPlayerUpgradeTableForSelectableEffect() || CanPlayerUseTableInfoCanvas();
        }

        private void SetSoilPreparationTime()
        {
            soilPreparationTime = tablesSettings.SoilPreparationTime - tableLvl * tablesSettings.SoilPreparationLvlTimeDelta;
        }

        private bool CanPlayerStartSoilPreparation()
        {
            if (!IsTableBroken && playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                potToSoilPreparation = currentPot;

                return potToSoilPreparation.GrowingRoom == growingRoom && 
                       !potToSoilPreparation.IsSoilInsidePot &&
                       playerMoney.CurrentPlayerMoney >= flowersSettings.SoilPrice;
            }

            return false;
        }

        private IEnumerator SoilPreparation()
        {
            potToSoilPreparation.PutOnTableAndKeepPlayerBusy(potOnTableTransform);

            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            tableObjectsRotation.StartObjectsRotation();
            soundsHandler.StartPlayingSoilPreparationAudio();

            actionProgressbar.EnableActionProgressbar(soilPreparationTime);
            yield return new WaitForSeconds(soilPreparationTime);
            tableObjectsRotation.PauseObjectsRotation();
            soundsHandler.StopPlayingSoilPreparationAudio();
            
            potToSoilPreparation.FillPotWithSoil();
            playerMoney.TakePlayerMoney(flowersSettings.SoilPrice);
            soundsHandler.PlayTakeMoneyAudio();
            potToSoilPreparation.TakeInPlayerHandsAndSetPlayerFree();
            UseBreakableTable();
            
            Save();
        }

        private void FixSoilPreparationTable()
        {
            FixBreakableFlowerTable(
                repairsAndUpgradesSettings.SoilPreparationMinQuantity * (tableLvl + 1),
                repairsAndUpgradesSettings.SoilPreparationMaxQuantity * (tableLvl + 1));
        }

        private bool CanPlayerUseTableInfoCanvas()
        {
            return !IsTableBroken && playerPickableObjectHandler.CurrentPickableObject is InfoBook;
        }

        private void UseTableInfoCanvas()
        {
            tableInfoCanvasLiaison.ShowCanvas(tableInfo, growingRoom);
        }



        public void Load(SoilPreparationTableForSaving soilPreparationTableForLoading) // CutScene
        {
            if (soilPreparationTableForLoading.IsValuesSaved)
            {
                tableLvl = soilPreparationTableForLoading.TableLvl;
                if (tableLvl > 0)
                {
                    for (int i = 0; i <= tableLvl; i++)
                    {
                        gearsMeshRenderers[i].enabled = true;
                    }
                    LoadLvlMesh();
                }

                breakableTableBaseComponent.LoadActionsBeforeBrokenQuantity(
                    soilPreparationTableForLoading.ActionsBeforeBrokenQuantity);
            }
            else
            {
                SetActionsBeforeBrokenQuantity(
                    repairsAndUpgradesSettings.SoilPreparationMaxQuantity * (tableLvl + 1),
                    repairsAndUpgradesSettings.SoilPreparationMaxQuantity * (tableLvl + 1));
            }
        }
    }
}
