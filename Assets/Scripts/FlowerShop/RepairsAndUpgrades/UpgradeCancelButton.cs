using FlowerShop.Effects;
using PlayerControl;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.RepairsAndUpgrades
{
    [RequireComponent(typeof(Button))]
    public class UpgradeCancelButton : MonoBehaviour
    {
        [Inject] private readonly PlayerBusyness playerBusyness;
        [Inject] private readonly SelectedTableEffect selectedTableEffect;
        [Inject] private readonly UpgradeCanvasLiaison upgradeCanvasLiaison;
        
        [HideInInspector, SerializeField] private Button canselButton;

        private void OnValidate()
        {
            canselButton = GetComponent<Button>();
        }
        
        private void OnEnable()
        {
            canselButton.onClick.AddListener(OnUpgradeCancelButtonClick);
        }

        private void OnDisable()
        {
            canselButton.onClick.RemoveListener(OnUpgradeCancelButtonClick);
        }

        private void OnUpgradeCancelButtonClick()
        {
            upgradeCanvasLiaison.UpgradeCanvas.enabled = false;
            playerBusyness.SetPlayerFree();
            selectedTableEffect.ActivateEffectWithoutDelay();
        }
    }
}