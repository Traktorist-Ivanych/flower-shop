using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public struct EducationHandlerForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public bool IsEducationActive { get; private set; }
        [field: SerializeField] public int Step { get; private set; }
        [field: SerializeField] public bool IsFirstLvlClueWasDisplayed { get; private set; }
        [field: SerializeField] public bool IsSecondLvlClueWasDisplayed { get; private set; }
        [field: SerializeField] public bool IsThirdLvlClueWasDisplayed { get; private set; }

        public EducationHandlerForSaving(bool isEducationActive, int step, bool isFirstLvlClueWasDisplayed, 
            bool isSecondLvlClueWasDisplayed, bool isThirdLvlClueWasDisplayed)
        {
            IsValuesSaved = true;
            IsEducationActive = isEducationActive;
            Step = step;
            IsFirstLvlClueWasDisplayed = isFirstLvlClueWasDisplayed;
            IsSecondLvlClueWasDisplayed = isSecondLvlClueWasDisplayed;
            IsThirdLvlClueWasDisplayed = isThirdLvlClueWasDisplayed;
        }
    }
}