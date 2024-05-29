using UnityEngine;
using Zenject;

namespace Input
{
    public class PlayerTapInput : MonoBehaviour
    {
        [Inject] private readonly CameraHandler cameraHandler;
        [Inject] private readonly PlayerTapSettings playerTapSettings;

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
                else if (hit.collider.gameObject.TryGetComponent(out IClicableFloor clicableFloor))
                {
                    clicableFloor.ExecuteClickableFloorAbility(hit.point);
                }
            }
        }
    }
}
