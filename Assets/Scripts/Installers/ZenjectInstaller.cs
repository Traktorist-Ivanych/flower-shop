using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Zenject;

public class ZenjectInstaller : MonoInstaller
{
    [Header("Player")]
    [SerializeField] private NavMeshAgent playerAgent;
    [SerializeField] private PlayerMoving playerMoving;
    [SerializeField] private PlayerAbilityExecutor playerAbilityExecutor;
    [FormerlySerializedAs("currentPlayerDinamicObject")] [SerializeField] private PlayerPickableObjectHandler playerPickableObjectHandler;
    [SerializeField] private PlayerBusyness playerBusyness;
    [SerializeField] private PlayerComponents playerComponents;
    [SerializeField] private PlayerMoney playerMoney;
    [FormerlySerializedAs("playerCoffeEffect")] [SerializeField] private PlayerCoffeeEffect playerCoffeeEffect;
    [Header("Crossing")]
    [SerializeField] private FlowersContainer flowersContainer;
    [Header("Sale")]
    [SerializeField] private FlowersForSaleCoefCalculator flowersForSaleCoefCalculator;
    [SerializeField] private FlowerSaleTablesForByers flowerSaleTablesForByers;
    [Header("Configuration")]
    [SerializeField] private GameConfiguration gameConfiguration;
    [Header("Buyers")]
    [SerializeField] private BuyersSpawner buyersSpawner;
    [Header("Canvases")]
    [SerializeField] private AllCanvasLiaisons allCanvasLiaisons;
    [Header("Repair And Improvement")]
    [SerializeField] private RepairAndImprovementTable repairAndImprovementTable;
    [SerializeField] private Hammer hammer;

    public override void InstallBindings()
    {
        Container.Bind<NavMeshAgent>().FromInstance(playerAgent).AsSingle().NonLazy();
        Container.Bind<PlayerMoving>().FromInstance(playerMoving).AsSingle().NonLazy();
        Container.Bind<PlayerAbilityExecutor>().FromInstance(playerAbilityExecutor).AsSingle().NonLazy();
        Container.Bind<PlayerPickableObjectHandler>().FromInstance(playerPickableObjectHandler).AsSingle().NonLazy();
        Container.Bind<PlayerBusyness>().FromInstance(playerBusyness).AsSingle().NonLazy();
        Container.Bind<PlayerComponents>().FromInstance(playerComponents).AsSingle().NonLazy();
        Container.Bind<PlayerMoney>().FromInstance(playerMoney).AsSingle().NonLazy();
        Container.Bind<PlayerCoffeeEffect>().FromInstance(playerCoffeeEffect).AsSingle().NonLazy();

        Container.Bind<FlowersContainer>().FromInstance(flowersContainer).AsSingle().NonLazy();

        Container.Bind<FlowersForSaleCoefCalculator>().FromInstance(flowersForSaleCoefCalculator).AsSingle().NonLazy();
        Container.Bind<FlowerSaleTablesForByers>().FromInstance(flowerSaleTablesForByers).AsSingle().NonLazy();

        Container.Bind<GameConfiguration>().FromInstance(gameConfiguration).AsSingle().NonLazy();

        Container.Bind<BuyersSpawner>().FromInstance(buyersSpawner).AsSingle().NonLazy();

        Container.Bind<AllCanvasLiaisons>().FromInstance(allCanvasLiaisons).AsSingle().NonLazy();

        Container.Bind<RepairAndImprovementTable>().FromInstance(repairAndImprovementTable).AsSingle().NonLazy();
        Container.Bind<Hammer>().FromInstance(hammer).AsSingle().NonLazy();
    }
}