using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FlowerShop.Environment
{
    public class CarsSpawner : MonoBehaviour
    {
        [Inject] private readonly EnvironmentSettings environmentSettings;

        [SerializeField] private Transform[] spawnTransforms;
        [SerializeField] private Transform[] endTransforms;

        private readonly List<CarMover> carMovers = new();

        private void Start()
        {
            StartCoroutine(SpawnCar());
        }

        public void AddCarMover(CarMover carMover)
        {
            carMovers.Add(carMover);
        }

        private void RemoveCarMover(CarMover carMover)
        {
            carMovers.Remove(carMover);
        }

        private IEnumerator SpawnCar()
        {
            float spawnTime = Random.Range(environmentSettings.CarSpawnMinTime, environmentSettings.CarSpawnMaxTime);

            yield return new WaitForSeconds(spawnTime);

            int carMoverIndex = Random.Range(0, carMovers.Count);
            int pathPointsIndex = Random.Range(0, spawnTransforms.Length);

            carMovers[carMoverIndex].Spawn(spawnTransforms[pathPointsIndex], endTransforms[pathPointsIndex]);
            RemoveCarMover(carMovers[carMoverIndex]);

            if (carMovers.Count > 0)
            {
                StartCoroutine(SpawnCar());
            }
        }
    }
}
