using System.Collections.Generic;
using UnityEngine;

namespace Saves
{
    public class ReferencesForLoad : MonoBehaviour
    {
        [SerializeField] private List<Object> uniqueKeyObjectsReferences;

        private void OnValidate()
        {
            foreach (Object uniqueKeyObjectsReference in uniqueKeyObjectsReferences)
            {
                if (uniqueKeyObjectsReference is not IUniqueKey)
                {
                    uniqueKeyObjectsReferences.Remove(uniqueKeyObjectsReference);
                    Debug.LogError("Only IUniqueKey can be added to List SavableObjectsReferences");
                }
            }
        }

        public IUniqueKey GetIUniqueKeyReference(string key)
        {
            foreach (Object uniqueKeyObjectsReference in uniqueKeyObjectsReferences)
            {
                if ((uniqueKeyObjectsReference as IUniqueKey)?.UniqueKey == key)
                {
                    return uniqueKeyObjectsReference as IUniqueKey;
                }
            }
            return null;
        }

        public T GetReference<T>(string key) where T : Object
        {
            foreach (Object uniqueKeyObjectsReference in uniqueKeyObjectsReferences)
            {
                if ((uniqueKeyObjectsReference as IUniqueKey)?.UniqueKey == key)
                {
                    return uniqueKeyObjectsReference as T;
                }
            }
            return null;
        }
    }
}