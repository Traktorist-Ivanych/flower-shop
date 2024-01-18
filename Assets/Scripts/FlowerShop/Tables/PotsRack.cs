using System.Collections.Generic;
using System.Linq;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Tables.Abstract;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class PotsRack : UpgradableTable, ISavableObject
    {
        [Inject] private readonly TablesSettings tablesSettings;
        
        [SerializeField] private Transform potObjectsTransform;
        [SerializeField] private List<Pot> pots;
        [SerializeField] private List<MeshRenderer> potsRenderers;

        private Pot currentPlayerPot;
        private int currentFreePots;

        [field: SerializeField] public string UniqueKey { get; private set; }

        private protected override void Awake()
        {
            base.Awake();

            Load();
        }

        private void Start()
        {
            for (int i = 0; i < currentFreePots; i++)
            {
                potsRenderers[i].enabled = true;
            }
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
            
            Save();
        }

        public void RemovePotFromListOnLoad(Pot potForRemoving)
        {
            if (pots.Contains(potForRemoving))
            {
                currentFreePots--;
                pots.Remove(potForRemoving);
            }
        }

        public void Save()
        {
            PotsRackForSaving potsRackForSaving = new(tableLvl);
            
            SavesHandler.Save(UniqueKey, potsRackForSaving);
        }

        public void Load()
        {
            PotsRackForSaving potsRackForLoading = SavesHandler.Load<PotsRackForSaving>(UniqueKey);

            if (potsRackForLoading.IsValuesSaved)
            {
                tableLvl = potsRackForLoading.TableLvl;
                LoadLvlMesh();
            }
            
            currentFreePots += (tableLvl + 1) * tablesSettings.PotsCountAvailableOnUpgradeDelta;
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
