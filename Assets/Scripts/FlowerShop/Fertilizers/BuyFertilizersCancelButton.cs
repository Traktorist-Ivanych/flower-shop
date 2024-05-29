using FlowerShop.ComputerPages;
using FlowerShop.Education;
using FlowerShop.Effects;
using PlayerControl;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.Fertilizers
{
    [RequireComponent(typeof(UIButton))]
    public class BuyFertilizersCancelButton : MonoBehaviour
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly PlayerBusyness playerBusyness;
        [Inject] private readonly FertilizersCanvasLiaison fertilizersCanvasLiaison;
        [Inject] private readonly SelectedTableEffect selectedTableEffect;
        
        [HideInInspector, SerializeField] private UIButton canselButton;

        private void OnValidate()
        {
            canselButton = GetComponent<UIButton>();
        }
        
        private void OnEnable()
        {
            canselButton.OnClickEvent += OnUpgradeCancelButtonClick;
        }

        private void OnDisable()
        {
            canselButton.OnClickEvent -= OnUpgradeCancelButtonClick;
        }

        private void OnUpgradeCancelButtonClick()
        {
            if (!educationHandler.IsEducationActive)
            {
                fertilizersCanvasLiaison.DisableCanvas();
                playerBusyness.SetPlayerFree();
                selectedTableEffect.ActivateEffectWithDelay();
            }
        }
    }
}