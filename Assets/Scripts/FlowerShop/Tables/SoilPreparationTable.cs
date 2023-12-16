using System.Collections;
using FlowerShop.PickableObjects;
using FlowerShop.Settings;
using FlowerShop.Tables.Abstract;
using FlowerShop.Tables.Helpers;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    [RequireComponent(typeof(TableObjectsRotation))]
    public class SoilPreparationTable : UpgradableBreakableTable
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly TablesSettings tablesSettings;
    
        [SerializeField] private Transform potOnTableTransform;
        [SerializeField] private MeshRenderer[] gearsMeshRenderers;

        [HideInInspector, SerializeField] private TableObjectsRotation tableObjectsRotation;

        private Pot potToSoilPreparation;
        private float soilPreparationTime;
        
        public int TableLvl => tableLvl;

        private void OnValidate()
        {
            tableObjectsRotation = GetComponent<TableObjectsRotation>();
        }
        
        private protected override void Start()
        {
            base.Start();
            SetSoilPreparationTime();

            SetActionsBeforeBrokenQuantity(
                repairsAndUpgradesSettings.SoilPreparationMinQuantity * (tableLvl + 1),
                repairsAndUpgradesSettings.SoilPreparationMaxQuantity * (tableLvl + 1));
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

        public override void UpgradeTable()
        {
            base.UpgradeTable();
            SetSoilPreparationTime();
            gearsMeshRenderers[tableLvl].enabled = true;
        }

        private void SetSoilPreparationTime()
        {
            soilPreparationTime = tablesSettings.SoilPreparationTime - tableLvl * tablesSettings.SoilPreparationLvlTimeDelta;
        }

        private bool CanPlayerStartSoilPreparation()
        {
            if (playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                potToSoilPreparation = currentPot;

                return potToSoilPreparation.GrowingRoom == growingRoom && 
                       !potToSoilPreparation.IsSoilInsidePot &&
                       !IsTableBroken;
            }

            return false;
        }

        private IEnumerator SoilPreparation()
        {
            potToSoilPreparation.PutOnTableAndKeepPlayerBusy(potOnTableTransform);

            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            tableObjectsRotation.StartObjectsRotation();
            
            yield return new WaitForSeconds(soilPreparationTime);
            tableObjectsRotation.PauseObjectsRotation();
            
            potToSoilPreparation.FillPotWithSoil();
            potToSoilPreparation.TakeInPlayerHandsAndSetPlayerFree();
            UseBreakableTable();
        }

        private bool CanPlayerFixTable()
        {
            return playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer && IsTableBroken;
        }

        private void FixSoilPreparationTable()
        {
            FixBreakableFlowerTable(
                repairsAndUpgradesSettings.SoilPreparationMinQuantity * (tableLvl + 1),
                repairsAndUpgradesSettings.SoilPreparationMaxQuantity * (tableLvl + 1));
        }

        private bool CanPlayerUpgradeTable()
        {
            return playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer && 
                   tableLvl < repairsAndUpgradesSettings.MaxUpgradableTableLvl;
        }
    }
}
