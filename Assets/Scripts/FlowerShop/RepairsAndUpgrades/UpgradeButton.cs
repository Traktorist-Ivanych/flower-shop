using FlowerShop.Education;
using FlowerShop.PickableObjects;
using PlayerControl;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.RepairsAndUpgrades
{
    [RequireComponent(typeof(Button))]
    public class UpgradeButton : MonoBehaviour
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly PlayerMoney playerMoney;
        [Inject] private readonly UpgradeCanvasLiaison upgradeCanvasLiaison;
        [Inject] private readonly RepairingAndUpgradingHammer repairingAndUpgradingHammer;

        [SerializeField] private Button upgradeButton;

        private void OnValidate()
        {
            upgradeButton = GetComponent<Button>();
        }

        private void OnEnable()
        {
            upgradeButton.onClick.AddListener(OnUpgradeButtonClick);
        }

        private void OnDisable()
        {
            upgradeButton.onClick.RemoveListener(OnUpgradeButtonClick);
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
