using Input;
using UnityEngine;
using Zenject;

namespace FlowerShop.Back
{
    public class QuitCanvasLiaison : MonoBehaviour
    {
        [Inject] private readonly PlayerInputActions playerInputActions;

        [SerializeField] private Canvas quitCanvas;

        public void ShowCanvas()
        {
            playerInputActions.EnableCanvasControlMode();
            quitCanvas.enabled = true;
        }

        public void HideCanvas()
        {
            playerInputActions.DisableCanvasControlMode();
            quitCanvas.enabled = false;
        }
    }
}
