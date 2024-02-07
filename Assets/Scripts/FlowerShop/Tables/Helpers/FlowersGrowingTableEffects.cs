using UnityEngine;
using Zenject;

namespace FlowerShop.Tables.Helpers
{
    [RequireComponent(typeof(TableObjectsRotation))]
    public class FlowersGrowingTableEffects : MonoBehaviour
    {
        [Inject] private readonly TablesSettings tablesSettings;
        
        [SerializeField] private MeshRenderer growingLightMeshRenderer;
        [SerializeField] private MeshRenderer growingTableFanMeshRenderer;
        [SerializeField] private Mesh[] growingLightLvlMeshes = new Mesh[2];
        [SerializeField] private ParticleSystem[] windPS = new ParticleSystem[2];
        
        [HideInInspector, SerializeField] private TableObjectsRotation tableObjectsRotation;
        [HideInInspector, SerializeField] private MeshFilter growingLightMeshFilter;

        private int currentTableLvl;

        private void OnValidate()
        {
            tableObjectsRotation = GetComponent<TableObjectsRotation>();
            growingLightMeshFilter = growingLightMeshRenderer.GetComponent<MeshFilter>();
        }

        public void SetFlowersGrowingTableLvlForEffects(int flowersGrowingTableLvl)
        {
            currentTableLvl = flowersGrowingTableLvl;
            if (currentTableLvl >= tablesSettings.FansEnableLvl)
            {
                growingTableFanMeshRenderer.enabled = true;
                growingLightMeshFilter.mesh = growingLightLvlMeshes[currentTableLvl - 1];
            }
        }

        public void EnableEffects()
        {
            growingLightMeshRenderer.enabled = true;
            if (currentTableLvl >= tablesSettings.FansEnableLvl)
            {
                foreach (ParticleSystem wind in windPS)
                {
                    wind.Play();
                }
            }
        }

        public void StartFansRotation()
        {
            tableObjectsRotation.StartObjectsRotation();
        }

        public void DisableEffects()
        {
            growingLightMeshRenderer.enabled = false;
            if (currentTableLvl >= tablesSettings.FansEnableLvl)
            {
                foreach (ParticleSystem wind in windPS)
                {
                    wind.Stop();
                }
            }
        }

        public void StopFansRotation()
        {
            tableObjectsRotation.PauseObjectsRotation();
        }
    }
}