using DG.Tweening;
using UnityEngine;
using Zenject;

namespace FlowerShop.Environment
{
    public class CarMover : MonoBehaviour
    {
        [Inject] private readonly EnvironmentSettings environmentSettings;

        [SerializeField] private CarsSpawner carsSpawner;

        [HideInInspector, SerializeField] private Transform carTransform;

        private Transform endPathTransform;

        private void OnValidate()
        {
            carTransform = transform;
        }

        private void Awake()
        {
            carsSpawner.AddCarMover(this);
        }

        public void Spawn(Transform startTransform, Transform endTransform)
        {
            carTransform.SetPositionAndRotation(startTransform.position, startTransform.rotation);
            endPathTransform = endTransform;

            carTransform.DOMove(endPathTransform.position, environmentSettings.CarMovingTime).OnComplete(FinishMoving);
        }

        private void FinishMoving()
        {
            carsSpawner.AddCarMover(this);
        }
    }
}
