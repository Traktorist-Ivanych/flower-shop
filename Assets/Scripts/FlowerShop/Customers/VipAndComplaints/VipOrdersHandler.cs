using FlowerShop.Achievements;
using FlowerShop.ComputerPages;
using FlowerShop.Flowers;
using FlowerShop.FlowersForCollection;
using FlowerShop.FlowersSale;
using FlowerShop.Saves.SaveData;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Customers.VipAndComplaints
{
    public class VipOrdersHandler : MonoBehaviour, ISavableObject
    {
        [Inject] private readonly CanvasIndicators canvasIndicators;
        [Inject] private readonly CustomersSettings customersSettings;
        [Inject] private readonly CyclicalSaver cyclicalSaver;
        [Inject] private readonly FlowersForPlayerCollection flowersForPlayerCollection;
        [Inject] private readonly FlowersForSaleCoeffCalculatorSettings flowersForSaleCoeffCalculatorSettings;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly KnowALotAboutBusiness knowALotAboutBusiness;
        [Inject] private readonly ReferencesForLoad referencesForLoad;
        [Inject] private readonly ShopRating shopRating;
        [Inject] private readonly VipCanvasLiaison vipCanvasLiaison;

        private float currentVipOrderHandleTime;
        private float vipOrderHandleTime;
        private float currentVipOrderTime;
        private int vipOrderDescriptionIndex;
        private string vipOrderFlowerInfoUniqueKey;
        private bool isCurrentVipOrderEducation;
        
        public float CurrentVipOrderPriceMultipler {  get; private set; }
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

        private void Start()
        {
            CalculateCurrentVipOrderPriceMultipler();
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
                if (isCurrentVipOrderEducation && currentVipOrderHandleTime >= vipOrderHandleTime - 5)
                {
                    currentVipOrderHandleTime = vipOrderHandleTime - 5;
                }

                UpdateVipOrderIndicator();

                if (currentVipOrderHandleTime >= vipOrderHandleTime)
                {
                    FaultVipOrder();
                }
            }
        }

        public void CalculateCurrentVipOrderPriceMultipler()
        {
            if (shopRating.CurrentAverageGrade >= customersSettings.MaxPriceMultiplerGradesBorder &&
                shopRating.CurrentGradesCount >= customersSettings.MinGradesCountForMaxPriceMultipler)
            {
                CurrentVipOrderPriceMultipler = customersSettings.MaxPriceMultipler;
            }
            else if (shopRating.CurrentAverageGrade >= customersSettings.MiddlePriceMultiplerGradesBorder)
            {
                CurrentVipOrderPriceMultipler = customersSettings.MiddlePriceMultipler;
            }
            else
            {
                CurrentVipOrderPriceMultipler = customersSettings.MinPriceMultipler;
            }
        }

        public void SetEducationVipOrder(FlowerInfo vipOrderFlowerInfo)
        {
            VipOrderFlowerInfo = vipOrderFlowerInfo;
            isCurrentVipOrderEducation = true;
            vipOrderFlowerInfoUniqueKey = VipOrderFlowerInfo.UniqueKey;
            vipOrderDescriptionIndex = Random.Range(1, customersSettings.LocalizedVipDescriptions.Length);

            SetVipOrderMain();

            Save();
        }

        public void CompleteVipOrder()
        {
            RemoveVipOrder();
            knowALotAboutBusiness.IncreaseProgress();
            isCurrentVipOrderEducation = false;

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
                    VipOrderFlowerInfo = customersSettings.InactiveOrder;
                    vipCanvasLiaison.SetVipFlowerInfo(VipOrderFlowerInfo, vipOrderDescriptionIndex);
                }
            }
            else
            {
                SetTimeToMakeVipOrder();
                VipOrderFlowerInfo = customersSettings.InactiveOrder;
                vipCanvasLiaison.SetVipFlowerInfo(VipOrderFlowerInfo, vipOrderDescriptionIndex);
            }
        }

        private void SetVipOrder()
        {
            VipOrderFlowerInfo = flowersForPlayerCollection.GetRandomFlowerInfoFromPlayerCollection();
            vipOrderFlowerInfoUniqueKey = VipOrderFlowerInfo.UniqueKey;
            vipOrderDescriptionIndex = Random.Range(1, customersSettings.LocalizedVipDescriptions.Length);

            SetVipOrderMain();
            
            Save();
        }

        private void SetVipOrderMain()
        {
            IsVipOrderActive = true;
            canvasIndicators.ShowVipIndicator();
            UpdateVipOrderIndicator();
            vipCanvasLiaison.SetVipFlowerInfo(VipOrderFlowerInfo, vipOrderDescriptionIndex);
            vipCanvasLiaison.ShowIndicator();
            vipOrderHandleTime = customersSettings.CompletingOrderTimeMain +
                (customersSettings.CompletingOrderTimeForFlowerLvl * (VipOrderFlowerInfo.FlowerLvl - 1));
        }

        private void FaultVipOrder()
        {
            RemoveVipOrder();
            shopRating.AddGrade(flowersForSaleCoeffCalculatorSettings.MinShopGrade);
            
            Save();
        }

        private void RemoveVipOrder()
        {
            IsVipOrderActive = false;
            currentVipOrderHandleTime = 0;
            vipOrderDescriptionIndex = 0;
            VipOrderFlowerInfo = customersSettings.InactiveOrder;
            vipCanvasLiaison.SetVipFlowerInfo(VipOrderFlowerInfo, vipOrderDescriptionIndex);
            canvasIndicators.HideVipIndicator();
            vipCanvasLiaison.HideIndicator();
        }

        private void SetTimeToMakeVipOrder()
        {
            float currentCoeff = (float)shopRating.CurrentAverageGrade / 
                                 flowersForSaleCoeffCalculatorSettings.MaxShopGrade *
                                 customersSettings.AverageGradeInfluence
                                 +
                                 flowersForPlayerCollection.FlowersInPlayerCollectionCount() /
                                 flowersSettings.AllUniqueFlowersCount *
                                 customersSettings.FlowersInPlayerCollectionInfluence;
            
            float minTime = customersSettings.MinVipTime - customersSettings.MinVipTimeDelta * currentCoeff;
            float maxTime = customersSettings.MaxVipTime - customersSettings.MaxVipTimeDelta * currentCoeff;
            
            currentVipOrderTime = Random.Range(minTime, maxTime);
        }

        private void UpdateVipOrderIndicator()
        {
            canvasIndicators.VipIndicatorImage.fillAmount = currentVipOrderHandleTime / vipOrderHandleTime;
        }

        [ContextMenu("Set currentVipOrderTime to 10 s")]
        public void DeleteAllPlayerPrefsKeys()
        {
            currentVipOrderTime = 10;
        }
    }
}