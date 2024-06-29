using FlowerShop.ComputerPages;
using FlowerShop.Education;
using FlowerShop.PickableObjects;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.RepairsAndUpgrades
{
    [RequireComponent(typeof(UIButton))]
    public class UpgradeButton : MonoBehaviour
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly PlayerMoney playerMoney;
        [Inject] private readonly UpgradeCanvasLiaison upgradeCanvasLiaison;
        [Inject] private readonly RepairingAndUpgradingHammer repairingAndUpgradingHammer;

        [SerializeField] private UIButton upgradeButton;

        private void OnValidate()
        {
            upgradeButton = GetComponent<UIButton>();
        }

        private void OnEnable()
        {
            upgradeButton.OnClickEvent += OnUpgradeButtonClick;
        }

        private void OnDisable()
        {
            upgradeButton.OnClickEvent -= OnUpgradeButtonClick;
        }

        private void OnUpgradeButtonClick()
        {
            if (upgradeCanvasLiaison.PriceInt <= playerMoney.CurrentPlayerMoney)
            {
                StartCoroutine(repairingAndUpgradingHammer.ImproveTable());
                upgradeCanvasLiaison.DisableCanvas();

                if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
                {
                    educationHandler.CompleteEducationStep();
                }
            }
        }
    }
}
