using UnityEngine;

public static class PlayerAnimatorParameters
{
    [Header("Bool")]
    public const string IsPlayerWalkBool = "IsPlayerWalk";

    [Header("Trigger")]
    public const string StatMakingCoffeTrigger = "StartMakingCoffe";
    public const string DrinkCoffeTrigger = "DrinkCoffe";
    public const string TakeLittleObjectTrigger = "TakeLittleObject";
    public const string GiveLittleObjectTrigger = "GiveLittleObject";
    public const string TakeBigObjectTrigger = "Take";
    public const string GiveBigObjectTrigger = "Give";
    public const string StartBuildsTrigger = "StartBuilds";
    public const string FinishBuildsTrigger = "FinishBuilds";
    public const string StartWeedingTrigger = "StartWeeding";
    public const string FinishWeedingTrigger = "FinishWeeding";
    public const string PourTrigger = "Pour";
    public const string ThrowTrigger = "Throw";
    public const string StartCrossingTrigger = "StartCrossing";
}
