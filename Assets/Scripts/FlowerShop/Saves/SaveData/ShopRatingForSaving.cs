using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public struct ShopRatingForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public int[] ShopGrades { get; private set; }
        [field: SerializeField] public int CurrentGradeIndex { get; private set; }
        [field: SerializeField] public double LastAverageGrade { get; private set; }
        
        public ShopRatingForSaving(int[] shopGrades, int currentGradeIndex, double lastAverageGrade)
        {
            IsValuesSaved = true;
            ShopGrades = shopGrades;
            CurrentGradeIndex = currentGradeIndex;
            LastAverageGrade = lastAverageGrade;
        }
    }
}