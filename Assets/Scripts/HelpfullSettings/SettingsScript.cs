using UnityEngine;
#if UNITY_EDITOR || DEVELOPMENT_BUILD 
public class SettingsScript : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 30;
    }
}

#endif
