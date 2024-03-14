using FlowerShop.ComputerPages;
using FlowerShop.Flowers;
using FlowerShop.FlowersSale;
using FlowerShop.Saves.SaveData;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Customers.VipAndComplaints
{
    public class ComplaintsHandler : MonoBehaviour, ISavableObject
    {
        [Inject] private readonly CanvasIndicators canvasIndicators;
        [Inject] private readonly ComplaintsCanvasLiaison complaintsCanvasLiaison;
        [Inject] private readonly ComputerMainPageCanvasLiaison computerMainPageCanvasLiaison;
        [Inject] private readonly CustomersSettings customersSettings;
        [Inject] private readonly CyclicalSaver cyclicalSaver;
        [Inject] private readonly FlowersContainer flowersContainer;
        [Inject] private readonly FlowersForSaleCoeffCalculatorSettings flowersForSaleCoeffCalculatorSettings;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly ReferencesForLoad referencesForLoad;
        [Inject] private readonly ShopRating shopRating;

        private float currentComplaintHandleTime;
        private float currentComplaintTime;
        private int complaintDescriptionIndex;
        private string complaintFlowerInfoUniqueKey;
        
        [field: SerializeField] public string UniqueKey { get; private set; }
        [field: HideInInspector, SerializeField] public FlowerInfo ComplaintFlowerInfo { get; private set; }
        [field: HideInInspector, SerializeField] public bool IsComplaintActive { get; private set; }

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
            currentComplaintTime -= Time.deltaTime;
            
            if (currentComplaintTime <= 0)
            {
                SetTimeToMakeComplaint();

                SetComplaint();
            }

            if (IsComplaintActive)
            {
                currentComplaintHandleTime += Time.deltaTime;
                UpdateComplaintIndicator();

                if (currentComplaintHandleTime >= customersSettings.ComplaintsHandleTime)
                {
                    FaultComplaint();
                }
            }
        }

        public void CompleteComplaint()
        {
            RemoveComplaint();
            shopRating.AddGrade(flowersForSaleCoeffCalculatorSettings.MaxShopGrade);
            
            Save();
        }

        public void Save()
        {
            ComplaintsHandlerForSaving complaintsHandlerForSaving =
                new(currentComplaintTime, currentComplaintHandleTime, IsComplaintActive, 
                    complaintDescriptionIndex, complaintFlowerInfoUniqueKey);
            
            SavesHandler.Save(UniqueKey, complaintsHandlerForSaving);
        }

        public void Load()
        {
            ComplaintsHandlerForSaving complaintsHandlerForLoading =
                SavesHandler.Load<ComplaintsHandlerForSaving>(UniqueKey);

            if (complaintsHandlerForLoading.IsValuesSaved)
            {
                currentComplaintTime = complaintsHandlerForLoading.CurrentComplaintTime;

                if (complaintsHandlerForLoading.IsComplaintActive)
                {
                    currentComplaintHandleTime = complaintsHandlerForLoading.CurrentComplaintHandleTime;
                    complaintDescriptionIndex = complaintsHandlerForLoading.ComplaintDescriptionIndex;
                    complaintFlowerInfoUniqueKey = complaintsHandlerForLoading.ComplaintFlowerInfoUniqueKey;
                    ComplaintFlowerInfo = 
                        referencesForLoad.GetReference<FlowerInfo>(complaintFlowerInfoUniqueKey);
                    
                    SetComplaintMain();
                }
                else
                {
                    ComplaintFlowerInfo = flowersSettings.FlowerInfoEmpty;
                }
            }
            else
            {
                SetTimeToMakeComplaint();
                ComplaintFlowerInfo = flowersSettings.FlowerInfoEmpty;
            }
        }

        private void SetComplaint()
        {
            ComplaintFlowerInfo = flowersContainer.GetRandomAvailableFlowerInfo();
            complaintFlowerInfoUniqueKey = ComplaintFlowerInfo.UniqueKey;
            complaintDescriptionIndex = Random.Range(0, customersSettings.ComplaintDescriptions.Length);

            SetComplaintMain();
            
            Save();
        }

        private void SetComplaintMain()
        {
            IsComplaintActive = true;
            computerMainPageCanvasLiaison.ComplaintsButton.interactable = true;
            canvasIndicators.ComplaintIndicatorImage.enabled = true;
            UpdateComplaintIndicator();
            complaintsCanvasLiaison.SetComplaintFlowerInfo(ComplaintFlowerInfo, complaintDescriptionIndex);
        }

        private void FaultComplaint()
        {
            RemoveComplaint();
            shopRating.AddGrade(flowersForSaleCoeffCalculatorSettings.MinShopGrade);
            
            Save();
        }

        private void RemoveComplaint()
        {
            IsComplaintActive = false;
            currentComplaintHandleTime = 0;
            complaintDescriptionIndex = 0;
            ComplaintFlowerInfo = flowersSettings.FlowerInfoEmpty;
            computerMainPageCanvasLiaison.ComplaintsButton.interactable = false;
            canvasIndicators.ComplaintIndicatorImage.enabled = false;
        }

        private void SetTimeToMakeComplaint()
        {
            currentComplaintTime =
                Random.Range(customersSettings.MinComplaintsTime, customersSettings.MaxComplaintsTime);
        }

        private void UpdateComplaintIndicator()
        {
            canvasIndicators.ComplaintIndicatorImage.fillAmount = 
                (customersSettings.ComplaintsHandleTime - currentComplaintHandleTime) / 
                customersSettings.ComplaintsHandleTime;
        }
    }
}