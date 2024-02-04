using System.Collections.Generic;
using FlowerShop.Tables;
using UnityEngine;

namespace FlowerShop.FlowersSale
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

            return null;
        }

        private void RemoveSaleTable(FlowersSaleTable table)
        {
            if (saleTables.Contains(table))
            {
                saleTables.Remove(table);
            }
        }
    }
}
