using FlowerShop.Effects;
using PlayerControl;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.Fertilizers
{
    [RequireComponent(typeof(Button))]
    public class BuyFertilizersCancelButton : MonoBehaviour
    {
        [Inject] private readonly PlayerBusyness playerBusyness;
        [Inject] private readonly FertilizersCanvasLiaison fertilizersCanvasLiaison;
        [Inject] private readonly SelectedTableEffect selectedTableEffect;
        
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
            fertilizersCanvasLiaison.FertilizersCanvas.enabled = false;
            playerBusyness.SetPlayerFree();
            selectedTableEffect.ActivateEffectWithDelay();
        }
    }
}