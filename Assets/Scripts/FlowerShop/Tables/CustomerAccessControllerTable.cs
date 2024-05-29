using System.Collections;
using DG.Tweening;
using FlowerShop.Achievements;
using FlowerShop.FlowersSale;
using FlowerShop.Help;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Settings;
using FlowerShop.Tables.Abstract;
using PlayerControl;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class CustomerAccessControllerTable : Table, ISavableObject
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly FlowersForSaleCoeffCalculator flowersForSaleCoeffCalculator;
        [Inject] private readonly FlowersForSaleCoeffCalculatorSettings flowersForSaleCoeffCalculatorSettings;
        [Inject] private readonly FlowersSaleTablesForCustomers flowersSaleTablesForCustomers;
        [Inject] private readonly HelpCanvasLiaison helpCanvasLiaison;
        [Inject] private readonly HelpTexts helpTexts;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly Sprinter sprinter;

        [SerializeField] private Transform customerAccessSign;
        [SerializeField] private Transform signOpen;
        [SerializeField] private Transform signClose;
        
        public bool IsAccessOpen { get; private set; }
        [field: SerializeField] public string UniqueKey { get; private set; }

        private void Awake()
        {
            Load();
        }

        public void Save()
        {
            BoolForSaving boolForSaving = new BoolForSaving(IsAccessOpen);
            
            SavesHandler.Save(UniqueKey, boolForSaving);
        }

        public void Load()
        {
            BoolForSaving boolForLoading = SavesHandler.Load<BoolForSaving>(UniqueKey);

            if (boolForLoading.IsValuesSaved)
            {
                IsAccessOpen = boolForLoading.SavingBool;

                if (IsAccessOpen)
                {
                    customerAccessSign.rotation = signOpen.rotation;
                }
            }
        }
        
        private protected override void TryInteractWithTable()
        {
            base.TryInteractWithTable();

            if (CanPlayerUseTable())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(() => StartCoroutine(UseTable()));

                if (flowersForSaleCoeffCalculator.CalculateCurrentGrade() < flowersForSaleCoeffCalculatorSettings.MaxShopGrade &&
                    !IsAccessOpen)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.IncreaseNumberOfFlowersForSale);
                }
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

        private void TryToShowHelpCanvas()
        {
            if (!playerBusyness.IsPlayerFree)
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.PlayerBusy);
            }
            else if (!playerPickableObjectHandler.IsPickableObjectNull)
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.HandsFull);
            }
        }

        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerUseTableForSelectedEffect() || CanPlayerUseTableInfoCanvas();
        }

        private bool CanPlayerUseTable()
        {
            return playerBusyness.IsPlayerFree && playerPickableObjectHandler.IsPickableObjectNull;
        }

        private bool CanPlayerUseTableForSelectedEffect()
        {
            if (IsAccessOpen)
            {
                return flowersSaleTablesForCustomers.GetCurrentSaleTableWithFlowerCount() < 3 && CanPlayerUseTable(); 
            }
            
            return flowersSaleTablesForCustomers.GetCurrentSaleTableWithFlowerCount() > 6 && CanPlayerUseTable();
        }

        private bool CanPlayerUseTableInfoCanvas()
        {
            return playerBusyness.IsPlayerFree && playerPickableObjectHandler.CurrentPickableObject is InfoBook;
        }

        private void UseTableInfoCanvas()
        {
            tableInfoCanvasLiaison.ShowCanvas(tableInfo, growingRoom);
        }
        private IEnumerator UseTable()
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartCrossingTrigger);

            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            
            RotateCustomerAccessSign();
            IsAccessOpen = !IsAccessOpen;
            selectedTableEffect.ActivateEffectWithDelay();
            playerBusyness.SetPlayerFree();

            if (!IsAccessOpen)
            {
                sprinter.SetProgress(0);
            }
            
            Save();
        }

        private void RotateCustomerAccessSign()
        {
            Vector3 rotationTarget;
            
            if (IsAccessOpen)
            {
                rotationTarget = signClose.localRotation.eulerAngles;
            }
            else
            {
                rotationTarget = signOpen.localRotation.eulerAngles;
            }

            customerAccessSign.DOLocalRotate(rotationTarget,
                actionsWithTransformSettings.MovingPickableObjectTime,
                RotateMode.FastBeyond360);
        }
    }
}