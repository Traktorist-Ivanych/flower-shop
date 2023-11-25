using UnityEngine;
using Zenject;

namespace Input
{
    public class InputInstaller : MonoInstaller
    {
        [SerializeField] private CameraHandler cameraHandler;
        [SerializeField] private MainCameraMover mainCameraMover;
        [SerializeField] private MainCameraMovingSetting mainCameraMovingSetting;
        [SerializeField] private PlayerTapInput playerTapInput;
        [SerializeField] private PlayerTapSettings playerTapSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<CameraHandler>().FromInstance(cameraHandler).AsSingle().NonLazy();
            Container.Bind<MainCameraMover>().FromInstance(mainCameraMover).AsSingle().NonLazy();
            Container.Bind<MainCameraMovingSetting>().FromInstance(mainCameraMovingSetting).AsSingle().NonLazy();
            Container.Bind<PlayerTapInput>().FromInstance(playerTapInput).AsSingle().NonLazy();
            Container.Bind<PlayerTapSettings>().FromInstance(playerTapSettings).AsSingle().NonLazy();
        }
    }
}