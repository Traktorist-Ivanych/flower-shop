using System;
using FlowerShop.Fertilizers;
using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using FlowerShop.Tables.Abstract;
using FlowerShop.Tables.Helpers;
using FlowerShop.Weeds;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    [RequireComponent(typeof(TableObjectsRotation))]
    public class FlowersGrowingTable : UpgradableBreakableTable
    {
        [Inject] private readonly FlowersSettings flowersSettings;
    
        [SerializeField] private Transform tablePotTransform;
        [SerializeField] private MeshRenderer growingLightMeshRenderer;
        [SerializeField] private MeshRenderer growingTableFanMeshRenderer;
        [SerializeField] private Mesh[] growingLightLvlMeshes = new Mesh[2];
        [SerializeField] private WeedPlanter weedPlanter;

        [HideInInspector, SerializeField] private TableObjectsRotation tableObjectsRotation;
        [HideInInspector, SerializeField] private MeshFilter growingLightMeshFilter;

        private Fertilizer fertilizer;
        private WateringCan wateringCan;
        private WeedingHoe weedingHoe;
        private Pot potOnTable;
        private bool isPotOnTable;

        private protected override void OnValidate()
        {
            base.OnValidate();
            
            tableObjectsRotation = GetComponent<TableObjectsRotation>();
            growingLightMeshFilter = growingLightMeshRenderer.GetComponent<MeshFilter>();
        }

        private protected override void Start()
        {
            base.Start();
            
            SetActionsBeforeBrokenQuantity(
                repairsAndUpgradesSettings.FlowerGrowingTableMinQuantity * (tableLvl + 1),
                repairsAndUpgradesSettings.FlowerGrowingTableMaxQuantity * (tableLvl + 1));
        }

        public override void ExecuteClickableAbility()
        {
            if (playerBusyness.IsPlayerFree)
            {
                if (CanPlayerPutPotOnTable())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(PutPotOnTable);
                }
                else if (CanPlayerPourPotOnTable())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(PourPotOnTable);
                }
                else if (CanPlayerDeleteWeedInPot())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(DeleteWeedInPot);
                }
                else if (CanPlayerUseFertilizer())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(UseFertilizer);
                }
                else if (CanPlayerTakePotInHands())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(TakePotInPlayerHands);
                }
                else if (CanPlayerFixTable())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(FixFlowerGrowingTable);
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
            growingLightMeshFilter.mesh = growingLightLvlMeshes[tableLvl - 1];
            growingTableFanMeshRenderer.enabled = true;
        }

        private bool CanPlayerPutPotOnTable()
        {
            if (!IsTableBroken && !isPotOnTable &&
                playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                potOnTable = currentPot;

                return potOnTable.GrowingRoom == growingRoom &&
                       potOnTable.PlantedFlowerInfo.FlowerName != flowersSettings.FlowerNameEmpty &&
                       potOnTable.FlowerGrowingLvl < flowersSettings.MaxFlowerGrowingLvl;
            }

            return false;
        }

        private void PutPotOnTable()
        {
            potOnTable.PutOnGrowingTableAndSetPlayerFree(tablePotTransform, tableLvl);
            isPotOnTable = true;
            growingLightMeshRenderer.enabled = true;
            playerPickableObjectHandler.ResetPickableObject();
            tableObjectsRotation.StartObjectsRotation();

            if (!potOnTable.IsWeedInPot)
            {
                weedPlanter.AddPotInPlantingWeedList(potOnTable);
            }
        }

        private bool CanPlayerPourPotOnTable()
        {
            if (!IsTableBroken && isPotOnTable && potOnTable.IsFlowerNeedWater &&
                playerPickableObjectHandler.CurrentPickableObject is WateringCan currentWateringCan)
            {
                wateringCan = currentWateringCan;

                return wateringCan.GrowingRoom == growingRoom && wateringCan.CurrentWateringsNumber > 0;
            }

            return false;
        }

        private void PourPotOnTable()
        {
            potOnTable.PourFlower();
        }

        private bool CanPlayerDeleteWeedInPot()
        {
            if (isPotOnTable && potOnTable.IsWeedInPot &&
                playerPickableObjectHandler.CurrentPickableObject is WeedingHoe currentWeedingHoe)
            {
                weedingHoe = currentWeedingHoe;

                return weedingHoe.GrowingRoom == growingRoom;
            }

            return false;
        }

        private void DeleteWeedInPot()
        {
            StartCoroutine(weedingHoe.DeleteWeed(potOnTable, weedPlanter));
        }

        private bool CanPlayerUseFertilizer()
        {
            if (!IsTableBroken && isPotOnTable &&
                playerPickableObjectHandler.CurrentPickableObject is Fertilizer currentFertilizer)
            {
                fertilizer = currentFertilizer;
                return fertilizer.AvailableUsesNumber > 0 &&
                       !potOnTable.IsPotTreatedByGrothAccelerator &&
                       potOnTable.FlowerGrowingLvl < flowersSettings.MaxFlowerGrowingLvl;
            }

            return false;
        }

        private void UseFertilizer()
        {
            fertilizer.TreatPot(potOnTable);
        }

        private bool CanPlayerTakePotInHands()
        {
            return !IsTableBroken && isPotOnTable && playerPickableObjectHandler.IsPickableObjectNull;
        }

        private void TakePotInPlayerHands()
        {
            weedPlanter.RemovePotFormPlantingWeedList(potOnTable);
            potOnTable.TakeInPlayerHandsFromGrowingTableAndSetPlayerFree();
            potOnTable = null;
            isPotOnTable = false;
            growingLightMeshRenderer.enabled = false;
            UseBreakableTable();
            tableObjectsRotation.PauseObjectsRotation();
        }

        private bool CanPlayerFixTable()
        {
            return playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer &&
                   IsTableBroken;
        }

        private void FixFlowerGrowingTable()
        {
            FixBreakableFlowerTable(
                repairsAndUpgradesSettings.FlowerGrowingTableMinQuantity * (tableLvl + 1),
                repairsAndUpgradesSettings.FlowerGrowingTableMaxQuantity * (tableLvl + 1));
        }

        private bool CanPlayerUpgradeTable()
        {
            return playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer &&
                   tableLvl < repairsAndUpgradesSettings.MaxUpgradableTableLvl;
        }
    }
}
