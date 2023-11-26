using UnityEngine;

#if UNITY_EDITOR || DEVELOPMENT_BUILD 
namespace DebugSetting
{
    public class TargetFrameRateSetter : MonoBehaviour
    {
        [SerializeField] private int desiredTargetFrameRate;
        
        private void Start()
        {
            Application.targetFrameRate = desiredTargetFrameRate;
        }
    }
}

#endif
