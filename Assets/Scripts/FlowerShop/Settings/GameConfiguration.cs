using UnityEngine;

public class GameConfiguration : MonoBehaviour
{
    [Header("Settings")]
    public readonly float PotMovingActionDelay = 0.6f;
    public readonly float ObjectsRotateDegreesPerSecond = 360;

    [Header("SoilPreparationTable")]
    public readonly float SoilPreparationTime = 1;
    public readonly float SoilPreparationLvlTimeDelta = 1;
}
