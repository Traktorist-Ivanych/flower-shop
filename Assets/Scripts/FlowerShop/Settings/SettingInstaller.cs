using FlowerShop.Coffee;
using FlowerShop.Customers;
using FlowerShop.Flowers;
using FlowerShop.FlowerSales;
using UnityEngine;
using Zenject;

namespace FlowerShop.Settings
{
    public class SettingInstaller : MonoInstaller
    {
        [SerializeField] private ActionsWithTransformSettings actionsWithTransformSettings;
        [SerializeField] private CoffeeSettings coffeeSettings;
        [SerializeField] private CustomersSettings customersSettings;
        [SerializeField] private FlowersSettings flowersSettings;
        [SerializeField] private FlowersForSaleCoeffCalculatorSettings flowersForSaleCoeffCalculatorSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<ActionsWithTransformSettings>().FromInstance(actionsWithTransformSettings).AsSingle().NonLazy();
            Container.Bind<CoffeeSettings>().FromInstance(coffeeSettings).AsSingle().NonLazy();
            Container.Bind<CustomersSettings>().FromInstance(customersSettings).AsSingle().NonLazy();
            Container.Bind<FlowersSettings>().FromInstance(flowersSettings).AsSingle().NonLazy();
            Container.Bind<FlowersForSaleCoeffCalculatorSettings>()
                .FromInstance(flowersForSaleCoeffCalculatorSettings).AsSingle().NonLazy();
        }
    }
}