using FlowerShop.Education;
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
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly PlayerBusyness playerBusyness;
        [Inject] private readonly SelectedTableEffect selectedTableEffect;
        [Inject] private readonly UpgradeCanvasLiaison upgradeCanvasLiaison;
        
        [HideInInspector, SerializeField] private Button cancelButton;

        private void OnValidate()
        {
            cancelButton = GetComponent<Button>();
        }
        
        private void OnEnable()
        {
            cancelButton.onClick.AddListener(OnUpgradeCancelButtonClick);
        }

        private void OnDisable()
        {
            cancelButton.onClick.RemoveListener(OnUpgradeCancelButtonClick);
        }

        private void OnUpgradeCancelButtonClick()
        {
            if (!educationHandler.IsEducationActive)
            {
                upgradeCanvasLiaison.DisableCanvas();
                playerBusyness.SetPlayerFree();
                selectedTableEffect.ActivateEffectWithoutDelay();
            }
        }
    }
}