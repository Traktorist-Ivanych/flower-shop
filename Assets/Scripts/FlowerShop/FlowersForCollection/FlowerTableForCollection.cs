using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using PlayerControl;
using UnityEngine;
using Zenject;

/// <summary>
/// better to add description comment (how it is used in gameplay) or rename CollectablesFlowerTables
/// </summary>
public class FlowerTableForCollection : FlowerTable
{
    [Inject] private readonly PlayerComponents playerComponents;

    [SerializeField] private Transform soilTransitionalPosition;
    [SerializeField] private Transform soilTablePosition;
    [SerializeField] private MeshRenderer soilMeshRenderer;
    [SerializeField] private MeshRenderer flowerMeshRenderer;
    [SerializeField] private FlowersForCollection flowersForCollection;
    [SerializeField] private float soilMovingToTransitionalPositionTime;
    [SerializeField] private float soilMovingToTablePositionTime;

    private Pot playerPot;
    private FlowerInfo flowerInfoForCollection;
    private Transform soilTransform;
    private MeshFilter flowerMeshFilter;
    private bool isSoilNeedForMovingToTransitionalPosition;
    private bool isSoilNeedForMovingToTablePosition;
    private float currentMovingTime;

    private void Start()
    {
        soilTransform = soilMeshRenderer.GetComponent<Transform>();
        flowerMeshFilter = flowerMeshRenderer.GetComponent<MeshFilter>();
    }

    private void Update()
    {
        // dotween and move all 'moving' logic to separate class
        if (isSoilNeedForMovingToTransitionalPosition)
        {
            currentMovingTime += Time.deltaTime;
            float lerpT = currentMovingTime / soilMovingToTransitionalPositionTime;
            soilTransform.position = Vector3.Lerp(playerPot.transform.position, soilTransitionalPosition.position, lerpT);

            if (currentMovingTime >= soilMovingToTransitionalPositionTime)
            {
                isSoilNeedForMovingToTransitionalPosition = false;
                currentMovingTime = 0;
                isSoilNeedForMovingToTablePosition = true;
            }
        }
        else if (isSoilNeedForMovingToTablePosition)
        {
            currentMovingTime += Time.deltaTime;
            float lerpT = currentMovingTime / soilMovingToTablePositionTime;
            soilTransform.position = Vector3.Lerp(soilTransitionalPosition.position, soilTablePosition.position, lerpT);

            if (currentMovingTime >= soilMovingToTablePositionTime)
            {
                isSoilNeedForMovingToTablePosition = false;
                playerBusyness.SetPlayerFree();
            }
        }
    }

    public override void ExecuteClickableAbility()
    {
        if (playerBusyness.IsPlayerFree && flowerInfoForCollection == null &&
            playerPickableObjectHandler.CurrentPickableObject is Pot)
        {
            playerPot = playerPickableObjectHandler.CurrentPickableObject as Pot;

            if (playerPot.GrowingRoom == growingRoom && playerPot.FlowerGrowingLvl >= 3 && !playerPot.IsWeedInPot &&
                flowersForCollection.IsFlowerForCollectionUnique(playerPot.PlantedFlowerInfo))
            {
                SetPlayerDestination();
            }
        }
    }

    public override void ExecutePlayerAbility()
    {
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.ThrowTrigger);
        flowerInfoForCollection = playerPot.PlantedFlowerInfo;
        flowersForCollection.AddFlowerToCollectionList(flowerInfoForCollection);
        playerPot.CleanPot();

        soilMeshRenderer.enabled = true;
        flowerMeshRenderer.enabled = true;
        flowerMeshFilter.mesh = flowerInfoForCollection.GetFlowerLvlMesh(3);
        soilTransform.SetPositionAndRotation(playerPot.transform.position, playerPot.transform.rotation);
        isSoilNeedForMovingToTransitionalPosition = true;
    }
}
