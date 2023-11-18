using UnityEngine;
using Zenject;

[RequireComponent(typeof(DinamicObjectMoving))]
public class WateringCan : MonoBehaviour, IDinamicObject, IGrowingRoom
{
    [Inject] private readonly CurrentPlayerDinamicObject playerDinamicObject;
    [Inject] private readonly GameConfiguration gameConfiguration;
    [Inject] private readonly PlayerComponents playerComponents;

    [SerializeField] private IGrowingRoom.GroweringRoom groweringRoom;
    [SerializeField] private MeshFilter wateringCanMeshFilter;
    [SerializeField] private MeshRenderer wateringCanIndicatorMeshRenderer;
    [SerializeField] private Transform wateringCanIndicatorTransform;
    [SerializeField] private Mesh[] wateringCanLvlMeshes = new Mesh[2];

    private DinamicObjectMoving wateringCanMoving;
    private int currentWateringsNumber;
    private int maxWateringsNumber;
    private int wateringCanLvl;

    public int CurrentWateringsNumber
    {
        get { return currentWateringsNumber; }
        set { currentWateringsNumber = value; }
    }

    public IGrowingRoom.GroweringRoom GetGroweringRoom() { return groweringRoom; }

    private void Start()
    {
        maxWateringsNumber = gameConfiguration.WateringsNumber;
        currentWateringsNumber = maxWateringsNumber;
        wateringCanMoving = GetComponent<DinamicObjectMoving>();
    }

    public void TakeWateringCan()
    {
        playerDinamicObject.SetPlayerDinamicObject(this);
        wateringCanMoving.PutBigDinamicObjectInPlayerHands();
        wateringCanIndicatorMeshRenderer.enabled = true;
        UpdateWateringCanIndicator();
    }

    public void GiveWateringCan(Transform targetTransfom) 
    {
        wateringCanMoving.PutBigDinamicObjectOnTable(targetTransfom);
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
