using System.Collections.Generic;
using UnityEngine;

namespace Saves
{
    public class UniqueKeysTest : MonoBehaviour
    {
        [ContextMenu("Check All Unique Keys")]
        private void CheckAllUniqueKeys()
        {
            MonoBehaviour[] allMonoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            
            Debug.Log("All MonoBehaviours = " + allMonoBehaviours.Length);

            List<ISavableObject> allSavableObjects = new();
            
            for (int i = 0; i < allMonoBehaviours.Length; i++)
            {
                if (allMonoBehaviours[i] is ISavableObject)
                {
                    allSavableObjects.Add(allMonoBehaviours[i] as ISavableObject);
                }
            }
            
            Debug.Log("All ISavableObjects = " + allSavableObjects.Count);

            bool isTextComplite = true;
            
            for (int i = 0; i < allSavableObjects.Count; i++)
            {
                for (int b = 0; b < allSavableObjects.Count; b++)
                {
                    if (allSavableObjects[i].Equals(allSavableObjects[b]))
                    {
                        continue;
                    }
                    if (allSavableObjects[i].UniqueKey == allSavableObjects[b].UniqueKey)
                    {
                        isTextComplite = false;
                        Debug.LogError("UniqueKey is not Unique! " + allSavableObjects[i] + " and " + allSavableObjects[b]);
                    }
                }
            }

            if (isTextComplite)
            {
                Debug.Log("Test completed!");
            }
            else
            {
                Debug.LogWarning("Test not completed!"); 
            }
        }
    }
}