using UnityEngine;
using Zenject;

namespace PlayerControl
{
    public class PlayerControlInstaller : MonoInstaller
    {
        [SerializeField] private PlayerStatsCanvasLiaison playerStatsCanvasLiaison;
        [SerializeField] private PlayerControlSettings playerControlSettings;

        public override void InstallBindings()
        {
            Container.Bind<PlayerStatsCanvasLiaison>().FromInstance(playerStatsCanvasLiaison).AsSingle().NonLazy();
            Container.Bind<PlayerControlSettings>().FromInstance(playerControlSettings).AsSingle().NonLazy();
        }
    }
}
