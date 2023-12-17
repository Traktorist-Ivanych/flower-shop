using UnityEngine;
using Zenject;

namespace PlayerControl
{
    public class PlayerControlInstaller : MonoInstaller
    {
        [SerializeField] private PlayerAbilityExecutor playerAbilityExecutor;
        [SerializeField] private PlayerBusyness playerBusyness;
        [SerializeField] private PlayerCoffeeEffect playerCoffeeEffect;
        [SerializeField] private PlayerComponents playerComponents;
        [SerializeField] private PlayerControlSettings playerControlSettings;
        [SerializeField] private PlayerMoney playerMoney;
        [SerializeField] private PlayerMoving playerMoving;
        [SerializeField] private PlayerPickableObjectHandler playerPickableObjectHandler;
        [SerializeField] private PlayerStatsCanvasLiaison playerStatsCanvasLiaison;
         

        public override void InstallBindings()
        {
            Container.Bind<PlayerAbilityExecutor>().FromInstance(playerAbilityExecutor).AsSingle().NonLazy();
            Container.Bind<PlayerBusyness>().FromInstance(playerBusyness).AsSingle().NonLazy();
            Container.Bind<PlayerCoffeeEffect>().FromInstance(playerCoffeeEffect).AsSingle().NonLazy();
            Container.Bind<PlayerComponents>().FromInstance(playerComponents).AsSingle().NonLazy();
            Container.Bind<PlayerControlSettings>().FromInstance(playerControlSettings).AsSingle().NonLazy();
            Container.Bind<PlayerMoney>().FromInstance(playerMoney).AsSingle().NonLazy();
            Container.Bind<PlayerMoving>().FromInstance(playerMoving).AsSingle().NonLazy();
            Container.Bind<PlayerPickableObjectHandler>().FromInstance(playerPickableObjectHandler).AsSingle().NonLazy();
            Container.Bind<PlayerStatsCanvasLiaison>().FromInstance(playerStatsCanvasLiaison).AsSingle().NonLazy();
        }
    }
}
