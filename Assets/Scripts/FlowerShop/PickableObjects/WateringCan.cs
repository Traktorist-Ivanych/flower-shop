using FlowerShop.Flowers;
using PlayerControl;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(OldPickableObjectMoving))]
public class WateringCan : MonoBehaviour, IPickableObject
{
    [Inject] private readonly PlayerPickableObjectHandler playerPickableObjectHandler;
    [Inject] private readonly GameConfiguration gameConfiguration;
    [Inject] private readonly PlayerComponents playerComponents;

    [field: SerializeField] public GrowingRoom GrowingRoom { get; private set; }
    [SerializeField] private MeshFilter wateringCanMeshFilter;
    [SerializeField] private MeshRenderer wateringCanIndicatorMeshRenderer;
    [SerializeField] private Transform wateringCanIndicatorTransform;
    [SerializeField] private Mesh[] wateringCanLvlMeshes = new Mesh[2];

    private OldPickableObjectMoving wateringCanMoving;
    private int currentWateringsNumber;
    private int maxWateringsNumber;
    private int wateringCanLvl;

    // can be just property
    public int CurrentWateringsNumber
    {
        get { return currentWateringsNumber; }
        set { currentWateringsNumber = value; }
    }

    private void Start()
    {
        maxWateringsNumber = gameConfiguration.WateringsNumber;
        currentWateringsNumber = maxWateringsNumber;
        wateringCanMoving = GetComponent<OldPickableObjectMoving>();
    }

    public void TakeInPlayerHands()
    {
        playerPickableObjectHandler.CurrentPickableObject = this;
        wateringCanMoving.PutBigPickableObjectInPlayerHands();
        wateringCanIndicatorMeshRenderer.enabled = true;
        UpdateWateringCanIndicator();
    }

    public void PutOnTable(Transform targetTransform) 
    {
        wateringCanMoving.PutBigPickableObjectOnTable(targetTransform);
        wateringCanIndicatorMeshRenderer.enabled = false;
    }

    public void PourPotWithWateringCan()
    {
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.PourTrigger);
        currentWateringsNumber--;
        UpdateWateringCanIndicator();
    }

    public bool IsWateringCanNeedForReplenish()
    {
        return currentWateringsNumber < maxWateringsNumber;
    }

    public float ReplenishWateringCanTime()
    {
        return gameConfiguration.ReplenishWateringCanTime * (maxWateringsNumber - currentWateringsNumber) / maxWateringsNumber;
    }

    public void ReplenishWateringCan()
    {
        currentWateringsNumber = maxWateringsNumber;
    }

    public void ImproveWateringCan()
    {
        wateringCanLvl++;
        maxWateringsNumber = gameConfiguration.WateringsNumber + gameConfiguration.WateringsNumberLvlDelta * wateringCanLvl;
        currentWateringsNumber = maxWateringsNumber;
        wateringCanMeshFilter.mesh = wateringCanLvlMeshes[wateringCanLvl - 1];
    }

    private void UpdateWateringCanIndicator()
    {
        float indicatorScaleZ = 0.9f * currentWateringsNumber / maxWateringsNumber + 0.1f;
        wateringCanIndicatorTransform.localScale = new Vector3(1, 1, indicatorScaleZ);
    }
}
