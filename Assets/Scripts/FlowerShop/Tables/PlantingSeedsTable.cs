using System.Collections;
using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using FlowerShop.Tables.Abstract;
using UnityEngine;
using Zenject;

public class PlantingSeedsTable : Table
{
    [Inject] private readonly GameConfiguration gameConfiguration;

    [SerializeField] private Transform potOnTableTransform;
    [SerializeField] private Canvas seedsCanvas; 
    [SerializeField] private FlowerName flowerNameEmpty;

    private Pot plantingSeedPot;

    public override void ExecuteClickableAbility()
    {
        if (playerBusyness.IsPlayerFree && playerPickableObjectHandler.CurrentPickableObject is Pot)
        {
            plantingSeedPot = playerPickableObjectHandler.CurrentPickableObject as Pot;

            if (plantingSeedPot.GrowingRoom == growingRoom &&
                plantingSeedPot.IsSoilInsidePot && plantingSeedPot.PlantedFlowerInfo.FlowerName == flowerNameEmpty)
            {
                SetPlayerDestination();
            }
        }
    }

    public override void ExecutePlayerAbility()
    {
        plantingSeedPot.PutOnTableAndKeepPlayerBusy(potOnTableTransform);
        StartCoroutine(OpenSeedsCanvas());
    }

    private IEnumerator OpenSeedsCanvas()
    {
        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay);
        seedsCanvas.enabled = true;
    }

    public void PlantSeed(FlowerInfo transmittedFlowerInfo)
    {
        plantingSeedPot.PlantSeed(transmittedFlowerInfo);
        seedsCanvas.enabled = false;
        plantingSeedPot.TakeInPlayerHandsAndSetPlayerFree();
    }
}
