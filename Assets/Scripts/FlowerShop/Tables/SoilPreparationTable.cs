using System;
using System.Collections;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Settings;
using FlowerShop.Sounds;
using FlowerShop.Tables.Abstract;
using FlowerShop.Tables.Helpers;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    [RequireComponent(typeof(TableObjectsRotation))]
    public class SoilPreparationTable : UpgradableBreakableTable, ISavableObject
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly SoundsHandler soundsHandler;
        [Inject] private readonly TablesSettings tablesSettings;
    
        [SerializeField] private Transform potOnTableTransform;
        [SerializeField] private MeshRenderer[] gearsMeshRenderers;

        [HideInInspector, SerializeField] private TableObjectsRotation tableObjectsRotation;

        private Pot potToSoilPreparation;
        private float soilPreparationTime;
        
        [field: SerializeField] public string UniqueKey { get; private set; }
        
        public int TableLvl => tableLvl;

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
            SetSoilPreparationTime();
            
            breakableTableBaseComponent.CheckIfTableBroken();
        }

        public override void ExecuteClickableAbility()
        {
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
                   CanPlayerUpgradeTableForSelectableEffect();
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
                       !potToSoilPreparation.IsSoilInsidePot;
            }

            return false;
        }

        private IEnumerator SoilPreparation()
        {
            potToSoilPreparation.PutOnTableAndKeepPlayerBusy(potOnTableTransform);

            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            tableObjectsRotation.StartObjectsRotation();
            soundsHandler.StartPlayingSoilPreparationAudio();
            
            yield return new WaitForSeconds(soilPreparationTime);
            tableObjectsRotation.PauseObjectsRotation();
            soundsHandler.StopPlayingSoilPreparationAudio();
            
            potToSoilPreparation.FillPotWithSoil();
            potToSoilPreparation.TakeInPlayerHandsAndSetPlayerFree();
            UseBreakableTable();
            
            Save();
        }

        private void FixSoilPreparationTable()
        {
            FixBreakableFlowerTable(
                repairsAndUpgradesSettings.SoilPreparationMinQuantity * (tableLvl + 1),
                repairsAndUpgradesSettings.SoilPreparationMaxQuantity * (tableLvl + 1));
            
            Save();
        }
    }
}
