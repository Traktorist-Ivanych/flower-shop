using FlowerShop.Coffee;
using FlowerShop.Customers;
using FlowerShop.Effects;
using FlowerShop.Environment;
using FlowerShop.Fertilizers;
using FlowerShop.Flowers;
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
        [Header("Flowers Container")]
        [SerializeField] private FlowersContainer flowersContainer;
        [Header("Sale")]
        [SerializeField] private FlowersForSaleCoeffCalculator flowersForSaleCoeffCalculator;
        [SerializeField] private FlowersSaleTablesForCustomers flowersSaleTablesForCustomers;
        [Header("Customers")]
        [SerializeField] private CustomersSpawner customersSpawner;
        [SerializeField] private CustomersObserver customersObserver;
        [Header("Canvas Liaisons")]
        [SerializeField] private CoffeeCanvasLiaison coffeeCanvasLiaison;
        [SerializeField] private UpgradeCanvasLiaison upgradeCanvasLiaison;
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

            Container.Bind<FlowersForSaleCoeffCalculator>().FromInstance(flowersForSaleCoeffCalculator).AsSingle().NonLazy();
            Container.Bind<FlowersSaleTablesForCustomers>().FromInstance(flowersSaleTablesForCustomers).AsSingle().NonLazy();

            Container.Bind<CustomersSpawner>().FromInstance(customersSpawner).AsSingle().NonLazy();
            Container.Bind<CustomersObserver>().FromInstance(customersObserver).AsSingle().NonLazy();

            Container.Bind<CoffeeCanvasLiaison>().FromInstance(coffeeCanvasLiaison).AsSingle().NonLazy();
            Container.Bind<UpgradeCanvasLiaison>().FromInstance(upgradeCanvasLiaison).AsSingle().NonLazy();

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