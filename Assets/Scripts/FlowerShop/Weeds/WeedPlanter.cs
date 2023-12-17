using System.Collections.Generic;
using FlowerShop.PickableObjects;
using FlowerShop.Tables;
using UnityEngine;
using Zenject;

namespace FlowerShop.Weeds
{
    public class WeedPlanter : MonoBehaviour
    {
        [Inject] private readonly WeedSettings weedSettings;

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
            
                if (potsForPlantingWeed.Count > 0 && weedSettings.ShouldWeedBePlanting())
                {
                    Pot potForPlantingWeed = potsForPlantingWeed[Random.Range(0, potsForPlantingWeed.Count)];
                    potForPlantingWeed.PlantWeed();
                    RemovePotFormPlantingWeedList(potForPlantingWeed);
                }
            }
        }

        public void AddPotInPlantingWeedList(Pot potForPlantingWeed)
        {
            if (!potsForPlantingWeed.Contains(potForPlantingWeed))
            {
                potsForPlantingWeed.Add(potForPlantingWeed);
            }
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
            float minWeedPlantTime = weedSettings.MinWeedPlantTime +
                                     weedSettings.MinWeedPlantTimeLvlDelta * soilPreparationTable.TableLvl;
            
            float maxWeedPlantTime = weedSettings.MaxWeedPlantTime +
                                     weedSettings.MaxWeedPlantTimeLvlDelta * soilPreparationTable.TableLvl;
            
            currentWeedPlantTime = Random.Range(minWeedPlantTime, maxWeedPlantTime);
        }
    }
}
