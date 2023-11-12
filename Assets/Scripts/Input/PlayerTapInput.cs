using UnityEngine;

public class PlayerTapInput : MonoBehaviour
{
    [SerializeField] private LayerMask inputLayerMask;
    [SerializeField] private float maxRayDistance;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void PlayerTap(Vector2 inputPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(inputPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, inputLayerMask))
        {
            if (hit.collider.gameObject.TryGetComponent(out IClickableAbility clickableAbility))
            {
                clickableAbility.ExecuteClickableAbility();
            }
        }
    }
}
