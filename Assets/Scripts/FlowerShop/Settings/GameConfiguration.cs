using UnityEngine;

public class GameConfiguration : MonoBehaviour
{
    [Header("Weeds")]
    [SerializeField] private AnimationCurve weedPlanting;
    [SerializeField] private float weedPlantingSuccessBorder;
    public readonly float MinWeedPlantTime = 120;
    public readonly float MaxWeedPlantTime = 240;
    public readonly float WeedingTime = 9;
    public readonly float WeedingTimeLvlDelta = 3;

    [Header("Settings")]
    public readonly float PotMovingActionDelay = 0.6f;
    public readonly float ObjectsRotateDegreesPerSecond = 360;

    [Header("SoilPreparationTable")]
    public readonly float SoilPreparationTime = 1;
    public readonly float SoilPreparationLvlTimeDelta = 1;

    [Header("WateringCan")]
    public readonly int WateringsNumber = 9;
    public readonly int WateringsNumberLvlDelta = 3;
    public readonly float ReplenishWateringCanTime = 5;

    [Header("FlowersCrossingTable")]
    public readonly float CrossingFlowerTime = 3;
    public readonly float CrossingFlowerLvlTimeDelta = 10;

    [Header("Improvements")]
    public readonly float TableImprovementTime = 10;

    [Header("Repairs")]
    public readonly float TableRepairTime = 8;
    public readonly int SoilPreparationMinQuantity = 8;
    public readonly int SoilPreparationMaxQuantity = 16;
    public readonly int WateringTableMinQuantity = 9;
    public readonly int WateringTableMaxQuantity = 16;
    public readonly int FlowerGrowingTableMinQuantity = 8;
    public readonly int FlowerGrowingTableMaxQuantity = 16;
    public readonly int CrossingTableMinQuantity = 5;
    public readonly int CrossingTableMaxQuantity = 10;

    public bool IsWeedPlanting()
    {
        return weedPlanting.Evaluate(Random.Range(0, 1f)) >= weedPlantingSuccessBorder;
    }
}
