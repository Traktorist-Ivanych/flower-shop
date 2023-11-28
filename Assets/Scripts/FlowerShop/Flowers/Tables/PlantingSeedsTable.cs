using System.Collections;
using FlowerShop.Flowers;
using UnityEngine;
using Zenject;

public class PlantingSeedsTable : FlowerTable
{
    [Inject] private readonly GameConfiguration gameConfiguration;

    [SerializeField] private Transform potOnTableTransform;
    [SerializeField] private Canvas seedsCanvas;
    [SerializeField] private Flower flowerEmpty;

    private Pot plantingSeedPot;

    public override void ExecuteClickableAbility()
    {
        if (playerBusyness.IsPlayerFree && playerPickableObjectHandler.CurrentPickableObject is Pot)
        {
            plantingSeedPot = playerPickableObjectHandler.CurrentPickableObject as Pot;

            if (plantingSeedPot.GrowingRoom == growingRoom &&
                plantingSeedPot.IsSoilInsidePot && plantingSeedPot.PlantedFlowerInfo.Flower == flowerEmpty)
            {
                SetPlayerDestination();
            }
        }
    }

    public override void ExecutePlayerAbility()
    {
        plantingSeedPot.GivePotAndKeepPlayerBusy(potOnTableTransform);
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
        plantingSeedPot.TakeInPlayerHands();
    }
}
