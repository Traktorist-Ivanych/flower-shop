using FlowerShop.ComputerPages;
using FlowerShop.Customers.VipAndComplaints;
using FlowerShop.Help;
using FlowerShop.PickableObjects;
using FlowerShop.Sounds;
using FlowerShop.Tables.Abstract;
using Input;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class ComputerTable : Table
    {
        [Inject] private readonly ComplaintsHandler complaintsHandler;
        [Inject] private readonly ComputerMainPageCanvasLiaison computerMainPageCanvasLiaison;
        [Inject] private readonly HelpCanvasLiaison helpCanvasLiaison;
        [Inject] private readonly HelpTexts helpTexts;
        [Inject] private readonly PlayerAnimationEvents playerAnimationEvents;
        [Inject] private readonly PlayerInputActions playerInputActions;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly SoundsHandler soundsHandler;
        [Inject] private readonly VipOrdersHandler vipOrdersHandler;

        [SerializeField] private MeshRenderer infoIndicator;

        private int infoIndicatorCount;

        public void ShowIndicator()
        {
            if (infoIndicatorCount == 0)
            {
                infoIndicator.enabled = true;
            }

            infoIndicatorCount++;
            soundsHandler.PlayComputerTableIndicator();
        }
        
        public void HideIndicator()
        {
            infoIndicatorCount--;

            if (infoIndicatorCount == 0)
            {
                infoIndicator.enabled = false;
            }
        }
        
        private protected override void TryInteractWithTable()
        {
            base.TryInteractWithTable();

            if (CanPlayerUseComputer())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(UseComputer);
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
            return CanPlayerUseComputerForSelectedEffect() || CanPlayerUseTableInfoCanvas();
        }

        private bool CanPlayerUseComputerForSelectedEffect()
        {
            return (complaintsHandler.IsComplaintActive || vipOrdersHandler.IsVipOrderActive) && 
                   CanPlayerUseComputer();
        }

        private bool CanPlayerUseComputer()
        {
            return playerBusyness.IsPlayerFree && playerPickableObjectHandler.IsPickableObjectNull;
        }

        private void UseComputer()
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartUsingComputer);
            playerAnimationEvents.SetCurrentAnimationEvent(EnableComputerMainPage);
        }

        private bool CanPlayerUseTableInfoCanvas()
        {
            return playerBusyness.IsPlayerFree && playerPickableObjectHandler.CurrentPickableObject is InfoBook;
        }

        private void UseTableInfoCanvas()
        {
            tableInfoCanvasLiaison.ShowCanvas(tableInfo, growingRoom);
        }

        private void EnableComputerMainPage()
        {
            playerInputActions.EnableCanvasControlMode();
            computerMainPageCanvasLiaison.ComputerMainPageCanvas.enabled = true;
        }
    }
}