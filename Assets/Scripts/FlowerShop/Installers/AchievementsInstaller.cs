using FlowerShop.Achievements;
using UnityEngine;
using Zenject;

namespace FlowerShop.Installers
{
    public class AchievementsInstaller : MonoInstaller
    {
        [Header("Settings")]
        [SerializeField] private AchievementsTextSettings achievementsTextSettings;
        [SerializeField] private AchievementsSettings achievementsSettings;
        [Header("Achievements")]
        [SerializeField] private GreatCollector greatCollector;
        [SerializeField] private AllTheBest allTheBest;
        [SerializeField] private Burnout burnout;
        [SerializeField] private LetThereBeSound letThereBeSound;
        [SerializeField] private ThereIsNoTimeToWait thereIsNoTimeToWait;
        [SerializeField] private PlantGrowingPlant plantGrowingPlant;
        [SerializeField] private KnowALotAboutBusiness knowALotAboutBusiness;
        [SerializeField] private CustomerFocus customerFocus;
        [SerializeField] private TopPlayerFlowerShop topPlayerFlowerShop;
        [SerializeField] private TheBestFlowerShop theBestFlowerShop;
        [SerializeField] private LoverOfDecorativeFlowers loverOfDecorativeFlowers;
        [SerializeField] private CoffeeLover coffeeLover;
        [SerializeField] private WildflowerLover wildflowerLover;
        [SerializeField] private LoverOfExoticFlowers loverOfExoticFlowers;
        [SerializeField] private Handyman handyman;
        [SerializeField] private MusicLover musicLover;
        [SerializeField] private ToCapacity toCapacity;
        [SerializeField] private DecentCitizen decentCitizen;
        [SerializeField] private ExemplaryStudent exemplaryStudent;
        [SerializeField] private TakingCareOfPlants takingCareOfPlants;
        [SerializeField] private Dedication dedication;
        [SerializeField] private WarehouseLogistics warehouseLogistics;
        [SerializeField] private Sprinter sprinter;
        [SerializeField] private HardworkingBreeder hardworkingBreeder;
        [SerializeField] private WeedKiller weedKiller;
        [SerializeField] private AspiringCollector aspiringCollector;

        public override void InstallBindings()
        {
            Container.Bind<AchievementsTextSettings>().FromInstance(achievementsTextSettings).AsSingle().NonLazy();
            Container.Bind<AchievementsSettings>().FromInstance(achievementsSettings).AsSingle().NonLazy();
            
            Container.Bind<GreatCollector>().FromInstance(greatCollector).AsSingle().NonLazy();
            Container.Bind<AllTheBest>().FromInstance(allTheBest).AsSingle().NonLazy();
            Container.Bind<Burnout>().FromInstance(burnout).AsSingle().NonLazy();
            Container.Bind<LetThereBeSound>().FromInstance(letThereBeSound).AsSingle().NonLazy();
            Container.Bind<ThereIsNoTimeToWait>().FromInstance(thereIsNoTimeToWait).AsSingle().NonLazy();
            Container.Bind<PlantGrowingPlant>().FromInstance(plantGrowingPlant).AsSingle().NonLazy();
            Container.Bind<KnowALotAboutBusiness>().FromInstance(knowALotAboutBusiness).AsSingle().NonLazy();
            Container.Bind<CustomerFocus>().FromInstance(customerFocus).AsSingle().NonLazy();
            Container.Bind<TopPlayerFlowerShop>().FromInstance(topPlayerFlowerShop).AsSingle().NonLazy();
            Container.Bind<TheBestFlowerShop>().FromInstance(theBestFlowerShop).AsSingle().NonLazy();
            Container.Bind<LoverOfDecorativeFlowers>().FromInstance(loverOfDecorativeFlowers).AsSingle().NonLazy();
            Container.Bind<CoffeeLover>().FromInstance(coffeeLover).AsSingle().NonLazy();
            Container.Bind<WildflowerLover>().FromInstance(wildflowerLover).AsSingle().NonLazy();
            Container.Bind<LoverOfExoticFlowers>().FromInstance(loverOfExoticFlowers).AsSingle().NonLazy();
            Container.Bind<Handyman>().FromInstance(handyman).AsSingle().NonLazy();
            Container.Bind<MusicLover>().FromInstance(musicLover).AsSingle().NonLazy();
            Container.Bind<ToCapacity>().FromInstance(toCapacity).AsSingle().NonLazy();
            Container.Bind<DecentCitizen>().FromInstance(decentCitizen).AsSingle().NonLazy();
            Container.Bind<ExemplaryStudent>().FromInstance(exemplaryStudent).AsSingle().NonLazy();
            Container.Bind<TakingCareOfPlants>().FromInstance(takingCareOfPlants).AsSingle().NonLazy();
            Container.Bind<Dedication>().FromInstance(dedication).AsSingle().NonLazy();
            Container.Bind<WarehouseLogistics>().FromInstance(warehouseLogistics).AsSingle().NonLazy();
            Container.Bind<Sprinter>().FromInstance(sprinter).AsSingle().NonLazy();
            Container.Bind<HardworkingBreeder>().FromInstance(hardworkingBreeder).AsSingle().NonLazy();
            Container.Bind<WeedKiller>().FromInstance(weedKiller).AsSingle().NonLazy();
            Container.Bind<AspiringCollector>().FromInstance(aspiringCollector).AsSingle().NonLazy();
        }
    }
}