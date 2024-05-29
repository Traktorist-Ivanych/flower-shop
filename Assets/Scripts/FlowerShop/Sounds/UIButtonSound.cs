using FlowerShop.ComputerPages;
using UnityEngine;
using Zenject;

namespace FlowerShop.Sounds
{
    [RequireComponent(typeof(UIButton))]
    public class UIButtonSound : MonoBehaviour
    {
        [Inject] private readonly SoundsHandler soundHandler;

        [HideInInspector, SerializeField] private UIButton button;

        private void OnValidate()
        {
            button = GetComponent<UIButton>();
        }

        private void OnEnable()
        {
            button.OnClickEvent += OnButtonClick;
        }

        private void OnDisable()
        {
            button.OnClickEvent -= OnButtonClick;
        }

        private void OnButtonClick()
        {
            soundHandler.PlayUIButtonAudio();
        }
    }
}
