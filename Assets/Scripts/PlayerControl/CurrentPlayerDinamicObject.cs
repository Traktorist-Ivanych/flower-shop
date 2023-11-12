using UnityEngine;

public class CurrentPlayerDinamicObject : MonoBehaviour
{
    private IDinamicObject dinamicObject;

    public IDinamicObject GetCurrentPlayerDinamicObject()
    {
        return dinamicObject;
    }

    public bool IsPlayerDinamicObjectNull()
    {
        if (dinamicObject == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetPlayerDinamicObject(IDinamicObject transmittedDinamicObject)
    {
        dinamicObject = transmittedDinamicObject;
    }

    public void ClearPlayerDinamicObject()
    {
        dinamicObject = null;
    }
}
