using System.Collections.Generic;
using Buyers;
using ModestTree;
using UnityEngine;
using Zenject;

public class BuyersSpawner : MonoBehaviour
{
    [Inject] private readonly GameConfiguration gameConfiguration;
    [Inject] private readonly FlowerSaleTablesForByers flowerSaleTablesForByers;
    [Inject] private readonly FlowersForSaleCoefCalculator flowersForSaleCoefCalculator;

    [SerializeField] private Transform[] startTransforms;
    [SerializeField] private Transform[] endTransforms;

    private readonly List<BuyerMoving> buyersMoving = new();
    private float minSpawnTime;
    private float maxSpawnTime;
    private float currentSpawnTime;

    private void Update()
    {
        if (currentSpawnTime <= 0)
        {
            minSpawnTime = gameConfiguration.MinBuyerSpawnTime -
                gameConfiguration.MinBuyerSpawnTimeDelta * flowersForSaleCoefCalculator.CurrentFlowersForSaleCoef;

            maxSpawnTime = gameConfiguration.MaxBuyerSpawnTime -
                gameConfiguration.MaxBuyerSpawnTimeDelta * flowersForSaleCoefCalculator.CurrentFlowersForSaleCoef;

            currentSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            if (buyersMoving.Count > 0)
            {
                FlowerSaleTable flowerSaleTableForBuyer = flowerSaleTablesForByers.GetSaleTableWithFlower();
                if (flowerSaleTableForBuyer != null)
                {
                    BuyerMoving buyerMoving = buyersMoving[Random.Range(0, buyersMoving.Count)];

                    Transform startBuyerTransform = startTransforms[Random.Range(0, startTransforms.Length)];

                    buyerMoving.SetBuyerStartDestination(startBuyerTransform, flowerSaleTableForBuyer);
                }
            }
        }
        else
        {
            currentSpawnTime -= Time.deltaTime;
        }
    }

    public void AddBuyerMoving(BuyerMoving buyerMoving)
    {
        buyersMoving.Add(buyerMoving);
    }

    public Transform GetEndTransform()
    {
        return endTransforms[Random.Range(0, endTransforms.Length)];
    }

    public void RemoveBuyerMoving(BuyerMoving buyerMoving)
    {
        //Assert.That(buyersMoving.Contains(buyerMoving));
        if (buyersMoving.Contains(buyerMoving))
        {
            buyersMoving.Remove(buyerMoving);
        }
        else
        {
            Debug.Log("Removing BuyerMoving does not exist in List<BuyerMoving>");
        }
    }
}
