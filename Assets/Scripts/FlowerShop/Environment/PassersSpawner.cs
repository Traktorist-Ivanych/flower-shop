using System.Collections;
using UnityEngine;
using Zenject;

namespace FlowerShop.Environment
{
    public class PassersSpawner : MonoBehaviour
    {
        [Inject] private readonly EnvironmentSettings environmentSettings;

        [SerializeField] private PasserPath[] passersPathes;
        [SerializeField] private PasserMover[] passerMovers;

        private int activePassersCount;
        private int currentPasserPathIndex;

        private void Start()
        {
            StartCoroutine(SpawnPasser());
        }

        public PasserPath GetNextPasserPath()
        {
            currentPasserPathIndex += Random.Range(1, passersPathes.Length);
            if (currentPasserPathIndex >= passersPathes.Length)
            {
                currentPasserPathIndex -= passersPathes.Length;
            }
            return passersPathes[currentPasserPathIndex];
        }

        private IEnumerator SpawnPasser()
        {
            yield return new WaitForSeconds(environmentSettings.SpawnPasserDelay);

            passerMovers[activePassersCount].SetPasserPath(GetNextPasserPath());
            activePassersCount++;

            if (activePassersCount < passerMovers.Length)
            {
                StartCoroutine(SpawnPasser());
            }
        }
    }
}
