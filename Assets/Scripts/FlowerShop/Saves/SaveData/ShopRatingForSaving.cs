﻿using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public struct ShopRatingForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public int[] ShopGrades { get; private set; }
        [field: SerializeField] public int CurrentGradeIndex { get; private set; }
        
        public ShopRatingForSaving(int[] shopGrades, int currentGradeIndex)
        {
            IsValuesSaved = true;
            ShopGrades = shopGrades;
            CurrentGradeIndex = currentGradeIndex;
        }
    }
}