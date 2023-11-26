using System.Collections.Generic;
using UnityEngine;

// THINK about Create FlowerSaleTablesCollection class which contains all flowerSaleTables and contains method for getting TablesForByers (where...)
// and unite with FlowersForSaleCoefCalculator
public class FlowerSaleTablesForByers : MonoBehaviour
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

    public void RemoveSaleTable(FlowerSaleTable table)
    {
        if (saleTables.Contains(table))
        {
            saleTables.Remove(table);
        }
        else
        {
            Debug.Log("Removing FlowerSaleTable does not exist in List<FlowerSaleTable>");
        }
    }
}
