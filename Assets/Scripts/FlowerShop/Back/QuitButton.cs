using FlowerShop.ComputerPages;
using UnityEngine;

namespace FlowerShop.Back
{
    [RequireComponent(typeof(UIButton))]
    public class QuitButton : MonoBehaviour
    {
        [HideInInspector, SerializeField] private UIButton uIButton;

        private void OnValidate()
        {
            uIButton = GetComponent<UIButton>();
        }

        private void OnEnable()
        {
            uIButton.OnClickEvent += OnButtonClick;
        }

        private void OnDisable()
        {
            uIButton.OnClickEvent -= OnButtonClick;
        }

        private void OnButtonClick()
        {
            Application.Quit();
        }
    }
}
