using FlowerShop.Ads;
using FlowerShop.ComputerPages;
using Input;
using UnityEngine;
using Zenject;

namespace FlowerShop.Money
{
    [RequireComponent(typeof(UIButton))]
    public class OpenAddMoneyCanvas : MonoBehaviour
    {
        [Inject] private readonly LevelPlayAds levelPlayAds;
        [Inject] private readonly PlayerInputActions playerInputActions;

        [SerializeField] private Canvas addMoneyCanvas;
        
        [HideInInspector, SerializeField] private UIButton uiButton;
        
        private void OnValidate()
        {
            uiButton = GetComponent<UIButton>();
        }

        private void OnEnable()
        {
            uiButton.OnClickEvent += OnButtonClick;
        }
        
        private void OnDisable()
        {
            uiButton.OnClickEvent -= OnButtonClick;
        }

        private void OnButtonClick()
        {
            levelPlayAds.LoadRewardedAd();
            addMoneyCanvas.enabled = true;
            playerInputActions.EnableCanvasControlMode();
        }
    }
}
