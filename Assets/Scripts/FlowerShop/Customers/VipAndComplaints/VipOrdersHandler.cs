﻿using FlowerShop.ComputerPages;
using FlowerShop.Flowers;
using FlowerShop.FlowersForCollection;
using FlowerShop.Saves.SaveData;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Customers.VipAndComplaints
{
    public class VipOrdersHandler : MonoBehaviour, ISavableObject
    {
        [Inject] private readonly CanvasIndicators canvasIndicators;
        [Inject] private readonly ComputerMainPageCanvasLiaison computerMainPageCanvasLiaison;
        [Inject] private readonly CustomersSettings customersSettings;
        [Inject] private readonly CyclicalSaver cyclicalSaver;
        [Inject] private readonly FlowersForPlayerCollection flowersForPlayerCollection;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly ReferencesForLoad referencesForLoad;
        [Inject] private readonly VipCanvasLiaison vipCanvasLiaison;

        private float currentVipOrderHandleTime;
        private float currentVipOrderTime;
        private int vipOrderDescriptionIndex;
        private string vipOrderFlowerInfoUniqueKey;
        
        [field: SerializeField] public string UniqueKey { get; private set; }
        [field: HideInInspector, SerializeField] public FlowerInfo VipOrderFlowerInfo { get; private set; }
        [field: HideInInspector, SerializeField] public bool IsVipOrderActive { get; private set; }

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
            if (flowersForPlayerCollection.FlowersInPlayerCollectionCount() >=
                customersSettings.MinFlowersInPlayerCollectionCount)
            {
                currentVipOrderTime -= Time.deltaTime;
            
                if (currentVipOrderTime <= 0)
                {
                    SetTimeToMakeVipOrder();

                    SetVipOrder();
                }
            }

            if (IsVipOrderActive)
            {
                currentVipOrderHandleTime += Time.deltaTime;
                UpdateVipOrderIndicator();

                if (currentVipOrderHandleTime >= customersSettings.ComplaintsHandleTime)
                {
                    FaultVipOrder();
                }
            }
        }

        public void CompleteVipOrder()
        {
            RemoveVipOrder();
            
            Save();
        }

        public void Save()
        {
            ComplaintsHandlerForSaving vipOrdersHandlerForSaving =
                new(currentVipOrderTime, currentVipOrderHandleTime, IsVipOrderActive, 
                    vipOrderDescriptionIndex, vipOrderFlowerInfoUniqueKey);
            
            SavesHandler.Save(UniqueKey, vipOrdersHandlerForSaving);
        }

        public void Load()
        {
            ComplaintsHandlerForSaving vipOrdersHandlerForLoading =
                SavesHandler.Load<ComplaintsHandlerForSaving>(UniqueKey);

            if (vipOrdersHandlerForLoading.IsValuesSaved)
            {
                currentVipOrderTime = vipOrdersHandlerForLoading.CurrentComplaintTime;

                if (vipOrdersHandlerForLoading.IsComplaintActive)
                {
                    currentVipOrderHandleTime = vipOrdersHandlerForLoading.CurrentComplaintHandleTime;
                    vipOrderDescriptionIndex = vipOrdersHandlerForLoading.ComplaintDescriptionIndex;
                    vipOrderFlowerInfoUniqueKey = vipOrdersHandlerForLoading.ComplaintFlowerInfoUniqueKey;
                    VipOrderFlowerInfo = 
                        referencesForLoad.GetReference<FlowerInfo>(vipOrderFlowerInfoUniqueKey);
                    
                    SetVipOrderMain();
                }
                else
                {
                    VipOrderFlowerInfo = flowersSettings.FlowerInfoEmpty;
                }
            }
            else
            {
                SetTimeToMakeVipOrder();
                VipOrderFlowerInfo = flowersSettings.FlowerInfoEmpty;
            }
        }

        private void SetVipOrder()
        {
            VipOrderFlowerInfo = flowersForPlayerCollection.GetRandomFlowerInfoFromPlayerCollection();
            vipOrderFlowerInfoUniqueKey = VipOrderFlowerInfo.UniqueKey;
            vipOrderDescriptionIndex = Random.Range(0, customersSettings.VipDescriptions.Length);

            SetVipOrderMain();
            
            Save();
        }

        private void SetVipOrderMain()
        {
            IsVipOrderActive = true;
            computerMainPageCanvasLiaison.VipButton.interactable = true;
            canvasIndicators.VipIndicatorImage.enabled = true;
            UpdateVipOrderIndicator();
            vipCanvasLiaison.SetComplaintFlowerInfo(VipOrderFlowerInfo, vipOrderDescriptionIndex);
        }

        private void FaultVipOrder()
        {
            RemoveVipOrder();
            
            Save();
        }

        private void RemoveVipOrder()
        {
            IsVipOrderActive = false;
            currentVipOrderHandleTime = 0;
            vipOrderDescriptionIndex = 0;
            VipOrderFlowerInfo = flowersSettings.FlowerInfoEmpty;
            computerMainPageCanvasLiaison.VipButton.interactable = false;
            canvasIndicators.VipIndicatorImage.enabled = false;
        }

        private void SetTimeToMakeVipOrder()
        {
            float minTime = customersSettings.MinVipTime -
                            customersSettings.MinVipTimeDelta *
                            flowersForPlayerCollection.FlowersInPlayerCollectionCount() /
                            flowersSettings.AllUniqueFlowersCount;
            
            float maxTime = customersSettings.MaxVipTime -
                            customersSettings.MaxVipTimeDelta *
                            flowersForPlayerCollection.FlowersInPlayerCollectionCount() /
                            flowersSettings.AllUniqueFlowersCount;
            
            currentVipOrderTime = Random.Range(minTime, maxTime);
        }

        private void UpdateVipOrderIndicator()
        {
            canvasIndicators.VipIndicatorImage.fillAmount = 
                (customersSettings.VipHandleTime - currentVipOrderHandleTime) / 
                customersSettings.VipHandleTime;
        }
    }
}