using FlowerShop.ComputerPages;
using FlowerShop.Customers.VipAndComplaints;
using FlowerShop.Tables.Abstract;
using Input;
using PlayerControl;
using Zenject;

namespace FlowerShop.Tables
{
    public class ComputerTable : Table
    {
        [Inject] private readonly ComplaintsHandler complaintsHandler;
        [Inject] private readonly ComputerMainPageCanvasLiaison computerMainPageCanvasLiaison;
        [Inject] private readonly PlayerAnimationEvents playerAnimationEvents;
        [Inject] private readonly PlayerInputActions playerInputActions;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly VipOrdersHandler vipOrdersHandler;

        private protected override void TryInteractWithTable()
        {
            base.TryInteractWithTable();

            if (CanPlayerUseComputer())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(UseComputer);
            }
        }

        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerUseComputerForSelectedEffect();
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

        private void EnableComputerMainPage()
        {
            playerInputActions.EnableCanvasControlMode();
            computerMainPageCanvasLiaison.ComputerMainPageCanvas.enabled = true;
        }
    }
}