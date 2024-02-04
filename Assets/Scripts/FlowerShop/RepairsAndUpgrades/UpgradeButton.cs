using FlowerShop.PickableObjects;
using PlayerControl;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.RepairsAndUpgrades
{
    public class UpgradeButton : MonoBehaviour
    {
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
                upgradeCanvasLiaison.UpgradeCanvas.enabled = false;
            }
        }
    }
}
