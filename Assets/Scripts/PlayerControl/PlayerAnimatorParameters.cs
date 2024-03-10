using UnityEngine;

namespace PlayerControl
{
    public static class PlayerAnimatorParameters
    {
        [Header("Bool")]
        public const string IsPlayerWalkBool = "IsPlayerWalk";

        [Header("Trigger")]
        public const string StatMakingCoffeeTrigger = "StartMakingCoffe";
        public const string DrinkCoffeeTrigger = "DrinkCoffe";
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
        public const string LoadToHold = "LoadToHold";
        public const string LoadToHoldLittleObject = "LoadToHoldLittleObject";
        public const string WalkSpeed = "WalkSpeed";
        public const string StartUsingComputer = "StartUsingComputer";
        public const string StopUsingComputer = "StopUsingComputer";
    }
}
