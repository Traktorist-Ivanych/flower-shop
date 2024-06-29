using UnityEngine;
using UnityEngine.UI;

namespace FlowerShop.ComputerPages
{
    [RequireComponent(typeof(Button))]
    public class UIButton : MonoBehaviour
    {
        public delegate void OnClick();
        public event OnClick OnClickEvent;
        
        [HideInInspector, SerializeField] private Button flowerButton;
        
        private void OnValidate()
        {
            flowerButton = GetComponent<Button>();
        }
        
        private void OnEnable()
        {
            flowerButton.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            flowerButton.onClick.RemoveListener(OnButtonClick);
        }
        
        private void OnButtonClick()
        {
            OnClickEvent?.Invoke();
        }
    }
}