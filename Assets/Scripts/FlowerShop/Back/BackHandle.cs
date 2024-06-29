using Input;
using UnityEngine;
using Zenject;

namespace FlowerShop.Back
{
    public class BackHandle : MonoBehaviour
    {
        [Inject] private readonly PlayerInputActions playerInputActions;
        [Inject] private readonly QuitCanvasLiaison quitCanvasLiaison;

        private void OnEnable()
        {
            playerInputActions.BackCallEvent += OnBackInputCall;
        }

        private void OnDisable()
        {
            playerInputActions.BackCallEvent -= OnBackInputCall;
        }

        private void OnBackInputCall()
        {
            quitCanvasLiaison.ShowCanvas();
        }
    }
}
