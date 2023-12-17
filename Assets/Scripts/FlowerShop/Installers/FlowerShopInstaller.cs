using FlowerShop.Coffee;
using FlowerShop.Customers;
using FlowerShop.Flowers;
using FlowerShop.FlowersSale;
using FlowerShop.PickableObjects;
using FlowerShop.RepairsAndUpgrades;
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
        [Header("Canvas Liaisons")]
        [SerializeField] private CoffeeCanvasLiaison coffeeCanvasLiaison;
        [SerializeField] private UpgradeCanvasLiaison upgradeCanvasLiaison;
        [Header("Repair And Upgrades")]
        [SerializeField] private RepairsAndUpgradesTable repairsAndUpgradesTable;
        [SerializeField] private RepairingAndUpgradingHammer repairingAndUpgradingHammer;

        public override void InstallBindings()
        {
            Container.Bind<FlowersContainer>().FromInstance(flowersContainer).AsSingle().NonLazy();

            Container.Bind<FlowersForSaleCoeffCalculator>().FromInstance(flowersForSaleCoeffCalculator).AsSingle().NonLazy();
            Container.Bind<FlowersSaleTablesForCustomers>().FromInstance(flowersSaleTablesForCustomers).AsSingle().NonLazy();

            Container.Bind<CustomersSpawner>().FromInstance(customersSpawner).AsSingle().NonLazy();

            Container.Bind<CoffeeCanvasLiaison>().FromInstance(coffeeCanvasLiaison).AsSingle().NonLazy();
            Container.Bind<UpgradeCanvasLiaison>().FromInstance(upgradeCanvasLiaison).AsSingle().NonLazy();

            Container.Bind<RepairsAndUpgradesTable>().FromInstance(repairsAndUpgradesTable).AsSingle().NonLazy();
            Container.Bind<RepairingAndUpgradingHammer>().FromInstance(repairingAndUpgradingHammer).AsSingle().NonLazy();
        }
    }
}