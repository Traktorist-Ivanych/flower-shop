using FlowerShop.ComputerPages;
using UnityEngine;
using Zenject;

namespace FlowerShop.Back
{
    [RequireComponent(typeof(UIButton))]
    public class CancelQuitButton : MonoBehaviour
    {
        [Inject] private readonly QuitCanvasLiaison quitCanvasLiaison;

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
            quitCanvasLiaison.HideCanvas();
        }
    }
}
