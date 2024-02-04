using UnityEngine;

namespace PlayerControl.SaveData
{
    [System.Serializable]
    public struct PlayerPickableObjectHandlerForSave
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public string PickableObjectPlayerPrefsKey { get; private set; }

        public PlayerPickableObjectHandlerForSave(string pickableObjectPlayerPrefsKey)
        {
            IsValuesSaved = true;
            PickableObjectPlayerPrefsKey = pickableObjectPlayerPrefsKey;
        }
    }
}