using System.Collections.Generic;
using FlowerShop.FlowersSale;
using FlowerShop.Saves.SaveData;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Customers
{
    public class CustomersSpawner : MonoBehaviour, ISavableObject
    {
        [Inject] private readonly CustomersSettings customersSettings;
        [Inject] private readonly CyclicalSaver cyclicalSaver;
        [Inject] private readonly FlowersForSaleCoeffCalculator flowersForSaleCoeffCalculator;

        [SerializeField] private List<Transform> spawnPathPoints;
        [SerializeField] private List<Transform> enterShopPathPoints;
        [SerializeField] private List<Transform> nextLookAroundPathPoints;
        [SerializeField] private List<Transform> leftFinishPathPoints;
        [SerializeField] private List<Transform> rightFinishPathPoints;

        private readonly List<CustomerMoving> customersMoving = new();
        private float minSpawnTime;
        private float maxSpawnTime;
        private float currentSpawnTime;
        private int pathIndexes;
        private int nextLookAroundPathPointsIndex;
        private int finishPathPointsIndex;

        [field: SerializeField] public string UniqueKey { get; private set; }

        private void Awake()
        {
            Load();
        }

        private void Start()
        {
            TryToSpawnCustomer();
        }

        private void OnEnable()
        {
            cyclicalSaver.CyclicalSaverEvent += Save;
        }

        private void OnDisable()
        {
            cyclicalSaver.CyclicalSaverEvent -= Save;
        }

        private void Update()
        {
            if (currentSpawnTime > 0)
            {
                currentSpawnTime -= Time.deltaTime;
            }
            else
            {
                CalculateCurrentSpawnTime();
                TryToSpawnCustomer();
            }
        }

        public void AddBuyerMoving(CustomerMoving customerMoving)
        {
            customersMoving.Add(customerMoving);
        }

        public void RemoveCustomerMoving(CustomerMoving customerMoving)
        {
            if (customersMoving.Contains(customerMoving))
            {
                customersMoving.Remove(customerMoving);
            }
        }

        public List<Transform> GetFinishPathPoints()
        {
            finishPathPointsIndex++;
            if (finishPathPointsIndex == 1)
            {
                return leftFinishPathPoints;
            }
            finishPathPointsIndex = 0;
            return rightFinishPathPoints;
        }
        
        public Transform GetNextLookAroundPathPoint()
        {
            nextLookAroundPathPointsIndex++;

            if (nextLookAroundPathPointsIndex >= nextLookAroundPathPoints.Count)
            {
                nextLookAroundPathPointsIndex = 0;
            }

            return nextLookAroundPathPoints[nextLookAroundPathPointsIndex];
        }

        public void Load()
        {
            FloatProgressForSaving floatProgressForLoading = SavesHandler.Load<FloatProgressForSaving>(UniqueKey);

            if (floatProgressForLoading.IsValuesSaved)
            {
                currentSpawnTime = floatProgressForLoading.Progress;
            }
        }

        public void Save()
        {
            FloatProgressForSaving floatProgressForSaving = new(currentSpawnTime);
            
            SavesHandler.Save(UniqueKey, floatProgressForSaving);
        }

        private void CalculateCurrentSpawnTime()
        {
            minSpawnTime = customersSettings.MinSpawnTime -
                           customersSettings.MinSpawnTimeDelta * flowersForSaleCoeffCalculator.CurrentFlowersForSaleCoeff;

            maxSpawnTime = customersSettings.MaxSpawnTime -
                           customersSettings.MaxSpawnTimeDelta * flowersForSaleCoeffCalculator.CurrentFlowersForSaleCoeff;

            currentSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        }

        private void TryToSpawnCustomer()
        {
            if (customersMoving.Count > 0)
            {
                Transform startCustomerTransform = spawnPathPoints[Random.Range(0, spawnPathPoints.Count)];
                
                CustomerMoving customerMoving = customersMoving[Random.Range(0, customersMoving.Count)];
                customerMoving.SpawnCustomer(startCustomerTransform, enterShopPathPoints);
            }
        }
    }
}
