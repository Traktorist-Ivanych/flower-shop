using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using Zenject;

namespace FlowerShop.Customers
{
    public class CustomersSpawner : MonoBehaviour
    {
        [Inject] private readonly CustomersSettings customersSettings;
        [Inject] private readonly FlowerSaleTablesForCustomers flowerSaleTablesForCustomers;
        [Inject] private readonly FlowersForSaleCoeffCalculator flowersForSaleCoeffCalculator;

        [SerializeField] private Transform[] startTransforms;
        [SerializeField] private Transform[] endTransforms;

        private readonly List<CustomerMoving> customersMoving = new();
        private float minSpawnTime;
        private float maxSpawnTime;
        private float currentSpawnTime;

        private void Update()
        {
            if (currentSpawnTime <= 0)
            {
                minSpawnTime = customersSettings.MinSpawnTime -
                               customersSettings.MinSpawnTimeDelta * flowersForSaleCoeffCalculator.CurrentFlowersForSaleCoef;

                maxSpawnTime = customersSettings.MaxSpawnTime -
                               customersSettings.MaxSpawnTimeDelta * flowersForSaleCoeffCalculator.CurrentFlowersForSaleCoef;

                currentSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
                
                TryToSpawnCustomer();
            }
            else
            {
                currentSpawnTime -= Time.deltaTime;
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
            Assert.That(customersMoving.Contains(customerMoving));
            
            customersMoving.Remove(customerMoving);
        }

        private void TryToSpawnCustomer()
        {
            if (customersMoving.Count > 0)
            {
                FlowerSaleTable flowerSaleTableForBuyer = flowerSaleTablesForCustomers.GetSaleTableWithFlower();
                    
                if (flowerSaleTableForBuyer != null)
                {
                    CustomerMoving customerMoving = customersMoving[Random.Range(0, customersMoving.Count)];

                    Transform startCustomerTransform = startTransforms[Random.Range(0, startTransforms.Length)];

                    customerMoving.SetBuyerStartDestination(startCustomerTransform, flowerSaleTableForBuyer);
                }
            }
        }
    }
}
