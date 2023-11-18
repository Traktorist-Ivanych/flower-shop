using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class FlowersForSaleCoefCalculator : MonoBehaviour
{
    [Inject] private readonly GameConfiguration gameConfiguration;

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
        float allAndUniqueFlowersForSale = gameConfiguration.AllFlowersForSale + gameConfiguration.UniqueFlowersForSale;

        float allFlowersForSaleCoef = 
            Mathf.Min(saleTablesWithFlowers.Count, gameConfiguration.AllFlowersForSale) / allAndUniqueFlowersForSale;

        float uniqueFlowersForSaleCoef = 
            Mathf.Min(uniqueFlowersForSale.Count, gameConfiguration.UniqueFlowersForSale) / allAndUniqueFlowersForSale;

        currentFlowersForSaleCoef = allFlowersForSaleCoef + uniqueFlowersForSaleCoef;
    }
}
