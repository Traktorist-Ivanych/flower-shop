using System.Collections.Generic;
using UnityEngine;

namespace Saves
{
    public class PlayerPrefsKeysSetter : MonoBehaviour
    {
        [HideInInspector, SerializeField] private int approachCount;
        
        [ContextMenu("Set Player Prefs Key")]
        private void SetPlayerPrefsKey()
        {
            approachCount++;
            
            GameObject[] allGameObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            List<ISavableObject> savableObjects = new();
            foreach (GameObject oneGameObject in allGameObjects)
            {
                if (oneGameObject.TryGetComponent(out ISavableObject savableObject))
                {
                    savableObjects.Add(savableObject);
                }
            }

            int counter = 0;

            foreach (ISavableObject savableObject in savableObjects)
            {
                savableObject.PlayerPrefsKey = savableObject+ " " + approachCount + " " + counter;
                counter++;
            }
        }
    }
}