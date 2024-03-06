using Input;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.Flowers
{
    [RequireComponent(typeof(Button))]
    public class FlowersCancelButton : MonoBehaviour
    {
        [Inject] private readonly FlowersCanvasLiaison flowersCanvasLiaison;
        [Inject] private readonly PlayerInputActions playerInputActions;
        
        [HideInInspector, SerializeField] private Button cancelButton;

        private void OnValidate()
        {
            cancelButton = GetComponent<Button>();
        }

        private void OnEnable()
        {
            cancelButton.onClick.AddListener(OnCancelButtonClick);
        }

        private void OnCancelButtonClick()
        {
            playerInputActions.DisableCanvasControlMode();
            flowersCanvasLiaison.FlowersCanvas.enabled = false;
        }
    }
}