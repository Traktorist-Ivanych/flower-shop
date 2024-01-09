using UnityEngine;
using Zenject;

namespace Saves
{
    public class SavesInstaller : MonoInstaller
    {
        [SerializeField] private CyclicalSaver cyclicalSaver;
        [SerializeField] private SavesSettings savesSettings;

        public override void InstallBindings()
        {
            Container.Bind<CyclicalSaver>().FromInstance(cyclicalSaver).AsSingle().NonLazy();
            Container.Bind<SavesSettings>().FromInstance(savesSettings).AsSingle().NonLazy();
        }
    }
}