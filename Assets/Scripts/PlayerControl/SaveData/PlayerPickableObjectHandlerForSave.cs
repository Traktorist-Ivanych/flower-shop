using UnityEngine;

namespace PlayerControl.SaveData
{
    [System.Serializable]
    public class PlayerPickableObjectHandlerForSave
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public string PickableObjectPlayerPrefsKey { get; private set; }
        
        public PlayerPickableObjectHandlerForSave() {}

        public PlayerPickableObjectHandlerForSave(string pickableObjectPlayerPrefsKey)
        {
            IsValuesSaved = true;
            PickableObjectPlayerPrefsKey = pickableObjectPlayerPrefsKey;
        }
    }
}