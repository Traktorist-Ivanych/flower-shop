using UnityEngine;

namespace Saves
{
    [CreateAssetMenu(
        fileName = "SavesSettings", 
        menuName = "Settings/Saves Settings", 
        order = 12)]
    public class SavesSettings : ScriptableObject
    {
        [field: SerializeField] public float SaveTimer { get; private set; }

        [ContextMenu("Delete All Player Prefs Keys")]
        public void DeleteAllPlayerPrefsKeys()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}