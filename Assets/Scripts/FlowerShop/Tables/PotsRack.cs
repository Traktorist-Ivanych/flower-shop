using System.Collections.Generic;
using System.Linq;
using FlowerShop.Flowers;
using FlowerShop.Help;
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
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly HelpCanvasLiaison helpCanvasLiaison;
        [Inject] private readonly HelpTexts helpTexts;
        [Inject] private readonly TablesSettings tablesSettings;
        
        [SerializeField] private Transform potObjectsTransform;
        [SerializeField] private FlowersCrossingTableProcess wildCrossingTableProcess;
        [SerializeField] private FlowersCrossingTableProcess roomCrossingTableProcess;
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

        private protected override void TryInteractWithTable()
        {
            base.TryInteractWithTable();
            
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
            if (playerPickableObjectHandler.IsPickableObjectNull)
            {
                if (currentFreePots <= 0)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.NotEnoughPots);
                }
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

        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerTakePotInHandsForSelectedEffect() || CanPlayerPutPotOnTable() || 
                   CanPlayerUpgradeTableForSelectableEffect() || CanPlayerUseTableInfoCanvas();
        }

        public override void UpgradeTableFinish()
        {
            base.UpgradeTableFinish();

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

        private bool CanPlayerTakePotInHandsForSelectedEffect()
        {
            if (growingRoom == flowersSettings.GrowingRoomWild)
            {
                return CanPlayerTakePotInHands();
            }

            if ((wildCrossingTableProcess.IsCrossingSeedReady &&
                 wildCrossingTableProcess.FlowerInfoForPlanting.GrowingRoom == growingRoom) ||
                (roomCrossingTableProcess.IsCrossingSeedReady &&
                 roomCrossingTableProcess.FlowerInfoForPlanting.GrowingRoom == growingRoom))
            {
                return CanPlayerTakePotInHands();
            }

            return false;
        }

        private void TakePotInPlayerHands()
        {
            potsRenderers[currentFreePots - 1].enabled = false;
            currentFreePots--;
            pots.Last().TakeInPlayerHandsAndSetPlayerFree();
            pots.Remove(pots.Last());
        }

        private bool CanPlayerUseTableInfoCanvas()
        {
            return playerPickableObjectHandler.CurrentPickableObject is InfoBook;
        }

        private void UseTableInfoCanvas()
        {
            tableInfoCanvasLiaison.ShowCanvas(tableInfo, growingRoom);
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



        public void Load(PotsRackForSaving potsRackForLoading) // CutScene
        {
            if (potsRackForLoading.IsValuesSaved)
            {
                tableLvl = potsRackForLoading.TableLvl;
                LoadLvlMesh();
            }

            currentFreePots += (tableLvl + 1) * tablesSettings.PotsCountAvailableOnUpgradeDelta;
        }
    }
}
