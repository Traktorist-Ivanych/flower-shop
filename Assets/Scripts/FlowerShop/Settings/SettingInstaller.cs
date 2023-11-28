using UnityEngine;
using Zenject;

namespace FlowerShop.Settings
{
    public class SettingInstaller : MonoInstaller
    {
        [SerializeField] private ActionsWithTransformSettings actionsWithTransformSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<ActionsWithTransformSettings>().FromInstance(actionsWithTransformSettings).AsSingle().NonLazy();
        }
    }
}