using FlowerShop.Fertilizers;
using FlowerShop.Tables.Abstract;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class FertilizersTable : Table
    {
        [Inject] private readonly FertilizersCanvasLiaison fertilizersCanvasLiaison;
        
        [SerializeField] private Transform fertilizersTableTransform;
        [SerializeField] private Fertilizer[] fertilizers;
        
        [HideInInspector, SerializeField] private MeshRenderer[] fertilizersMeshRenderers;

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

        public override void ExecuteClickableAbility()
        {
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

        public void IncreaseAvailableFertilizersUsesNumber()
        {
            foreach (Fertilizer fertilizer in fertilizers)
            {
                fertilizer.IncreaseAvailableUsesNumber();
            }
        }

        private bool CanPlayerTakeFertilizerInHands()
        {
            return playerPickableObjectHandler.IsPickableObjectNull;
        }

        private void ShowFertilizerCanvas()
        {
            fertilizersCanvasLiaison.FertilizersCanvas.enabled = true;
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