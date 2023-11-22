using UnityEngine;

public class PlayerPickableObjectHandler : MonoBehaviour
{
    private IPickableObject pickableObject;

    public IPickableObject GetCurrentPlayerPickableObject()
    {
        return pickableObject;
    }

    public bool IsPlayerPickableObjectNull()
    {
        return pickableObject == null;
    }

    public void SetPlayerPickableObject(IPickableObject transmittedPickableObject)
    {
        pickableObject = transmittedPickableObject;
    }

    public void ClearPlayerPickableObject()
    {
        pickableObject = null;
    }
}
