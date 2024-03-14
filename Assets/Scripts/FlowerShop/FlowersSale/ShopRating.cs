using System;
using System.Threading;
using FlowerShop.ComputerPages;
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
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly PlayerStatsCanvasLiaison playerStatsCanvasLiaison;
        [Inject] private readonly StatsEffects statsEffects;
        [Inject] private readonly StatsCanvasLiaison statsCanvasLiaison;

        private int[] shopGrades;
        private int currentGradeIndex;
        private double lastAverageGrade;
        
        private int gradesCount;
        private int fiveStars;
        private int fourStars;
        private int treeStars;
        private int twoStars;
        private int oneStar;

        public double CurrentAverageGrade { get; private set; } 
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

            CalculateAverageGrade();
        }

        public void Load()
        {
            ShopRatingForSaving shopRatingForSaving = SavesHandler.Load<ShopRatingForSaving>(UniqueKey);

            if (shopRatingForSaving.IsValuesSaved)
            {
                shopGrades = shopRatingForSaving.ShopGrades; 
                currentGradeIndex = shopRatingForSaving.CurrentGradeIndex; 
                lastAverageGrade = shopRatingForSaving.LastAverageGrade;
                CalculateCurrentAverageGrade();
            }
            else
            {
                shopGrades = new int[flowersSettings.MaxGradesCount];
                CurrentAverageGrade = 0;
                UpdateGradeRatingOnCanvas();
            }
        }

        public void Save()
        {
            ShopRatingForSaving shopRatingForSaving = new(shopGrades, currentGradeIndex, lastAverageGrade);
            
            SavesHandler.Save(UniqueKey, shopRatingForSaving);
        }

        private void CalculateAverageGrade()
        {
            lastAverageGrade = CurrentAverageGrade;
            
            CalculateCurrentAverageGrade();
            
            double averageGradeDelta = CurrentAverageGrade - lastAverageGrade;

            if (averageGradeDelta >= 0)
            {
                statsEffects.RatingChangeEffect.DisplayEffect(Color.green, 
                    "+" + averageGradeDelta.ToString(Thread.CurrentThread.CurrentCulture));
            }
            else
            {
                statsEffects.RatingChangeEffect.DisplayEffect(Color.red, 
                    averageGradeDelta.ToString(Thread.CurrentThread.CurrentCulture));
            }

            Save();
        }

        private void CalculateCurrentAverageGrade()
        {
            double gradesCountForCurrentAverage = 0;
            double gradesSumForCurrentAverage = 0;

            foreach (int shopGrade in shopGrades)
            {
                if (shopGrade != 0)
                {
                    gradesCount++;
                    gradesCountForCurrentAverage++;
                    gradesSumForCurrentAverage += shopGrade;

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

            if (gradesCountForCurrentAverage > 0)
            {
                double averageGrade = gradesSumForCurrentAverage / gradesCountForCurrentAverage;
                CurrentAverageGrade = Math.Round(averageGrade, 1, MidpointRounding.AwayFromZero);
            }
            
            UpdateGradeRatingOnCanvas();
            statsCanvasLiaison.UpdateStatsCanvas(gradesCount, fiveStars, fourStars, treeStars, twoStars, oneStar);
        }

        private void UpdateGradeRatingOnCanvas()
        {
            playerStatsCanvasLiaison.UpdateShopRating(
                CurrentAverageGrade.ToString(Thread.CurrentThread.CurrentCulture));
        }
    }
}