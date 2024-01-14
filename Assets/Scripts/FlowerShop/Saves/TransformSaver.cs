using FlowerShop.Saves.SaveData;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Saves
{
    public class TransformSaver : MonoBehaviour, ISavableObject
    {
        [Inject] private readonly CyclicalSaver cyclicalSaver;
        
        [field: SerializeField] public string UniqueKey { get; set; }
        
        private void Awake()
        {
            Load();
        }

        private void OnEnable()
        {
            cyclicalSaver.CyclicalSaverEvent += Save;
        }

        private void OnDisable()
        {
            cyclicalSaver.CyclicalSaverEvent -= Save;
        }

        public void Save()
        {
            TransformForSaving transformSaving = new TransformForSaving(transform);
            SavesHandler.Save(UniqueKey, transformSaving);
        }

        public void Load()
        {
            TransformForSaving loadedTransform = SavesHandler.Load<TransformForSaving>(UniqueKey);

            if (loadedTransform.IsValuesSaved)
            {
                transform.SetPositionAndRotation(loadedTransform.Position, loadedTransform.Rotation);
            }
        }
    }
}