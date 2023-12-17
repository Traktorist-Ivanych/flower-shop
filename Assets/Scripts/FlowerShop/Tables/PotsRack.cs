using System.Collections.Generic;
using System.Linq;
using FlowerShop.PickableObjects;
using FlowerShop.Tables.Abstract;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class PotsRack : UpgradableTable
    {
        [Inject] private readonly TablesSettings tablesSettings;
        
        [SerializeField] private Transform potObjectsTransform;
        [SerializeField] private List<Pot> pots;
        [SerializeField] private List<MeshRenderer> potsRenderers;

        private Pot currentPlayerPot;
        private int currentFreePots;

        private protected override void Start()
        {
            base.Start();

            currentFreePots = tablesSettings.PotsCountAvailableOnStart;
        }
        
        public override void ExecuteClickableAbility()
        {
            if (playerBusyness.IsPlayerFree)
            {
                if (CanPlayerTakePotInHands())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(TakePotInPlayerHands);
                }
                else if (CanPlayerPutPotOnTable())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(PutPotOnTable);
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

            currentFreePots += tablesSettings.PotsCountAvailableOnUpgradeDelta;
            for (int i = 0; i < currentFreePots; i++)
            {
                potsRenderers[i].enabled = true;
            }
        }

        private bool CanPlayerTakePotInHands()
        {
            return playerPickableObjectHandler.IsPickableObjectNull && currentFreePots > 0;
        }

        private void TakePotInPlayerHands()
        {
            potsRenderers[currentFreePots - 1].enabled = false;
            currentFreePots--;
            pots.Last().TakeInPlayerHandsAndSetPlayerFree();
            pots.Remove(pots.Last());
        }

        private bool CanPlayerPutPotOnTable()
        {
            if (playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                currentPlayerPot = currentPot;
                
                return !currentPlayerPot.IsSoilInsidePot && currentPlayerPot.GrowingRoom == growingRoom;
            }

            return false;
        }

        private void PutPotOnTable()
        {
            currentFreePots++;
            potsRenderers[currentFreePots - 1].enabled = true;
            pots.Add(currentPlayerPot);
            currentPlayerPot.PutOnTableAndSetPlayerFree(potObjectsTransform);
            playerPickableObjectHandler.ResetPickableObject();
        }

        private bool CanPlayerUpgradeTable()
        {
            return playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer &&
                   tableLvl < repairsAndUpgradesSettings.MaxUpgradableTableLvl;
        }
    }
}
