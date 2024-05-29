using FlowerShop.ComputerPages;
using UnityEngine;
using Zenject;

namespace FlowerShop.Sounds
{
    [RequireComponent(typeof(UIButton))]
    public class OpenSoundsSettingsCanvasButton : MonoBehaviour
    {
        [Inject] private readonly SoundsSettingsCanvasLiaison soundsSettingsCanvasLiaison;

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
            soundsSettingsCanvasLiaison.ShowCanvas();
        }
    }
}
