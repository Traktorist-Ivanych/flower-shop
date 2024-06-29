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
    public class FlowersCrossingTable : Table, ISavableObject
    {
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly HelpCanvasLiaison helpCanvasLiaison;
        [Inject] private readonly HelpTexts helpTexts;
        [Inject] private readonly ReferencesForLoad referencesForLoad;

        [SerializeField] private Transform tablePotTransform;
        [SerializeField] private FlowersCrossingTableProcess flowersCrossingTableProcess;
        [SerializeField] private ParticleSystem crossingFlowerPS;

        [field: SerializeField] public string UniqueKey { get; private set; }

        public Pot PotOnTable { get; private set; }
        public bool IsPotOnCrossingTable { get; private set; }

        private void Awake()
        {
            Load();
        }

        private void Start()
        {
            flowersCrossingTableProcess.CheckCrossingAbility();
        }

        private protected override void TryInteractWithTable()
        {
            base.TryInteractWithTable();

            if (playerBusyness.IsPlayerFree)
            {
                if (CanPlayerPutPotOnTable())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(PutPotOnTable);
                }
                else if (CanPlayerTakePotInHands())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(TakePotInPlayerHands);
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
            if (flowersCrossingTableProcess.IsTableBroken)
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.BrokenTable);
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                if (currentPot.GrowingRoom != growingRoom)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.MismatchGrowingRoom);
                }
                else if (IsPotOnCrossingTable)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.TableAlreadyHasPot);
                }
                else if (currentPot.PlantedFlowerInfo == flowersSettings.FlowerInfoEmpty)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.NoFlowerPlanted);
                }
                else if (currentPot.FlowerGrowingLvl < flowersSettings.MaxFlowerGrowingLvl)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.FlowerDidNotGrow);
                }
                else if (currentPot.IsWeedInPot)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.WeedInPot);
                }
                else if (currentPot.PlantedFlowerInfo.FlowerLvl == flowersSettings.MediumFlowerLvlForTableLvl - 1 &&
                    flowersCrossingTableProcess.TableLvl < flowersSettings.MediumTableLvlForFlowerLvl)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.InconsistencyFlowerAndCrossingTableLvl);
                }
                else if (currentPot.PlantedFlowerInfo.FlowerLvl == flowersSettings.MaxFlowerLvlForTableLvl - 1 &&
                    flowersCrossingTableProcess.TableLvl < flowersSettings.MaxTableLvlForFlowerLvl)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.InconsistencyFlowerAndCrossingTableLvl);
                }
            }
            else if (playerPickableObjectHandler.IsPickableObjectNull)
            {
                if (!IsPotOnCrossingTable)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.EmptyTable);
                }
                else if (flowersCrossingTableProcess.IsSeedCrossing)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.SeedCrossing);
                }
            }
            else
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.WrongPickableObject);
            }
        }

        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerPutPotOnTable() || CanPlayerTakePotInHandsForSelectedEffect() || CanPlayerUseTableInfoCanvas();
        }

        public void PlayCrossingFlowerEffects()
        {
            crossingFlowerPS.Play();
        }

        public void StopCrossingFlowerEffects()
        {
            crossingFlowerPS.Stop();
        }

        public void Load()
        {
            FlowersCrossingTableForSaving flowersCrossingTableForLoading =
                SavesHandler.Load<FlowersCrossingTableForSaving>(UniqueKey);

            if (flowersCrossingTableForLoading.IsValuesSaved)
            {
                PotOnTable = referencesForLoad.GetReference<Pot>(flowersCrossingTableForLoading.PotUniqueKey);

                if (PotOnTable != null)
                {
                    PotOnTable.LoadOnTable(tablePotTransform);
                    IsPotOnCrossingTable = true;
                }
            }
        }

        public void Save()
        {
            string potOnTableUniqueKey = "Empty";

            if (PotOnTable)
            {
                potOnTableUniqueKey = PotOnTable.UniqueKey;
            }

            FlowersCrossingTableForSaving flowersCrossingTableForSaving = new(potOnTableUniqueKey);

            SavesHandler.Save(UniqueKey, flowersCrossingTableForSaving);
        }

        private bool CanPlayerPutPotOnTable()
        {
            if (!IsPotOnCrossingTable && playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                PotOnTable = currentPot;

                if (PotOnTable.FlowerGrowingLvl < flowersSettings.MaxFlowerGrowingLvl)
                {
                    return false;
                }
                else
                {
                    if (PotOnTable.PlantedFlowerInfo.FlowerLvl == flowersSettings.MediumFlowerLvlForTableLvl - 1 &&
                        flowersCrossingTableProcess.TableLvl < flowersSettings.MediumTableLvlForFlowerLvl)
                    {
                        return false;
                    }
                    else if (PotOnTable.PlantedFlowerInfo.FlowerLvl == flowersSettings.MaxFlowerLvlForTableLvl - 1 &&
                        flowersCrossingTableProcess.TableLvl < flowersSettings.MaxTableLvlForFlowerLvl)
                    {
                        return false;
                    }
                    else
                    {
                        return PotOnTable.GrowingRoom == growingRoom &&
                               !PotOnTable.IsWeedInPot &&
                               !flowersCrossingTableProcess.IsTableBroken;
                    }
                }
            }

            return false;
        }

        private void PutPotOnTable()
        {
            PotOnTable.PutOnTableAndSetPlayerFree(tablePotTransform);
            playerPickableObjectHandler.ResetPickableObject();
            IsPotOnCrossingTable = true;
            flowersCrossingTableProcess.CheckCrossingAbility();

            Save();
        }

        private bool CanPlayerTakePotInHandsForSelectedEffect()
        {
            return CanPlayerTakePotInHands() && PotOnTable.FlowerGrowingLvl < flowersSettings.MaxFlowerGrowingLvl;
        }

        private bool CanPlayerTakePotInHands()
        {
            return IsPotOnCrossingTable &&
                   !flowersCrossingTableProcess.IsSeedCrossing &&
                   playerPickableObjectHandler.IsPickableObjectNull;
        }

        private bool CanPlayerUseTableInfoCanvas()
        {
            return !flowersCrossingTableProcess.IsTableBroken && playerPickableObjectHandler.CurrentPickableObject is InfoBook;
        }

        private void UseTableInfoCanvas()
        {
            tableInfoCanvasLiaison.ShowCanvas(tableInfo, growingRoom);
        }

        private void TakePotInPlayerHands()
        {
            IsPotOnCrossingTable = false;
            flowersCrossingTableProcess.CheckCrossingAbility();
            PotOnTable.TakeInPlayerHandsAndSetPlayerFree();
            PotOnTable = null;

            SavesHandler.DeletePlayerPrefsKey(UniqueKey);
        }



        public void Load(FlowersCrossingTableForSaving flowersCrossingTableForLoading) // CutScene
        {
            if (flowersCrossingTableForLoading.IsValuesSaved)
            {
                PotOnTable = referencesForLoad.GetReference<Pot>(flowersCrossingTableForLoading.PotUniqueKey);

                if (PotOnTable != null)
                {
                    PotOnTable.LoadOnTable(tablePotTransform);
                    IsPotOnCrossingTable = true;
                }
            }
        }
    }
}
