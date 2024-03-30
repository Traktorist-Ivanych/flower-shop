using System.Collections.Generic;
using FlowerShop.Flowers;
using FlowerShop.Tables;
using UnityEngine;
using Zenject;

namespace FlowerShop.FlowersSale
{
    /// <summary>
    /// Calculates coefficient, depending on number of unique flowers and total number of flowers, offered for sale
    /// </summary>
    public class FlowersForSaleCoeffCalculator : MonoBehaviour
    {
        [Inject] private readonly FlowersForSaleCoeffCalculatorSettings flowersForSaleCoeffCalculatorSettings;

        private readonly List<FlowersSaleTable> saleTablesWithFlowers = new();
        private readonly HashSet<FlowerInfo> uniqueFlowersForSale = new();

        public float CurrentFlowersForSaleCoeff { get; private set; }

        public void AddSaleTableWithFLowerInList(FlowersSaleTable flowersSaleTableWith)
        {
            saleTablesWithFlowers.Add(flowersSaleTableWith);
            uniqueFlowersForSale.Add(flowersSaleTableWith.FlowerInfoForSale);

            CalculateFlowersForSaleCoeff();
        }

        public void RemoveFlowerSaleTableWithoutFlowerFromList(FlowersSaleTable flowersSaleTableWithout)
        {
            if (saleTablesWithFlowers.Contains(flowersSaleTableWithout))
            {
                saleTablesWithFlowers.Remove(flowersSaleTableWithout);

                uniqueFlowersForSale.Clear();
                foreach(FlowersSaleTable flowerSaleTable in saleTablesWithFlowers)
                {
                    uniqueFlowersForSale.Add(flowerSaleTable.FlowerInfoForSale);
                }

                CalculateFlowersForSaleCoeff();
            }
        }

        public int CalculateCurrentGrade()
        {
            int currentGrade = Mathf.RoundToInt(
                CurrentFlowersForSaleCoeff * flowersForSaleCoeffCalculatorSettings.MaxShopGrade / 
                flowersForSaleCoeffCalculatorSettings.SaleCoeffForMaxGrade);
            
            return Mathf.Clamp(
                value: currentGrade, 
                min: flowersForSaleCoeffCalculatorSettings.MinShopGrade, 
                max: flowersForSaleCoeffCalculatorSettings.MaxShopGrade);
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
