using System.Collections.Generic;
using UnityEngine;
public class FlowersForSaleCoefCalculator : MonoBehaviour
{
    private readonly List<FlowerSaleTable> saleTablesWithFlowers = new();
    private readonly HashSet<Flower> uniqueFlowersForSale = new();
    private float currentFlowersForSaleCoef;

    public float CurrentFlowersForSaleCoef
    {
        get => currentFlowersForSaleCoef;
    }

    public void AddFlowerSaleTableWithFLowerInList(FlowerSaleTable saleTableWithFlower)
    {
        saleTablesWithFlowers.Add(saleTableWithFlower);
        uniqueFlowersForSale.Add(saleTableWithFlower.FlowerForSale);

        CalculateFlowersForSaleCoef();
    }

    public void RemoveFlowerSaleTableWithoutFLowerFromList(FlowerSaleTable saleTableWithoutFlower)
    {
        if (saleTablesWithFlowers.Contains(saleTableWithoutFlower))
        {
            saleTablesWithFlowers.Remove(saleTableWithoutFlower);

            uniqueFlowersForSale.Clear();
            foreach(FlowerSaleTable flowerSaleTable in saleTablesWithFlowers)
            {
                uniqueFlowersForSale.Add(flowerSaleTable.FlowerForSale);
            }

            CalculateFlowersForSaleCoef();
        }
    }

    private void CalculateFlowersForSaleCoef()
    {
        float allFlowersForSaleCoef = Mathf.Min(saleTablesWithFlowers.Count, 10f) / 15;
        float uniqueFlowersForSaleCoef = Mathf.Min(uniqueFlowersForSale.Count, 5f) / 15;

        currentFlowersForSaleCoef = allFlowersForSaleCoef + uniqueFlowersForSaleCoef;
    }
}
