using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace FlowerShop.FlowerSales
{
    public class FlowerSaleTablesForCustomers : MonoBehaviour
    {
        private readonly List<FlowerSaleTable> saleTables = new();

        public void AddSaleTableWithFlower(FlowerSaleTable table)
        {
            saleTables.Add(table);
        }

        public FlowerSaleTable GetSaleTableWithFlower()
        {
            if (saleTables.Count > 0)
            {
                int flowerSaleTablesIndex = Random.Range(0, saleTables.Count);
                FlowerSaleTable flowerSaleTable = saleTables[flowerSaleTablesIndex];
                RemoveSaleTable(flowerSaleTable);
                return flowerSaleTable;
            }
            else 
            { 
                return null; 
            }
        }

        private void RemoveSaleTable(FlowerSaleTable table)
        {
            Assert.That(saleTables.Contains(table));
        
            saleTables.Remove(table);
        }
    }
}
