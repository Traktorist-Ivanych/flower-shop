using System.Collections.Generic;
using FlowerShop.Flowers;
using UnityEngine;
using Zenject;

namespace FlowerShop.FlowerSales
{
    /// <summary>
    /// Calculates coefficient, depending on number of unique flowers and total number of flowers, offered for sale
    /// </summary>
    public class FlowersForSaleCoeffCalculator : MonoBehaviour
    {
        [Inject] private readonly FlowersForSaleCoeffCalculatorSettings flowersForSaleCoeffCalculatorSettings;

        private readonly List<FlowerSaleTable> saleTablesWithFlowers = new();
        private readonly HashSet<FlowerInfo> uniqueFlowersForSale = new();

        public float CurrentFlowersForSaleCoeff { get; private set; }

        public void AddFlowerSaleTableWithFLowerInList(FlowerSaleTable saleTableWithFlower)
        {
            saleTablesWithFlowers.Add(saleTableWithFlower);
            uniqueFlowersForSale.Add(saleTableWithFlower.FlowerInfoForSale);

            CalculateFlowersForSaleCoeff();
        }

        public void RemoveFlowerSaleTableWithoutFlowerFromList(FlowerSaleTable saleTableWithoutFlower)
        {
            if (saleTablesWithFlowers.Contains(saleTableWithoutFlower))
            {
                saleTablesWithFlowers.Remove(saleTableWithoutFlower);

                uniqueFlowersForSale.Clear();
                foreach(FlowerSaleTable flowerSaleTable in saleTablesWithFlowers)
                {
                    uniqueFlowersForSale.Add(flowerSaleTable.FlowerInfoForSale);
                }

                CalculateFlowersForSaleCoeff();
            }
        }

        private void CalculateFlowersForSaleCoeff()
        {
            float allAndUniqueFlowersForSale = 
                flowersForSaleCoeffCalculatorSettings.AllFlowersForSale + 
                flowersForSaleCoeffCalculatorSettings.UniqueFlowersForSale;

            float allFlowersForSaleCoeff = 
                Mathf.Min(saleTablesWithFlowers.Count, flowersForSaleCoeffCalculatorSettings.AllFlowersForSale) / 
                allAndUniqueFlowersForSale;

            float uniqueFlowersForSaleCoeff = 
                Mathf.Min(uniqueFlowersForSale.Count, flowersForSaleCoeffCalculatorSettings.UniqueFlowersForSale) / 
                allAndUniqueFlowersForSale;

            CurrentFlowersForSaleCoeff = allFlowersForSaleCoeff + uniqueFlowersForSaleCoeff;
        }
    }
}
