using UnityEngine;

// rename to Pickable...Handler
public class CurrentPlayerDinamicObject : MonoBehaviour
{
    private IDynamicObject dynamicObject;

    public IDynamicObject GetCurrentPlayerDinamicObject()
    {
        return dynamicObject;
    }

    public bool IsPlayerDynamicObjectNull()
    {
        return dynamicObject == null;
    }

    public void SetPlayerDinamicObject(IDynamicObject transmittedDynamicObject)
    {
        dynamicObject = transmittedDynamicObject;
    }

    public void ClearPlayerDinamicObject()
    {
        dynamicObject = null;
    }
}
