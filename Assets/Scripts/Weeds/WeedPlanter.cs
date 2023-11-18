using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WeedPlanter : MonoBehaviour
{
    [Inject] private readonly GameConfiguration gameConfiguration;

    [SerializeField] private SoilPreparationTable soilPreparationTable;

    private readonly List<Pot> potsForPlantingWeed = new();
    private float currentWeedPlantTime;

    private void Start()
    {
        SetCurrentWeedPlantTime();
    }

    private void Update()
    {
        if (currentWeedPlantTime >= 0)
        {
            currentWeedPlantTime -= Time.deltaTime;
        }
        else
        {
            SetCurrentWeedPlantTime();
            
            if (potsForPlantingWeed.Count > 0 && gameConfiguration.IsWeedPlanting())
            {
                Pot potForPlantingWeed = potsForPlantingWeed[Random.Range(0, potsForPlantingWeed.Count)];
                potForPlantingWeed.PlantWeed();
                RemovePotFormPlantingWeedList(potForPlantingWeed);
            }
        }
    }

    public void AddPotInPlantingWeedList(Pot potForPlantingWeed)
    {
        potsForPlantingWeed.Add(potForPlantingWeed);
    }

    public void RemovePotFormPlantingWeedList(Pot potForRemoving)
    {
        if (potsForPlantingWeed.Contains(potForRemoving))
        {
            potsForPlantingWeed.Remove(potForRemoving);
        }
    }

    private void SetCurrentWeedPlantTime()
    {
        currentWeedPlantTime = Random.Range(gameConfiguration.MinWeedPlantTime * (soilPreparationTable.TableLvl + 1), 
                                            gameConfiguration.MaxWeedPlantTime * (soilPreparationTable.TableLvl + 1));
    }
}
