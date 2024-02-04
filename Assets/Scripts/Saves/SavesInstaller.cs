using UnityEngine;
using Zenject;

namespace Saves
{
    public class SavesInstaller : MonoInstaller
    {
        [SerializeField] private CyclicalSaver cyclicalSaver;
        [SerializeField] private ReferencesForLoad referencesForLoad;
        [SerializeField] private SavesSettings savesSettings;

        public override void InstallBindings()
        {
            Container.Bind<CyclicalSaver>().FromInstance(cyclicalSaver).AsSingle().NonLazy();
            Container.Bind<ReferencesForLoad>().FromInstance(referencesForLoad).AsSingle().NonLazy();
            Container.Bind<SavesSettings>().FromInstance(savesSettings).AsSingle().NonLazy();
        }
    }
}