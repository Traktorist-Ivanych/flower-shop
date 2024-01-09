using UnityEngine;

namespace Saves
{
    public static class SavesHandler
    {
        public static void Save<T>(string key, T saveData)
        {
            string jsonData = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(key, jsonData);
        }

        public static T Load<T>(string key) where T : new()
        {
            if (PlayerPrefs.HasKey(key))
            {
                string loadedJsonData = PlayerPrefs.GetString(key);
                return JsonUtility.FromJson<T>(loadedJsonData);
            }

            return new T();
        }

        public static void DeletePlayerPrefsKey(string keyForDeleting)
        {
            if (PlayerPrefs.HasKey(keyForDeleting))
            {
                PlayerPrefs.DeleteKey(keyForDeleting);
            }
        }
    }
}
