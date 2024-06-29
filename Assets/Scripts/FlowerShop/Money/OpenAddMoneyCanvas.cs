using FlowerShop.ComputerPages;
using UnityEngine;

namespace FlowerShop.Money
{
    [RequireComponent(typeof(UIButton))]
    public class OpenAddMoneyCanvas : MonoBehaviour
    {
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
            addMoneyCanvas.enabled = true;
        }
    }
}
