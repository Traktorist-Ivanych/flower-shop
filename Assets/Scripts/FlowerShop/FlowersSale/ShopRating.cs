using FlowerShop.Achievements;
using FlowerShop.ComputerPages;
using FlowerShop.Customers.VipAndComplaints;
using FlowerShop.Flowers;
using FlowerShop.Saves.SaveData;
using PlayerControl;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.FlowersSale
{
    public class ShopRating : MonoBehaviour, ISavableObject
    {
        [Inject] private readonly Burnout burnout;
        [Inject] private readonly FlowersForSaleCoeffCalculatorSettings flowersForSaleCoeffCalculatorSettings;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly PlayerStatsCanvasLiaison playerStatsCanvasLiaison;
        [Inject] private readonly StatsEffects statsEffects;
        [Inject] private readonly StatsCanvasLiaison statsCanvasLiaison;
        [Inject] private readonly TheBestFlowerShop theBestFlowerShop;
        [Inject] private readonly VipOrdersHandler vipOrdersHandler;

        private int[] shopGrades;
        private int currentGradeIndex;
        
        private int gradesCount;
        private int fiveStars;
        private int fourStars;
        private int treeStars;
        private int twoStars;
        private int oneStar;

        public int CurrentGradesCount { get; private set; }
        public float CurrentAverageGrade { get; private set; }
        public float CurrentCustomersSpawnCoeff { get; private set; }
        [field: SerializeField] public string UniqueKey { get; private set; }

        private void Awake()
        {
            Load();
        }

        public void AddGrade(int currentGrade)
        {
            if (currentGradeIndex >= flowersSettings.MaxGradesCount)
            {
                currentGradeIndex = 0;
            }
            shopGrades[currentGradeIndex] = currentGrade;
            currentGradeIndex++;

            CalculateCurrentAverageGrade();

            if (currentGrade == 5)
            {
                statsEffects.RatingChangeEffect.DisplayEffect(Color.green, "+" + currentGrade.ToString());
            }
            else
            {
                statsEffects.RatingChangeEffect.DisplayEffect(Color.red, "+" + currentGrade.ToString());
            }

            if (currentGrade == flowersForSaleCoeffCalculatorSettings.MinShopGrade)
            {
                burnout.IncreaseProgress();
            }

            vipOrdersHandler.CalculateCurrentVipOrderPriceMultipler();

            Save();
        }

        public void Load()
        {
            ShopRatingForSaving shopRatingForSaving = SavesHandler.Load<ShopRatingForSaving>(UniqueKey);

            if (shopRatingForSaving.IsValuesSaved)
            {
                shopGrades = shopRatingForSaving.ShopGrades; 
                currentGradeIndex = shopRatingForSaving.CurrentGradeIndex; 
                CalculateCurrentAverageGrade();
            }
            else
            {
                shopGrades = new int[flowersSettings.MaxGradesCount];
                CurrentAverageGrade = 0;
                UpdateGradeRatingOnCanvas(
                    "0" + flowersForSaleCoeffCalculatorSettings.FractionalSeparationSign.GetLocalizedString() + "0");
                statsCanvasLiaison.UpdateStatsCanvas(gradesCount, fiveStars, fourStars, treeStars, twoStars, oneStar);
            }
        }

        public void Save()
        {
            ShopRatingForSaving shopRatingForSaving = new(shopGrades, currentGradeIndex);
            
            SavesHandler.Save(UniqueKey, shopRatingForSaving);
        }

        private void CalculateCurrentAverageGrade()
        {
            int gradesSumForCurrentAverage = 0;
            gradesCount = 0;
            fiveStars = 0;
            fourStars = 0;
            treeStars = 0;
            twoStars = 0;
            oneStar = 0;

            foreach (int shopGrade in shopGrades)
            {
                if (shopGrade != 0)
                {
                    gradesSumForCurrentAverage += shopGrade;
                    gradesCount++;

                    switch (shopGrade)
                    {
                        case 5:
                            fiveStars++;
                            break;
                        case 4:
                            fourStars++;
                            break;
                        case 3:
                            treeStars++;
                            break;
                        case 2:
                            twoStars++;
                            break;
                        case 1:
                            oneStar++;
                            break;
                    }
                }
            }

            CurrentGradesCount = gradesCount;

            theBestFlowerShop.SetProgress(fiveStars);

            if (gradesCount > 0)
            {
                CurrentAverageGrade = (float)gradesSumForCurrentAverage / gradesCount;
                string ratingText = (gradesSumForCurrentAverage / gradesCount).ToString() + 
                    flowersForSaleCoeffCalculatorSettings.FractionalSeparationSign.GetLocalizedString() + 
                    (gradesSumForCurrentAverage % gradesCount * 10 / gradesCount).ToString();
                UpdateGradeRatingOnCanvas(ratingText);
            }
            else
            {
                CurrentAverageGrade = 0;
                UpdateGradeRatingOnCanvas(
                    "0" + flowersForSaleCoeffCalculatorSettings.FractionalSeparationSign.GetLocalizedString() + "0");
            }

            statsCanvasLiaison.UpdateStatsCanvas(gradesCount, fiveStars, fourStars, treeStars, twoStars, oneStar);

            CalculateCurrentCustomersSpawnCoeff();
        }

        private void CalculateCurrentCustomersSpawnCoeff()
        {
            CurrentCustomersSpawnCoeff = CurrentAverageGrade / flowersForSaleCoeffCalculatorSettings.MaxShopGrade;
        }

        private void UpdateGradeRatingOnCanvas(string rating)
        {
            playerStatsCanvasLiaison.UpdateShopRating(rating);
        }
    }
}