using FlowerShop.Coffee;
using FlowerShop.Customers;
using UnityEngine;
using Zenject;

namespace FlowerShop.Settings
{
    public class SettingInstaller : MonoInstaller
    {
        [SerializeField] private ActionsWithTransformSettings actionsWithTransformSettings;
        [SerializeField] private CoffeeSettings coffeeSettings;
        [SerializeField] private CustomersSettings customersSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<ActionsWithTransformSettings>().FromInstance(actionsWithTransformSettings).AsSingle().NonLazy();
            Container.Bind<CoffeeSettings>().FromInstance(coffeeSettings).AsSingle().NonLazy();
            Container.Bind<CustomersSettings>().FromInstance(customersSettings).AsSingle().NonLazy();
        }
    }
}