using FlowerShop.Coffee;
using FlowerShop.Customers;
using FlowerShop.Flowers;
using FlowerShop.FlowerSales;
using FlowerShop.RepairsAndUpgrades;
using FlowerShop.Settings;
using FlowerShop.Tables;
using FlowerShop.Weeds;
using UnityEngine;
using Zenject;

namespace FlowerShop.Installers
{
    public class SettingInstaller : MonoInstaller
    {
        [SerializeField] private ActionsWithTransformSettings actionsWithTransformSettings;
        [SerializeField] private CoffeeSettings coffeeSettings;
        [SerializeField] private CustomersSettings customersSettings;
        [SerializeField] private FlowersSettings flowersSettings;
        [SerializeField] private TablesSettings tablesSettings;
        [SerializeField] private WeedSettings weedSettings;
        [SerializeField] private RepairsAndUpgradesSettings repairsAndUpgradesSettings;
        [SerializeField] private FlowersForSaleCoeffCalculatorSettings flowersForSaleCoeffCalculatorSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<ActionsWithTransformSettings>().FromInstance(actionsWithTransformSettings).AsSingle().NonLazy();
            Container.Bind<CoffeeSettings>().FromInstance(coffeeSettings).AsSingle().NonLazy();
            Container.Bind<CustomersSettings>().FromInstance(customersSettings).AsSingle().NonLazy();
            Container.Bind<FlowersSettings>().FromInstance(flowersSettings).AsSingle().NonLazy();
            Container.Bind<TablesSettings>().FromInstance(tablesSettings).AsSingle().NonLazy();
            Container.Bind<WeedSettings>().FromInstance(weedSettings).AsSingle().NonLazy();
            Container.Bind<RepairsAndUpgradesSettings>().FromInstance(repairsAndUpgradesSettings).AsSingle().NonLazy();
            Container.Bind<FlowersForSaleCoeffCalculatorSettings>()
                .FromInstance(flowersForSaleCoeffCalculatorSettings).AsSingle().NonLazy();
        }
    }
}