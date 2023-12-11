using FlowerShop.Flowers;
using FlowerShop.Settings;
using FlowerShop.Weeds;
using UnityEngine;
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

        [HideInInspector, SerializeField] private Transform waterIndicatorTransform;

        [field: HideInInspector, SerializeField] public MeshFilter FlowerMeshFilter { get; private set; }

        [field: HideInInspector, SerializeField] public MeshFilter SoilMeshFilter { get; private set; }

        [field: HideInInspector, SerializeField] public MeshFilter WeedMeshFilter { get; private set; }

        private void OnValidate()
        {
            FlowerMeshFilter = flowerRenderer.GetComponent<MeshFilter>();
            SoilMeshFilter = soilRenderer.GetComponent<MeshFilter>();
            WeedMeshFilter = weedRenderer.GetComponent<MeshFilter>();
            waterIndicatorTransform = waterIndicator.transform;
        }

        private void Update()
        {
            waterIndicatorTransform.rotation = Quaternion.Euler(actionsWithTransformSettings.ConstantIndicatorRotation);
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
    }
}
