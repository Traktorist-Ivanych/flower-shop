using System.Collections;
using UnityEngine;
using Zenject;

public class CrossingTableProcess : ImprovableBreakableFlowerTable
{
    [Inject] private readonly FlowersContainer flowersContainer;
    [Inject] private readonly PlayerComponents playerComponents;

    [SerializeField] private Transform tablePotTransform;
    [SerializeField] private Transform crossingTableBlender;
    [Header("Indicators")]
    [SerializeField] private MeshRenderer crossingAbilityRedIndicator;
    [SerializeField] private MeshRenderer crossingAbilityGreenIndicator;
    [SerializeField] private MeshRenderer crossedFlowerRoomRedIndicator;
    [SerializeField] private MeshRenderer crossedFlowerRoomBlueIndicator;
    [SerializeField] private MeshRenderer crossedFlowerRoomYellowIndicator;
    [SerializeField] private MeshRenderer crossedFlowerRoomGreenIndicator;
    [Header("CrossingTables")]
    [SerializeField] private CrossingTable firstCrossingTable;
    [SerializeField] private CrossingTable secondCrossingTable;
    [Header("CrossingTableBlender")]
    [SerializeField] private MeshFilter crossingTableBlenderMeshFilter;
    [SerializeField] private Mesh[] crossingTableBlenderLvlMeshes = new Mesh[2];

    private delegate void CrossingTableProcessAction();
    private event CrossingTableProcessAction CrossingTableProcessEvent;

    private Pot potForPlanting;
    private Flower flowerForPlanting;
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
                firstCrossingTable.GetPotOnTable().CrossFlower();
                secondCrossingTable.GetPotOnTable().CrossFlower();
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
                    if (playerDinamicObject.GetCurrentPlayerDinamicObject() is Pot)
                    {
                        potForPlanting = playerDinamicObject.GetCurrentPlayerDinamicObject() as Pot;
                        if (potForPlanting.IsSoilInsidePot && potForPlanting.PlantedFlower.FlowerEnum == IFlower.Flower.Empty &&
                            potForPlanting.GetGroweringRoom() == flowerForPlanting.FlowerGrowingRoom)
                        {
                            SetPlayerDestination();
                            CrossingTableProcessEvent = null;
                            CrossingTableProcessEvent += delegate { StartCoroutine(PlantCrossesSeedProcess()); };
                        }
                    }
                }
                else if (playerDinamicObject.IsPlayerDinamicObjectNull() && isFlowerReadyForCrossing)
                {
                    SetPlayerDestination();
                    CrossingTableProcessEvent = null;
                    CrossingTableProcessEvent += delegate { StartCoroutine(StartFlowerCrossingProcess()); };
                }
            }
            if (playerDinamicObject.GetCurrentPlayerDinamicObject() is Hammer && !isSeedCrossing)
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
                    CrossingTableProcessEvent += ShowImprovableCanvas;
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

        if (firstCrossingTable.IsPotOnCrossingTable && firstCrossingTable.GetPotOnTable().FlowerGrowingLvl >= 3 &&
            secondCrossingTable.IsPotOnCrossingTable && secondCrossingTable.GetPotOnTable().FlowerGrowingLvl >= 3)
        {
            flowerForPlanting = flowersContainer.GetFlowerFromCrossingRecipe(
                firstCrossingTable.GetPotOnTable().PlantedFlower.FlowerEnum,
                secondCrossingTable.GetPotOnTable().PlantedFlower.FlowerEnum
                );

            if (flowerForPlanting.FlowerEnum == IFlower.Flower.Empty)
            {
                isFlowerReadyForCrossing = false;
            }
            else
            {
                crossingAbilityRedIndicator.enabled = false;
                crossedFlowerRoomRedIndicator.enabled = false;

                isFlowerReadyForCrossing = true;
                crossingAbilityGreenIndicator.enabled = true;

                if (flowerForPlanting.FlowerGrowingRoom == IGrowingRoom.GroweringRoom.Wild)
                {
                    crossedFlowerRoomYellowIndicator.enabled = true;
                }
                else if (flowerForPlanting.FlowerGrowingRoom == IGrowingRoom.GroweringRoom.Exotic)
                {
                    crossedFlowerRoomBlueIndicator.enabled = true;
                }
                else if (flowerForPlanting.FlowerGrowingRoom == IGrowingRoom.GroweringRoom.Decorative)
                {
                    crossedFlowerRoomGreenIndicator.enabled = true;
                }
            }
        }
    }

    public override void ImproveTable()
    {
        base.ImproveTable();
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
        potForPlanting.PlantSeed(flowerForPlanting);
        isCrossingSeedReady = false;
        CheckCrossingAbility();

        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay);
        potForPlanting.TakePotInPlayerHands();
    }

    private void FixCrossingTable()
    {
        FixBreakableFlowerTable(
            gameConfiguration.CrossingTableMinQuantity * (tableLvl + 1),
            gameConfiguration.CrossingTableMaxQuantity * (tableLvl + 1));
    }
}