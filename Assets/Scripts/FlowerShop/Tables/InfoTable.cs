using FlowerShop.Help;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Tables.Abstract;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class InfoTable : Table, ISavableObject
    {
        [Inject] private readonly HelpCanvasLiaison helpCanvasLiaison;
        [Inject] private readonly HelpTexts helpTexts;

        [SerializeField] private Transform infoBookOnTableTransform;
        [SerializeField] private InfoBook infoBook;

        private bool isInfoBookInPlayerHands;

        [field: SerializeField] public string UniqueKey { get; private set; }

        private void Awake()
        {
            Load();
        }
        private void Start()
        {
            if (isInfoBookInPlayerHands)
            {
                infoBook.LoadInPlayerHands();
            }
        }

        public void Load()
        {
            BoolForSaving repairsAndUpgradesTableForLoading =
                SavesHandler.Load<BoolForSaving>(UniqueKey);

            if (repairsAndUpgradesTableForLoading.IsValuesSaved &&
                repairsAndUpgradesTableForLoading.SavingBool)
            {
                isInfoBookInPlayerHands = true;
            }
        }

        public void Save()
        {
            BoolForSaving boolForSaving =
                new(isInfoBookInPlayerHands);

            SavesHandler.Save(UniqueKey, boolForSaving);
        }

        private protected override void TryInteractWithTable()
        {
            base.TryInteractWithTable();

            if (playerBusyness.IsPlayerFree)
            {
                if (CanPlayerTakeInfoBookInHands())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(TakeInfoBookInPlayerHands);
                }
                else if (CanPlayerPutInfoBookOnTable())
                {
                    SetPlayerDestinationAndOnPlayerArriveAction(PutInfoBookOnTable);
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
            helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.WrongPickableObject);
        }

        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerTakeInfoBookInHands() ||
                   CanPlayerPutInfoBookOnTable();
        }

        private bool CanPlayerTakeInfoBookInHands()
        {
            return playerPickableObjectHandler.IsPickableObjectNull;
        }

        private void TakeInfoBookInPlayerHands()
        {
            infoBook.TakeInPlayerHandsAndSetPlayerFree();
            isInfoBookInPlayerHands = true;

            Save();
        }

        private bool CanPlayerPutInfoBookOnTable()
        {
            return playerPickableObjectHandler.CurrentPickableObject is InfoBook;
        }

        private void PutInfoBookOnTable()
        {
            playerPickableObjectHandler.ResetPickableObject();
            infoBook.PutOnTableAndSetPlayerFree(infoBookOnTableTransform);
            isInfoBookInPlayerHands = false;

            Save();
        }
    }
}
