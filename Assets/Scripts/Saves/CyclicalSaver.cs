using UnityEngine;
using Zenject;

namespace Saves
{
    public class CyclicalSaver : MonoBehaviour
    {
        [Inject] private readonly SavesSettings savesSettings;

        public delegate void CyclicalSaverAction();
        public event CyclicalSaverAction CyclicalSaverEvent;

        private float currentSaverTime;

        private void Update()
        {
            currentSaverTime += Time.deltaTime;

            if (currentSaverTime >= savesSettings.SaveTimer)
            {
                currentSaverTime = 0;
                CyclicalSaverEvent?.Invoke();
            }
        }
    }
}