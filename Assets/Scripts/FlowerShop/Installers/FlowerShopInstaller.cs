using FlowerShop.Coffee;
using FlowerShop.ComputerPages;
using FlowerShop.Customers;
using FlowerShop.Customers.VipAndComplaints;
using FlowerShop.Effects;
using FlowerShop.Environment;
using FlowerShop.Fertilizers;
using FlowerShop.Flowers;
using FlowerShop.FlowersForCollection;
using FlowerShop.FlowersSale;
using FlowerShop.PickableObjects;
using FlowerShop.RepairsAndUpgrades;
using FlowerShop.Sounds;
using FlowerShop.Tables;
using UnityEngine;
using Zenject;

namespace FlowerShop.Installers
{
    public class FlowerShopInstaller : MonoInstaller
    {
        [Header("Flowers Containers")]
        [SerializeField] private FlowersContainer flowersContainer;
        [SerializeField] private FlowersForPlayerCollection flowersForPlayerCollection;
        [Header("Sale")]
        [SerializeField] private FlowersForSaleCoeffCalculator flowersForSaleCoeffCalculator;
        [SerializeField] private FlowersSaleTablesForCustomers flowersSaleTablesForCustomers;
        [Header("Customers")] 
        [SerializeField] private ComplaintsHandler complaintsHandler;
        [SerializeField] private CustomersSpawner customersSpawner;
        [SerializeField] private CustomersObserver customersObserver;
        [SerializeField] private VipOrdersHandler vipOrdersHandler;
        [Header("Canvas Liaisons")]
        [SerializeField] private CoffeeCanvasLiaison coffeeCanvasLiaison;
        [SerializeField] private ComplaintsCanvasLiaison complaintsCanvasLiaison;
        [SerializeField] private ComputerMainPageCanvasLiaison computerMainPageCanvas;
        [SerializeField] private FlowerInfoCanvasLiaison flowerInfoCanvasLiaison;
        [SerializeField] private FlowersCanvasLiaison flowersCanvasLiaison;
        [SerializeField] private UpgradeCanvasLiaison upgradeCanvasLiaison;
        [SerializeField] private VipCanvasLiaison vipCanvasLiaison;
        [Header("Canvas Elements")]
        [SerializeField] private CanvasIndicators canvasIndicators;
        [Header("Repair And Upgrades")]
        [SerializeField] private RepairsAndUpgradesTable repairsAndUpgradesTable;
        [SerializeField] private RepairingAndUpgradingHammer repairingAndUpgradingHammer;
        [Header("Fertilizers")] 
        [SerializeField] private FertilizersTable fertilizersTable;
        [SerializeField] private FertilizersCanvasLiaison fertilizersCanvasLiaison;
        [Header("Player")]
        [SerializeField] private PlayerSounds playerSounds;
        [Header("Sounds")]
        [SerializeField] private SoundsHandler soundsHandler;
        [Header("Environment")]
        [SerializeField] private AutomaticDoors automaticDoors;
        [Header("Effects")]
        [SerializeField] private SelectedTableEffect selectedTableEffect;

        public override void InstallBindings()
        {
            Container.Bind<FlowersContainer>().FromInstance(flowersContainer).AsSingle().NonLazy();
            Container.Bind<FlowersForPlayerCollection>().FromInstance(flowersForPlayerCollection).AsSingle().NonLazy();

            Container.Bind<FlowersForSaleCoeffCalculator>().FromInstance(flowersForSaleCoeffCalculator).AsSingle().NonLazy();
            Container.Bind<FlowersSaleTablesForCustomers>().FromInstance(flowersSaleTablesForCustomers).AsSingle().NonLazy();

            Container.Bind<ComplaintsHandler>().FromInstance(complaintsHandler).AsSingle().NonLazy();
            Container.Bind<CustomersSpawner>().FromInstance(customersSpawner).AsSingle().NonLazy();
            Container.Bind<CustomersObserver>().FromInstance(customersObserver).AsSingle().NonLazy();
            Container.Bind<VipOrdersHandler>().FromInstance(vipOrdersHandler).AsSingle().NonLazy();

            Container.Bind<CoffeeCanvasLiaison>().FromInstance(coffeeCanvasLiaison).AsSingle().NonLazy();
            Container.Bind<ComplaintsCanvasLiaison>().FromInstance(complaintsCanvasLiaison).AsSingle().NonLazy();
            Container.Bind<ComputerMainPageCanvasLiaison>().FromInstance(computerMainPageCanvas).AsSingle().NonLazy();
            Container.Bind<FlowerInfoCanvasLiaison>().FromInstance(flowerInfoCanvasLiaison).AsSingle().NonLazy();
            Container.Bind<FlowersCanvasLiaison>().FromInstance(flowersCanvasLiaison).AsSingle().NonLazy();
            Container.Bind<UpgradeCanvasLiaison>().FromInstance(upgradeCanvasLiaison).AsSingle().NonLazy();
            Container.Bind<VipCanvasLiaison>().FromInstance(vipCanvasLiaison).AsSingle().NonLazy();
            
            Container.Bind<CanvasIndicators>().FromInstance(canvasIndicators).AsSingle().NonLazy();

            Container.Bind<RepairsAndUpgradesTable>().FromInstance(repairsAndUpgradesTable).AsSingle().NonLazy();
            Container.Bind<RepairingAndUpgradingHammer>().FromInstance(repairingAndUpgradingHammer).AsSingle().NonLazy();
            
            Container.Bind<FertilizersTable>().FromInstance(fertilizersTable).AsSingle().NonLazy();
            Container.Bind<FertilizersCanvasLiaison>().FromInstance(fertilizersCanvasLiaison).AsSingle().NonLazy();
            
            Container.Bind<PlayerSounds>().FromInstance(playerSounds).AsSingle().NonLazy();
            
            Container.Bind<SoundsHandler>().FromInstance(soundsHandler).AsSingle().NonLazy();
            
            Container.Bind<AutomaticDoors>().FromInstance(automaticDoors).AsSingle().NonLazy();
            
            Container.Bind<SelectedTableEffect>().FromInstance(selectedTableEffect).AsSingle().NonLazy();
        }
    }
}