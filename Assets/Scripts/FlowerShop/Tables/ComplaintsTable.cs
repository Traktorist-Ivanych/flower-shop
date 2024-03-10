﻿using System.Collections.Generic;
using DG.Tweening;
using FlowerShop.Customers;
using FlowerShop.Customers.VipAndComplaints;
using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Settings;
using FlowerShop.Tables.Abstract;
using FlowerShop.Tables.Interfaces;
using PlayerControl;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class ComplaintsTable : Table, ISavableObject, ISpecialSaleTable
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly ComplaintsHandler complaintsHandler;
        [Inject] private readonly CustomersSpawner customersSpawner;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly ReferencesForLoad referencesForLoad;
        
        [SerializeField] private MeshRenderer soilRenderer;
        [SerializeField] private MeshRenderer flowerRenderer;
        
        [HideInInspector, SerializeField] private Transform soilTransform;
        
        private FlowerInfo flowerInfoForComplaint;
        private Pot potForComplaint;
        private bool isPotOnTable;
        private string flowerInfoOnTableUniqueKey;
        
        [field: SerializeField] public Transform TablePotTransform { get; private set; }
        [field: HideInInspector, SerializeField] public MeshFilter SoilMeshFilter { get; private set; }
        [field: HideInInspector, SerializeField] public MeshFilter FlowerMeshFilter { get; private set; }
        [field: SerializeField] public string UniqueKey { get; private set; }
        [field: SerializeField] public List<Transform> ToFlowerPathPoints { get; private set; }
        [field: SerializeField] public List<Transform> FinishWithFlowerPathPoints { get; private set; }
        
        private void OnValidate()
        {
            SoilMeshFilter = soilRenderer.GetComponent<MeshFilter>();
            FlowerMeshFilter = flowerRenderer.GetComponent<MeshFilter>();
            soilTransform = soilRenderer.GetComponent<Transform>();
        }

        private void Awake()
        {
            Load();
        }

        public override void ExecuteClickableAbility()
        {
            base.ExecuteClickableAbility();

            if (CanPlayerUseComplaintsTable())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(UseComplaintsTable);
            }
        }
        
        public void ExecuteSpecialSale()
        {
            isPotOnTable = false;
            soilRenderer.enabled = false;
            flowerRenderer.enabled = false;
            
            flowerInfoForComplaint = flowersSettings.FlowerInfoEmpty;
            flowerInfoOnTableUniqueKey = flowersSettings.FlowerInfoEmpty.UniqueKey;
            
            SavesHandler.DeletePlayerPrefsKey(UniqueKey);
        }

        public void Load()
        {
            FlowerInfoReferenceForSaving flowerInfoReferenceForLoading =
                SavesHandler.Load<FlowerInfoReferenceForSaving>(UniqueKey);

            if (flowerInfoReferenceForLoading.IsValuesSaved)
            {
                flowerInfoOnTableUniqueKey = flowerInfoReferenceForLoading.FlowerInfoOnTableUniqueKey;

                flowerInfoForComplaint = referencesForLoad.GetReference<FlowerInfo>(flowerInfoOnTableUniqueKey);

                if (flowerInfoForComplaint != flowersSettings.FlowerInfoEmpty)
                {
                    SetFlowerOnSaleTable();
                }
            }
        }

        public void Save()
        {
            FlowerInfoReferenceForSaving flowerInfoReferenceForSaving = new(flowerInfoOnTableUniqueKey);
            
            SavesHandler.Save(UniqueKey, flowerInfoReferenceForSaving);
        }

        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerUseComplaintsTable();
        }

        private bool CanPlayerUseComplaintsTable()
        {
            if (playerBusyness.IsPlayerFree && playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                potForComplaint = currentPot;
                
                return !isPotOnTable && !potForComplaint.IsWeedInPot &&
                       potForComplaint.PlantedFlowerInfo.Equals(complaintsHandler.ComplaintFlowerInfo) &&
                       potForComplaint.FlowerGrowingLvl >= flowersSettings.MaxFlowerGrowingLvl;
            }
            
            return false;
        }

        private void UseComplaintsTable()
        {
            flowerInfoForComplaint = potForComplaint.PlantedFlowerInfo;
            potForComplaint.CleanPot();
            flowerInfoOnTableUniqueKey = flowerInfoForComplaint.UniqueKey;
            
            SetFlowerOnSaleTable();
            complaintsHandler.CompleteComplaint();

            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.ThrowTrigger);
            
            soilTransform.SetPositionAndRotation(
                position: playerComponents.PlayerHandsForBigObjectTransform.position,
                rotation: playerComponents.PlayerHandsForBigObjectTransform.rotation);

            soilTransform.DOJump(
                    endValue: TablePotTransform.position, 
                    jumpPower: actionsWithTransformSettings.PickableObjectDoTweenJumpPower, 
                    numJumps: actionsWithTransformSettings.DefaultDoTweenJumpsNumber, 
                    duration: actionsWithTransformSettings.MovingPickableObjectTime)
                .OnComplete(() => playerBusyness.SetPlayerFree());

            Save();
        }
        
        private void SetFlowerOnSaleTable()
        {
            isPotOnTable = true;
            customersSpawner.AddSpecialSaleTable(this);
            
            soilRenderer.enabled = true;
            SoilMeshFilter.mesh = flowerInfoForComplaint.FlowerSoilMesh;
            flowerRenderer.enabled = true;
            FlowerMeshFilter.mesh = flowerInfoForComplaint.GetFlowerLvlMesh(flowersSettings.MaxFlowerGrowingLvl);
        }
    }
}