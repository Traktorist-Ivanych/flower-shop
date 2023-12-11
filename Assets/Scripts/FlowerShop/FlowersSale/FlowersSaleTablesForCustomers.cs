using System.Collections.Generic;
using FlowerShop.Tables;
using NUnit.Framework;
using UnityEngine;

namespace FlowerShop.FlowerSales
{
    public class FlowersSaleTablesForCustomers : MonoBehaviour
    {
        private readonly List<FlowersSaleTable> saleTables = new();

        public void AddSaleTableWithFlower(FlowersSaleTable table)
        {
            saleTables.Add(table);
        }

        public FlowersSaleTable GetSaleTableWithFlower()
        {
            if (saleTables.Count > 0)
            {
                int flowerSaleTablesIndex = Random.Range(0, saleTables.Count);
                FlowersSaleTable flowersSaleTable = saleTables[flowerSaleTablesIndex];
                RemoveSaleTable(flowersSaleTable);
                return flowersSaleTable;
            }
            else 
            { 
                return null; 
            }
        }

        private void RemoveSaleTable(FlowersSaleTable table)
        {
            Assert.That(saleTables.Contains(table));
        
            saleTables.Remove(table);
        }
    }
}
