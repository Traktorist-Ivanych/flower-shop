using System.Collections.Generic;
using FlowerShop.Education;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Tables;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Weeds
{
    public class WeedPlanter : MonoBehaviour, ISavableObject
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly CyclicalSaver cyclicalSaver;
        [Inject] private readonly WeedSettings weedSettings;

        [SerializeField] private SoilPreparationTable soilPreparationTable;

        private readonly List<Pot> potsForPlantingWeed = new();
        private float currentWeedPlantTime;

        [field: SerializeField] public string UniqueKey { get; private set; }

        private void Awake()
        {
            Load();
        }

        private void OnEnable()
        {
            cyclicalSaver.CyclicalSaverEvent += Save;
        }

        private void OnDisable()
        {
            cyclicalSaver.CyclicalSaverEvent -= Save;
        }

        private void Update()
        {
            if (currentWeedPlantTime > 0)
            {
                currentWeedPlantTime -= Time.deltaTime;
            }
            else
            {
                SetCurrentWeedPlantTime();
            
                if (!educationHandler.IsEducationActive && potsForPlantingWeed.Count > 0 &&
                    weedSettings.ShouldWeedBePlanting())
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

        public void Load()
        {
            FloatProgressForSaving floatProgressForLoading = SavesHandler.Load<FloatProgressForSaving>(UniqueKey);

            if (floatProgressForLoading.IsValuesSaved)
            {
                currentWeedPlantTime = floatProgressForLoading.Progress;
            }
            else
            {
                SetCurrentWeedPlantTime();
            }
        }

        public void Save()
        {
            FloatProgressForSaving floatProgressForSaving = new(currentWeedPlantTime);
            
            SavesHandler.Save(UniqueKey, floatProgressForSaving);
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
