using System.Collections.Generic;
using FlowerShop.Environment;
using UnityEngine;
using Zenject;

namespace FlowerShop.Customers
{
    public class CustomersObserver : MonoBehaviour
    {
        [Inject] private readonly AutomaticDoors automaticDoors;
        [Inject] private readonly EnvironmentSettings environmentSettings;
        
        [SerializeField] private Transform automaticDoorsCenter;
        
        private int customersNearDoorsCount;
        
        private readonly List<CustomerMoving> activeCustomers = new();

        private void Update()
        {
            customersNearDoorsCount = 0;
            
            if (activeCustomers.Count > 0)
            {
                foreach (CustomerMoving activeCustomer in activeCustomers)
                {
                    float distanceToAutomaticDoors =
                        Vector3.Distance(automaticDoorsCenter.position, activeCustomer.transform.position);
                    
                    if (distanceToAutomaticDoors < environmentSettings.MinDistanceToOpenAutomaticDoors)
                    {
                        customersNearDoorsCount++;
                    }
                }
            }

            if (customersNearDoorsCount != 0 && !automaticDoors.IsOpen)
            {
                automaticDoors.Open();
            }
            else if (customersNearDoorsCount == 0 && automaticDoors.IsOpen)
            {
                automaticDoors.Close();
            }
        }

        public void AddActiveCustomer(CustomerMoving customerMoving)
        {
            activeCustomers.Add(customerMoving);
        }

        public void RemoveInactiveCustomer(CustomerMoving customerMoving)
        {
            activeCustomers.Remove(customerMoving);
        }
    }
}