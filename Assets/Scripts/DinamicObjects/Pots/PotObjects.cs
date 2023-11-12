using UnityEngine;

public class PotObjects : MonoBehaviour
{
    [SerializeField] private MeshRenderer flowerRenderer;
    [SerializeField] private MeshRenderer soilRenderer;
    [SerializeField] private MeshRenderer waterIndicator;
    [SerializeField] private MeshRenderer weedRenderer;
    [SerializeField] private Weed weed;

    private MeshFilter flowerMeshFilter;
    private MeshFilter soilMeshFilter;
    private MeshFilter weedMeshFilter;
    private Transform waterIndicatorTransform;

    public MeshFilter FlowerMeshFilter
    {
        get => flowerMeshFilter;
    }

    public MeshFilter SoilMeshFilter
    {
        get => soilMeshFilter;
    }

    public MeshFilter WeedMeshFilter
    {
        get => weedMeshFilter;
    }

    private void Start()
    {
        flowerMeshFilter = flowerRenderer.GetComponent<MeshFilter>();
        soilMeshFilter = soilRenderer.GetComponent<MeshFilter>();
        weedMeshFilter = weedRenderer.GetComponent<MeshFilter>();
        waterIndicatorTransform = waterIndicator.transform;
    }

    private void Update()
    {
        waterIndicatorTransform.rotation = Quaternion.Euler(-90,180,0);
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

    public void ShowWaterIndivator()
    {
        waterIndicator.enabled = true;
    }

    public void HideWaterIndicator()
    {
        waterIndicator.enabled = false;
    }

    public void SetFlowerLvlMesh(Flower currentFlower, int currentGrowingLvl)
    {
        flowerMeshFilter.mesh = currentFlower.GetFlowerLvlMesh(currentGrowingLvl);
    }

    public void SetWeedLvlMesh(int currentWeedGrowingLvl)
    {
        weedMeshFilter.mesh = weed.GetWeedLvlMesh(currentWeedGrowingLvl);
    }
}
