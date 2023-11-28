using System.Collections;
using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using PlayerControl;
using UnityEngine;
using Zenject;

public class CrossingTableProcess : UpgradableBreakableFlowerTable
{
    [Inject] private readonly FlowersContainer flowersContainer;
    [Inject] private readonly PlayerComponents playerComponents;

    [SerializeField] private Transform tablePotTransform;
    [SerializeField] private Transform crossingTableBlender;
    [SerializeField] private Flower flowerEmpty;
    
    [Header("Indicators")]
    [SerializeField] private MeshRenderer crossingAbilityRedIndicator;
    [SerializeField] private MeshRenderer crossingAbilityGreenIndicator;
    [SerializeField] private MeshRenderer crossedFlowerRoomRedIndicator;
    [SerializeField] private MeshRenderer crossedFlowerRoomBlueIndicator;
    [SerializeField] private MeshRenderer crossedFlowerRoomYellowIndicator;
    [SerializeField] private MeshRenderer crossedFlowerRoomGreenIndicator;
    
    [Header("CrossingTables")]
    [SerializeField] private FlowersCrossingTable firstFlowersCrossingTable;
    [SerializeField] private FlowersCrossingTable secondFlowersCrossingTable;
    
    [Header("CrossingTableBlender")]
    [SerializeField] private MeshFilter crossingTableBlenderMeshFilter;
    [SerializeField] private Mesh[] crossingTableBlenderLvlMeshes = new Mesh[2];

    [Header("GrowingRooms")] 
    [SerializeField] private GrowingRoom growingRoomWild;
    [SerializeField] private GrowingRoom growingRoomExotic;
    [SerializeField] private GrowingRoom growingRoomDecorative;

    private delegate void CrossingTableProcessAction();
    private event CrossingTableProcessAction CrossingTableProcessEvent;

    private Pot potForPlanting;
    private FlowerInfo flowerInfoForPlanting;
    private float crossingFlowerTime;
    private float currentCrossingFlowerTime;
    private bool isSeedCrossing;
    private bool isFlowerReadyForCrossing;
    private bool isCrossingSeedReady;

    public bool IsSeedCrossing
    {
        get => isSeedCrossing;
    }

    private protected override void Start()
    {
        base.Start();
        SetCrossingFlowerTime();

        SetActionsBeforeBrokenQuantity(
            gameConfiguration.CrossingTableMinQuantity * (tableLvl + 1),
            gameConfiguration.CrossingTableMaxQuantity * (tableLvl + 1));
    }

    private void Update()
    {
        if (isSeedCrossing)
        {
            crossingTableBlender.Rotate(Vector3.forward, gameConfiguration.ObjectsRotateDegreesPerSecond * Time.deltaTime);

            if (currentCrossingFlowerTime <= crossingFlowerTime)
            {
                currentCrossingFlowerTime += Time.deltaTime;
            }
            else
            {
                isSeedCrossing = false;
                currentCrossingFlowerTime = 0;
                isCrossingSeedReady = true;

                crossingAbilityRedIndicator.enabled = true;
                crossingAbilityGreenIndicator.enabled = false;
                firstFlowersCrossingTable.GetPotOnTable().CrossFlower();
                secondFlowersCrossingTable.GetPotOnTable().CrossFlower();
                UseBreakableFlowerTable();
            }
        }
    }

    public override void ExecuteClickableAbility()
    {
        if (playerBusyness.IsPlayerFree && !isSeedCrossing)
        {
            if (!IsTableBroken)
            {
                if (isCrossingSeedReady)
                {
                    if (playerPickableObjectHandler.CurrentPickableObject is Pot)
                    {
                        potForPlanting = playerPickableObjectHandler.CurrentPickableObject as Pot;
                        if (potForPlanting.IsSoilInsidePot && potForPlanting.PlantedFlowerInfo.Flower == flowerEmpty &&
                            potForPlanting.GrowingRoom == flowerInfoForPlanting.GrowingRoom)
                        {
                            SetPlayerDestination();
                            CrossingTableProcessEvent = null;
                            CrossingTableProcessEvent += delegate { StartCoroutine(PlantCrossesSeedProcess()); };
                        }
                    }
                }
                else if (playerPickableObjectHandler.IsPickableObjectNull && isFlowerReadyForCrossing)
                {
                    SetPlayerDestination();
                    CrossingTableProcessEvent = null;
                    CrossingTableProcessEvent += delegate { StartCoroutine(StartFlowerCrossingProcess()); };
                }
            }
            if (playerPickableObjectHandler.CurrentPickableObject is UpgradingAndRepairingHammer && !isSeedCrossing)
            {
                if (IsTableBroken)
                {
                    SetPlayerDestination();
                    CrossingTableProcessEvent = null;
                    CrossingTableProcessEvent += FixCrossingTable;
                }
                else if (tableLvl < 2)
                {
                    SetPlayerDestination();
                    CrossingTableProcessEvent = null;
                    CrossingTableProcessEvent += ShowUpgradeCanvas;
                }
            }
        }
    }

    public override void ExecutePlayerAbility()
    {
        CrossingTableProcessEvent?.Invoke();
    }

    public void CheckCrossingAbility()
    {
        crossingAbilityRedIndicator.enabled = true;
        crossingAbilityGreenIndicator.enabled = false;

        crossedFlowerRoomRedIndicator.enabled = true;
        crossedFlowerRoomBlueIndicator.enabled = false;
        crossedFlowerRoomYellowIndicator.enabled = false;
        crossedFlowerRoomGreenIndicator.enabled = false;

        if (firstFlowersCrossingTable.IsPotOnCrossingTable && firstFlowersCrossingTable.GetPotOnTable().FlowerGrowingLvl >= 3 &&
            secondFlowersCrossingTable.IsPotOnCrossingTable && secondFlowersCrossingTable.GetPotOnTable().FlowerGrowingLvl >= 3)
        {
            flowerInfoForPlanting = flowersContainer.GetFlowerFromCrossingRecipe(
                firstFlowersCrossingTable.GetPotOnTable().PlantedFlowerInfo.Flower,
                secondFlowersCrossingTable.GetPotOnTable().PlantedFlowerInfo.Flower);

            if (flowerInfoForPlanting.Flower == flowerEmpty)
            {
                isFlowerReadyForCrossing = false;
            }
            else
            {
                crossingAbilityRedIndicator.enabled = false;
                crossedFlowerRoomRedIndicator.enabled = false;

                isFlowerReadyForCrossing = true;
                crossingAbilityGreenIndicator.enabled = true;

                if (flowerInfoForPlanting.GrowingRoom == growingRoomWild)
                {
                    crossedFlowerRoomYellowIndicator.enabled = true;
                }
                else if (flowerInfoForPlanting.GrowingRoom == growingRoomExotic)
                {
                    crossedFlowerRoomBlueIndicator.enabled = true;
                }
                else if (flowerInfoForPlanting.GrowingRoom == growingRoomDecorative)
                {
                    crossedFlowerRoomGreenIndicator.enabled = true;
                }
            }
        }
    }

    public override void UpgradeTable()
    {
        base.UpgradeTable();
        SetCrossingFlowerTime();
        crossingTableBlenderMeshFilter.mesh = crossingTableBlenderLvlMeshes[tableLvl - 1];
    }

    private void SetCrossingFlowerTime()
    {
        crossingFlowerTime = gameConfiguration.CrossingFlowerTime - gameConfiguration.CrossingFlowerLvlTimeDelta * tableLvl;
    }

    private IEnumerator StartFlowerCrossingProcess()
    {
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartCrossingTrigger);

        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay);
        isSeedCrossing = true;
    }

    private IEnumerator PlantCrossesSeedProcess()
    {
        potForPlanting.GivePotAndKeepPlayerBusy(tablePotTransform);

        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay);
        potForPlanting.PlantSeed(flowerInfoForPlanting);
        isCrossingSeedReady = false;
        CheckCrossingAbility();

        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay);
        potForPlanting.TakeInPlayerHands();
    }

    private void FixCrossingTable()
    {
        FixBreakableFlowerTable(
            gameConfiguration.CrossingTableMinQuantity * (tableLvl + 1),
            gameConfiguration.CrossingTableMaxQuantity * (tableLvl + 1));
    }
}