using FlowerShop.Flowers;
using FlowerShop.Settings;
using FlowerShop.Weeds;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace FlowerShop.PickableObjects.Helpers
{
    public class PotObjects : MonoBehaviour
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        
        [SerializeField] private MeshRenderer flowerRenderer;
        [SerializeField] private MeshRenderer soilRenderer;
        [SerializeField] private MeshRenderer waterIndicator;
        [SerializeField] private MeshRenderer weedRenderer;
        [SerializeField] private Weed weed;
        [SerializeField] private ParticleSystem weedIndicatorPS;
        [SerializeField] private ParticleSystem seedIndicatorPS;
        [SerializeField] private ParticleSystem rareFlowerPS;
        [SerializeField] private Material seedGrowingMaterial;
        [SerializeField] private Material seedPlantedMaterial;
        [SerializeField] private MeshRenderer mainIndicatorMeshRenderer;
        [SerializeField] private MeshRenderer progressIndicatorMeshRenderer;

        [HideInInspector, SerializeField] private Transform waterIndicatorTransform;
        [HideInInspector, SerializeField] private Renderer seedIndicatorPSRenderer;
        [HideInInspector, SerializeField] private Transform mainIndicatorTransform;
        [HideInInspector, SerializeField] private Transform progressIndicatorTransform;

        public bool IsProgressbarActive { get; private set; }
        [field: HideInInspector, SerializeField] public MeshFilter FlowerMeshFilter { get; private set; }
        [field: HideInInspector, SerializeField] public MeshFilter SoilMeshFilter { get; private set; }
        [field: HideInInspector, SerializeField] public MeshFilter WeedMeshFilter { get; private set; }

        private void OnValidate()
        {
            FlowerMeshFilter = flowerRenderer.GetComponent<MeshFilter>();
            SoilMeshFilter = soilRenderer.GetComponent<MeshFilter>();
            WeedMeshFilter = weedRenderer.GetComponent<MeshFilter>();
            waterIndicatorTransform = waterIndicator.transform;
            seedIndicatorPSRenderer = seedIndicatorPS.GetComponent<Renderer>();
            mainIndicatorTransform = mainIndicatorMeshRenderer.GetComponent<Transform>();
            progressIndicatorTransform = progressIndicatorMeshRenderer.GetComponent<Transform>();
        }

        private void Update()
        {
            waterIndicatorTransform.rotation = Quaternion.Euler(actionsWithTransformSettings.ConstantIndicatorRotation);
        }

        public void ShowProgressbar()
        {
            mainIndicatorTransform.rotation = Quaternion.Euler(actionsWithTransformSettings.ConstantProgressbarRotation);
            IsProgressbarActive = true;
            mainIndicatorMeshRenderer.enabled = true;
            progressIndicatorMeshRenderer.enabled = true;
        }

        public void HideProgressbar()
        {
            IsProgressbarActive = false;
            mainIndicatorMeshRenderer.enabled = false;
            progressIndicatorMeshRenderer.enabled = false;
        }

        public void UpdateProgressbar(float zValue)
        {
            progressIndicatorTransform.localScale = new Vector3(
                progressIndicatorTransform.localScale.x,
                progressIndicatorTransform.localScale.y,
                zValue);
        }

        public void ShowSoil()
        {
            soilRenderer.enabled = true;
        }

        public void ShowFlower()
        {
            flowerRenderer.enabled = true;
        }

        public void ShowWeed()
        {
            weedRenderer.enabled = true;
        }

        public void HideWeed()
        {
            weedRenderer.enabled = false;
        }

        public void HideAllPotObjects()
        {
            soilRenderer.enabled = false;
            flowerRenderer.enabled = false;
            waterIndicator.enabled = false;
            weedRenderer.enabled = false;
        }

        public void PlayRareFlowerEffect()
        {
            rareFlowerPS.Play();
        }

        public void ShowWaterIndicator()
        {
            waterIndicator.enabled = true;
        }

        public void HideWaterIndicator()
        {
            waterIndicator.enabled = false;
        }

        public void SetFlowerLvlMesh(FlowerInfo currentFlowerInfo, int currentGrowingLvl)
        {
            FlowerMeshFilter.mesh = currentFlowerInfo.GetFlowerLvlMesh(currentGrowingLvl);
        }

        public void SetWeedLvlMesh(int currentWeedGrowingLvl)
        {
            WeedMeshFilter.mesh = weed.GetWeedLvlMesh(currentWeedGrowingLvl);
        }

        public void PlayWeedEffects()
        {
            weedIndicatorPS.Play();
        }

        public void PlaySeedGrowingEffects()
        {
            seedIndicatorPSRenderer.material = seedGrowingMaterial;
            seedIndicatorPS.Play();
        }

        public void PlaySeedPlantedEffects()
        {
            seedIndicatorPSRenderer.material = seedPlantedMaterial;
            seedIndicatorPS.Play();
        }
    }
}
