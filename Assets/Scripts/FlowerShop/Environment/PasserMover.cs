using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace FlowerShop.Environment
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PasserMover : MonoBehaviour
    {
        [Inject] private readonly EnvironmentSettings customersSettings;
        [Inject] private readonly PassersSpawner passersSpawner;

        [HideInInspector, SerializeField] private NavMeshAgent navAgent;

        private PasserPath passerPath;
        private int currentPathPointIndex;
        private bool isPasserActive;

        private void OnValidate()
        {
            navAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (isPasserActive && navAgent.remainingDistance <= customersSettings.MinRemainingDistanceBetweenPathPoints)
            {
                if (passerPath.IsPasserOnEndTransform(currentPathPointIndex))
                {
                    currentPathPointIndex = 0;
                    SetPasserPath(passersSpawner.GetNextPasserPath());
                }
                else
                {
                    currentPathPointIndex++;
                    navAgent.destination = passerPath.GetNextPathTransform(currentPathPointIndex).position;
                }
            }
        }

        public void SetPasserPath(PasserPath nextPasserPath)
        {
            isPasserActive = true;
            passerPath = nextPasserPath;

            navAgent.Warp(passerPath.GetSpawnTransform().position);
            currentPathPointIndex++;
            navAgent.destination = passerPath.GetNextPathTransform(currentPathPointIndex).position;
        }
    }
}
