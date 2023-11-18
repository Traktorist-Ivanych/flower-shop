using UnityEngine;

public class GameConfiguration : MonoBehaviour
{
    [Header("Buyers")]
    [SerializeField] private AnimationCurve buyerBuyingFlower;
    [SerializeField] private float buyerBuyingFlowerSuccessBorder;
    public readonly float MinBuyerSpawnTime = 150;
    public readonly float MinBuyerSpawnTimeDelta = 120;
    public readonly float MaxBuyerSpawnTime = 250;
    public readonly float MaxBuyerSpawnTimeDelta = 190;

    [Header("Weeds")]
    [SerializeField] private AnimationCurve weedPlanting;
    [SerializeField] private float weedPlantingSuccessBorder;
    public readonly float MinWeedPlantTime = 120;
    public readonly float MaxWeedPlantTime = 240;
    public readonly float WeedingTime = 9;
    public readonly float WeedingTimeLvlDelta = 3;

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
    public readonly float SoilPreparationLvlTimeDelta = 1f;

    [Header("WateringCan")]
    public readonly int WateringsNumber = 9;
    public readonly int WateringsNumberLvlDelta = 3;
    public readonly float ReplenishWateringCanTime = 5;

    [Header("FlowerGrowingTable")]
    public readonly float UpGrowingLvlTime = 3;
    public readonly float UpGrowingLvlTableLvlTimeDelta = 10f;

    [Header("CrossingTable")]
    public readonly float CrossingFlowerTime = 3;
    public readonly float CrossingFlowerLvlTimeDelta = 10;

    [Header("Flowers For Sale Coef")]
    public readonly float AllFlowersForSale = 10;
    public readonly float UniqueFlowersForSale = 5;

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

    [Header("Coffe")]
    public readonly int CoffePrice = 1250;
    public readonly float CoffeEffectDurationTime = 600;

    public bool IsByerBuyingFlower()
    {
        return buyerBuyingFlower.Evaluate(Random.Range(0, 1f)) >= buyerBuyingFlowerSuccessBorder;
    }

    public bool IsWeedPlanting()
    {
        return weedPlanting.Evaluate(Random.Range(0, 1f)) >= weedPlantingSuccessBorder;
    }
}
