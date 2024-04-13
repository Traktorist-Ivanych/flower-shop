using System.Collections.Generic;
using FlowerShop.Ads;
using FlowerShop.Fertilizers;
using FlowerShop.PickableObjects;
using FlowerShop.Tables.Abstract;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class FertilizersTable : Table
    {
        [Inject] private readonly LevelPlayAds levelPlayAds;
        [Inject] private readonly FertilizersCanvasLiaison fertilizersCanvasLiaison;
        [Inject] private readonly FertilizersSetting fertilizersSetting;
        
        [SerializeField] private Transform fertilizersTableTransform;
        [SerializeField] private Fertilizer[] fertilizers;
        
        [HideInInspector, SerializeField] private MeshRenderer[] fertilizersMeshRenderers;

        private readonly List<Pot> activePots = new();
        private Fertilizer currentPlayerFertilizer;

        private void OnValidate()
        {
            fertilizersMeshRenderers = new MeshRenderer[fertilizers.Length];
            
            for (int i = 0; i < fertilizers.Length; i++)
            {
                fertilizersMeshRenderers[i] = fertilizers[i].GetComponent<MeshRenderer>();
            }
        }

        private void Start()
        {
            bool isAllFertilizersOnTable = true;

            for (int i = 0; i < fertilizers.Length; i++)
            {
                if (fertilizers[i].IsFertilizerInPlayerHands)
                {
                    isAllFertilizersOnTable = false;
                    fertilizersMeshRenderers[i].enabled = true;
                }
                else
                {
                    fertilizersMeshRenderers[i].enabled = false;
                }
            }

            if (isAllFertilizersOnTable)
            {
                fertilizersMeshRenderers[1].enabled = true;
            }
        }

        private protected override void TryInteractWithTable()
        {
            base.TryInteractWithTable();

            if (playerBusyness.IsPlayerFree)
            {
                if (CanPlayerTakeFertilizerInHands())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(ShowFertilizerCanvas);
                }
                else if (CanPlayerPutFertilizerOnTable())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(PutFertilizerOnTable);
                }
            }
        }

        public void TakeFertilizerInPlayerHands(Fertilizer fertilizerToTakePlayer)
        {
            for (int i = 0; i < fertilizers.Length; i++)
            {
                if (fertilizers[i].Equals(fertilizerToTakePlayer))
                {
                    fertilizersMeshRenderers[i].enabled = true;
                    fertilizers[i].TakeInPlayerHandsAndSetPlayerFree();
                }
                else
                {
                    fertilizersMeshRenderers[i].enabled = false; 
                }
            }
        }
        
        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerTakeFertilizerInHandsForSelectableEffect() || CanPlayerPutFertilizerOnTable();
        }
        
        public void IncreaseAvailableFertilizersUsesNumber()
        {
            foreach (Fertilizer fertilizer in fertilizers)
            {
                fertilizer.IncreaseAvailableUsesNumber(fertilizersSetting.IncreaseFertilizerAmount);
            }
        }
        
        public void IncreaseAvailableFertilizersUsesNumberByIAP()
        {
            foreach (Fertilizer fertilizer in fertilizers)
            {
                fertilizer.IncreaseAvailableUsesNumber(fertilizersSetting.IncreaseFertilizerAmountIAP);
            }
        }

        public void AddActivePot(Pot potToAdd)
        {
            activePots.Add(potToAdd);
        }
        
        public void RemoveActivePot(Pot potToAdd)
        {
            activePots.Remove(potToAdd);
        }
        
        private bool CanPlayerTakeFertilizerInHandsForSelectableEffect()
        {
            bool isFertilizersHaveEnoughUsages = false;
            bool canActivePotsBeTreated = false;
            
            foreach (Fertilizer fertilizer in fertilizers)
            {
                if (fertilizer.AvailableUsesNumber > 0)
                {
                    isFertilizersHaveEnoughUsages = true;
                }
            }
            
            foreach (Pot activePot in activePots)
            {
                if (activePot.CanPotBeTreated())
                {
                    canActivePotsBeTreated = true;
                    break;
                }
            }
            
            return isFertilizersHaveEnoughUsages && canActivePotsBeTreated &&
                   CanPlayerTakeFertilizerInHands();
        }

        private bool CanPlayerTakeFertilizerInHands()
        {
            return playerPickableObjectHandler.IsPickableObjectNull;
        }

        private void ShowFertilizerCanvas()
        {
            levelPlayAds.LoadRewardedAd();
            fertilizersCanvasLiaison.EnableCanvas();
        }

        private bool CanPlayerPutFertilizerOnTable()
        {
            if (playerPickableObjectHandler.CurrentPickableObject is Fertilizer currentFertilizer)
            {
                currentPlayerFertilizer = currentFertilizer;
                return true;
            }

            return false;
        }

        private void PutFertilizerOnTable()
        {
            currentPlayerFertilizer.PutOnTableAndSetPlayerFree(fertilizersTableTransform);
        }
    }
}