using UnityEngine;

public class GameConfiguration : MonoBehaviour
{
    [Header("Player Moving Settings")]
    public readonly float PlayerNavAgentSpeed = 8;
    public readonly float PlayerNavAgentAngularSpeed = 750;
    public readonly float PlayerNavAgentAcceleration = 75;
    public readonly float PlayerMovingRotation = 8;

    public readonly float PlayerNavAgentCoffeSpeed = 10;
    public readonly float PlayerNavAgentCoffeAngularSpeed = 1000;
    public readonly float PlayerNavAgentCoffeAcceleration = 100;
    public readonly float PlayerMovingCoffeRotation = 10;

    [Header("Settings")]
    public readonly float PotMovingActionDelay = 0.6f;
    public readonly float ObjectsRotateDegreesPerSecond = 360;

    [Header("SoilPreparationTable")]
    public readonly float SoilPreparationTime = 1;
    public readonly float SoilPreparationLvlTimeDelta = 0.2f;

    [Header("WateringCan")]
    public readonly int WateringsNumber = 10;
    public readonly int WateringsNumberLvlDelta = 2;
    public readonly float ReplenishWateringCanTime = 5;

    [Header("FlowerGrowingTable")]
    public readonly float UpGrowingLvlTime = 5;
    public readonly float UpGrowingLvlTableLvlTimeDelta = 1;

    [Header("CrossingTable")]
    public readonly float CrossingFlowerTime = 6;
    public readonly float CrossingFlowerLvlTimeDelta = 2;

    [Header("Buyers")]
    public readonly float MinBuyerSpawnTime = 90;
    public readonly float MinBuyerSpawnTimeDelta = 60;
    public readonly float MaxBuyerSpawnTime = 120;
    public readonly float MaxBuyerSpawnTimeDelta = 75;

    [Header("Weeds")]
    public readonly float MinWeedPlantTime = 300;
    public readonly float MaxWeedPlantTime = 400;
    public readonly float WeedingTime = 9;
    public readonly float WeedingTimeLvlDelta = 3;

    [Header("Improvements")]
    public readonly float TableImprovementTime = 2;

    [Header("Repairs")]
    public readonly float TableRepairTime = 8;
    public readonly int SoilPreparationMinQuantity = 2;
    public readonly int SoilPreparationMaxQuantity = 4;
    public readonly int WateringTableMinQuantity = 2;
    public readonly int WateringTableMaxQuantity = 4;
    public readonly int FlowerGrowingTableMinQuantity = 3;
    public readonly int FlowerGrowingTableMaxQuantity = 5;
    public readonly int CrossingTableMinQuantity = 1;
    public readonly int CrossingTableMaxQuantity = 3;
}
