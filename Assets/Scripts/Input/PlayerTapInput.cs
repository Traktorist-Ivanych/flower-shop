using UnityEngine;
using Zenject;

namespace Input
{
    public class PlayerTapInput : MonoBehaviour
    {
        [Inject] private CameraHandler cameraHandler;
        [Inject] private PlayerTapSettings playerTapSettings;

        public void PlayerTap(Vector2 inputPosition)
        {
            Ray ray = cameraHandler.MainCamera.ScreenPointToRay(inputPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 
                    maxDistance: playerTapSettings.MaxInteractionRayDistance,
                    layerMask: playerTapSettings.InteractionLayerMask))
            {
                if (hit.collider.gameObject.TryGetComponent(out IClickableAbility clickableAbility))
                {
                    clickableAbility.ExecuteClickableAbility();
                }
            }
        }
    }
}
