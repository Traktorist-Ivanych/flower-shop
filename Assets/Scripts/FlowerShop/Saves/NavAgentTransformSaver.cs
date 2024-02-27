using FlowerShop.Saves.SaveData;
using Saves;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace FlowerShop.Saves
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavAgentTransformSaver : MonoBehaviour
    {
        [Inject] private readonly CyclicalSaver cyclicalSaver;

        [HideInInspector, SerializeField] private NavMeshAgent navMeshAgent;
        
        [field: SerializeField] public string UniqueKey { get; set; }

        private void OnValidate()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

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
                transform.rotation = loadedTransform.Rotation;
                navMeshAgent.Warp(loadedTransform.Position);
            }
        }
    }
}