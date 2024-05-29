using FlowerShop.Coffee;
using FlowerShop.Customers;
using FlowerShop.Education;
using FlowerShop.Effects;
using FlowerShop.Environment;
using FlowerShop.Fertilizers;
using FlowerShop.Flowers;
using FlowerShop.FlowersSale;
using FlowerShop.Help;
using FlowerShop.RepairsAndUpgrades;
using FlowerShop.Settings;
using FlowerShop.Sounds;
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
        [SerializeField] private EducationSettings educationSettings;
        [SerializeField] private EffectsSettings effectsSettings;
        [SerializeField] private EnvironmentSettings environmentSettings;
        [SerializeField] private FertilizersSetting fertilizersSetting;
        [SerializeField] private FlowersForSaleCoeffCalculatorSettings flowersForSaleCoeffCalculatorSettings;
        [SerializeField] private FlowersSettings flowersSettings;
        [SerializeField] private HelpTexts helpTexts;
        [SerializeField] private RepairsAndUpgradesSettings repairsAndUpgradesSettings;
        [SerializeField] private SoundSettings soundSettings;
        [SerializeField] private TablesSettings tablesSettings;
        [SerializeField] private WeedSettings weedSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<ActionsWithTransformSettings>().FromInstance(actionsWithTransformSettings).AsSingle().NonLazy();
            Container.Bind<CoffeeSettings>().FromInstance(coffeeSettings).AsSingle().NonLazy();
            Container.Bind<CustomersSettings>().FromInstance(customersSettings).AsSingle().NonLazy();
            Container.Bind<EducationSettings>().FromInstance(educationSettings).AsSingle().NonLazy();
            Container.Bind<EffectsSettings>().FromInstance(effectsSettings).AsSingle().NonLazy();
            Container.Bind<EnvironmentSettings>().FromInstance(environmentSettings).AsSingle().NonLazy();
            Container.Bind<FertilizersSetting>().FromInstance(fertilizersSetting).AsSingle().NonLazy();
            Container.Bind<FlowersForSaleCoeffCalculatorSettings>()
                .FromInstance(flowersForSaleCoeffCalculatorSettings).AsSingle().NonLazy();
            Container.Bind<FlowersSettings>().FromInstance(flowersSettings).AsSingle().NonLazy();
            Container.Bind<HelpTexts>().FromInstance(helpTexts).AsSingle().NonLazy();
            Container.Bind<RepairsAndUpgradesSettings>().FromInstance(repairsAndUpgradesSettings).AsSingle().NonLazy();
            Container.Bind<SoundSettings>().FromInstance(soundSettings).AsSingle().NonLazy();
            Container.Bind<TablesSettings>().FromInstance(tablesSettings).AsSingle().NonLazy();
            Container.Bind<WeedSettings>().FromInstance(weedSettings).AsSingle().NonLazy();
        }
    }
}