using System.Collections.Generic;
using FlowerShop.FlowersSale;
using FlowerShop.Saves.SaveData;
using FlowerShop.Tables;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Customers
{
    public class CustomersSpawner : MonoBehaviour, ISavableObject
    {
        [Inject] private readonly CyclicalSaver cyclicalSaver;
        [Inject] private readonly CustomersSettings customersSettings;
        [Inject] private readonly FlowersSaleTablesForCustomers flowersSaleTablesForCustomers;
        [Inject] private readonly FlowersForSaleCoeffCalculator flowersForSaleCoeffCalculator;

        [SerializeField] private Transform[] startTransforms;
        [SerializeField] private Transform[] endTransforms;

        private readonly List<CustomerMoving> customersMoving = new();
        private float minSpawnTime;
        private float maxSpawnTime;
        private float currentSpawnTime;

        [field: SerializeField] public string UniqueKey { get; private set; }

        private void Awake()
        {
            Load();
        }

        private void OnEnable()
        {
            cyclicalSaver.CyclicalSaverEvent += Save;
        }

        private void Start()
        {
            TryToSpawnCustomer();
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

        public Transform GetEndTransform()
        {
            return endTransforms[Random.Range(0, endTransforms.Length)];
        }

        public void RemoveBuyerMoving(CustomerMoving customerMoving)
        {
            if (customersMoving.Contains(customerMoving))
            {
                customersMoving.Remove(customerMoving);
            }
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
                FlowersSaleTable flowersSaleTableForBuyer = flowersSaleTablesForCustomers.GetSaleTableWithFlower();
                    
                if (flowersSaleTableForBuyer)
                {
                    CustomerMoving customerMoving = customersMoving[Random.Range(0, customersMoving.Count)];

                    Transform startCustomerTransform = startTransforms[Random.Range(0, startTransforms.Length)];

                    customerMoving.SetBuyerStartDestination(startCustomerTransform, flowersSaleTableForBuyer);
                }
            }
        }
    }
}
