using FlowerShop.ComputerPages;
using FlowerShop.Education;
using FlowerShop.Effects;
using PlayerControl;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.RepairsAndUpgrades
{
    [RequireComponent(typeof(UIButton))]
    public class UpgradeCancelButton : MonoBehaviour
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly PlayerBusyness playerBusyness;
        [Inject] private readonly SelectedTableEffect selectedTableEffect;
        [Inject] private readonly UpgradeCanvasLiaison upgradeCanvasLiaison;
        
        [HideInInspector, SerializeField] private UIButton cancelButton;

        private void OnValidate()
        {
            cancelButton = GetComponent<UIButton>();
        }
        
        private void OnEnable()
        {
            cancelButton.OnClickEvent += OnUpgradeCancelButtonClick;
        }

        private void OnDisable()
        {
            cancelButton.OnClickEvent -= OnUpgradeCancelButtonClick;
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