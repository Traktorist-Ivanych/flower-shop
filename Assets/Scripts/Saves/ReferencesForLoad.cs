using System.Collections.Generic;
using UnityEngine;

namespace Saves
{
    public class ReferencesForLoad : MonoBehaviour
    {
        [SerializeField] private List<MonoBehaviour> savableObjectsReferences;

        private void OnValidate()
        {
            foreach (MonoBehaviour savableObjectsReference in savableObjectsReferences)
            {
                if (savableObjectsReference is not IUniqueKey)
                {
                    savableObjectsReferences.Remove(savableObjectsReference);
                    Debug.LogError("Only IPlayerPrefsKey can be added to List SavableObjectsReferences");
                }
            }
        }

        public IUniqueKey GetIUniqueKeyReference(string key)
        {
            foreach (MonoBehaviour savableObjectsReference in savableObjectsReferences)
            {
                if ((savableObjectsReference as IUniqueKey)?.UniqueKey == key)
                {
                    return savableObjectsReference as IUniqueKey;
                }
            }
            return null;
        }

        public T GetReference<T>(string key) where T : MonoBehaviour
        {
            foreach (MonoBehaviour savableObjectsReference in savableObjectsReferences)
            {
                if ((savableObjectsReference as ISavableObject)?.UniqueKey == key)
                {
                    return savableObjectsReference as T;
                }
            }
            return null;
        }
    }
}