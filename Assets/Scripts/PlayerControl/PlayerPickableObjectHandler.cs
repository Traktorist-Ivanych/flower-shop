using PlayerControl.SaveData;
using Saves;
using UnityEngine;
using Zenject;

namespace PlayerControl
{
    public class PlayerPickableObjectHandler : MonoBehaviour, ISavableObject
    {
        [Inject] private readonly ReferencesForLoad referencesForLoad;

        private IPickableObject currentPickableObject;
        
        [field: SerializeField] public string UniqueKey { get; set; }
        
        public bool IsPickableObjectNull => CurrentPickableObject == null;

        public IPickableObject CurrentPickableObject
        {
            get => currentPickableObject;
            set
            {
                currentPickableObject = value;
                Save();
            }
        }

        private void Awake()
        {
            Load();
        }

        public void ResetPickableObject()
        {
            SavesHandler.DeletePlayerPrefsKey(UniqueKey);
            CurrentPickableObject = null;
        }

        public void Save()
        {
            PlayerPickableObjectHandlerForSave playerPickableObjectHandlerForSave =
                new((currentPickableObject as IUniqueKey)?.UniqueKey);
            
            SavesHandler.Save(UniqueKey, playerPickableObjectHandlerForSave);
        }

        public void Load()
        {
            PlayerPickableObjectHandlerForSave playerPickableObjectHandlerForLoad =
                SavesHandler.Load<PlayerPickableObjectHandlerForSave>(UniqueKey);

            if (playerPickableObjectHandlerForLoad.IsValuesSaved)
            {
                IUniqueKey loadedIUniqueKey = 
                    referencesForLoad.GetIUniqueKeyReference(playerPickableObjectHandlerForLoad.PickableObjectPlayerPrefsKey);

                if (loadedIUniqueKey is IPickableObject loadedPickableObject)
                {
                    CurrentPickableObject = loadedPickableObject;
                    CurrentPickableObject.LoadInPlayerHands();
                }
            }
        }
    }
}
