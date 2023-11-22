using System.Collections;
using UnityEngine;
using Zenject;

public class PlantingSeedsTable : FlowerTable
{
    [Inject] private readonly GameConfiguration gameConfiguration;

    [SerializeField] private Transform potOnTableTransform;
    [SerializeField] private Canvas seedsCanvas;

    private Pot plantingSeedPot;

    public override void ExecuteClickableAbility()
    {
        if (playerBusyness.IsPlayerFree && PlayerPickableObjectHandler.GetCurrentPlayerPickableObject() is Pot)
        {
            plantingSeedPot = PlayerPickableObjectHandler.GetCurrentPlayerPickableObject() as Pot;

            if (plantingSeedPot.GetGroweringRoom() == groweringRoom &&
                plantingSeedPot.IsSoilInsidePot && plantingSeedPot.PlantedFlower.FlowerEnum == IFlower.Flower.Empty)
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

    public void PlantSeed(Flower transmittedFlower)
    {
        plantingSeedPot.PlantSeed(transmittedFlower);
        seedsCanvas.enabled = false;
        plantingSeedPot.TakePotInPlayerHands();
    }
}
