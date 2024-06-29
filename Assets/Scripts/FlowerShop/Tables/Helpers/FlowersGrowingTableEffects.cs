using FlowerShop.Sounds;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables.Helpers
{
    [RequireComponent(typeof(TableObjectsRotation))]
    public class FlowersGrowingTableEffects : MonoBehaviour
    {
        [Inject] private readonly SoundsHandler soundsHandler;
        [Inject] private readonly TablesSettings tablesSettings;
        
        [SerializeField] private MeshRenderer growingLightMeshRenderer;
        [SerializeField] private MeshRenderer growingTableFanMeshRenderer;
        [SerializeField] private Mesh[] growingLightLvlMeshes = new Mesh[2];
        [SerializeField] private ParticleSystem[] windPS = new ParticleSystem[2];
        
        [HideInInspector, SerializeField] private TableObjectsRotation tableObjectsRotation;
        [HideInInspector, SerializeField] private MeshFilter growingLightMeshFilter;

        private int currentTableLvl;
        private bool isEffectsEnabled;

        private void OnValidate()
        {
            tableObjectsRotation = GetComponent<TableObjectsRotation>();
            growingLightMeshFilter = growingLightMeshRenderer.GetComponent<MeshFilter>();
        }

        public void SetFlowersGrowingTableLvlForEffects(int flowersGrowingTableLvl)
        {
            currentTableLvl = flowersGrowingTableLvl;
            growingLightMeshFilter.mesh = growingLightLvlMeshes[currentTableLvl - 1];
            
            if (currentTableLvl >= tablesSettings.FansEnableLvl)
            {
                growingTableFanMeshRenderer.enabled = true;
                if (currentTableLvl == tablesSettings.FansEnableLvl && isEffectsEnabled)
                {
                    EnableUpgradableEffects();
                }
            }
        }

        public void EnableEffects()
        {
            if (!isEffectsEnabled)
            {
                isEffectsEnabled = true;
                growingLightMeshRenderer.enabled = true;
                if (currentTableLvl >= tablesSettings.FansEnableLvl)
                {
                    EnableUpgradableEffects();
                }
            }
        }

        public void DisableEffects()
        {
            if (isEffectsEnabled)
            {
                isEffectsEnabled = false;
                growingLightMeshRenderer.enabled = false;
                if (currentTableLvl >= tablesSettings.FansEnableLvl)
                {
                    soundsHandler.StopPlayingGrowingTableFansAudio();
                    tableObjectsRotation.PauseObjectsRotation();
                    foreach (ParticleSystem wind in windPS)
                    {
                        wind.Stop();
                    }
                }
            }
        }

        private void EnableUpgradableEffects()
        {
            soundsHandler.StartPlayingGrowingTableFansAudio();
            tableObjectsRotation.StartObjectsRotation();
            foreach (ParticleSystem wind in windPS)
            {
                wind.Play();
            }
        }
    }
}