using FlowerShop.Flowers;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [System.Serializable]
    public class PotForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public bool IsSoilInsidePot { get; private set; }
        [field: SerializeField] public FlowerInfo PlantedFlowerInfo { get; private set; }
        [field: SerializeField] public int FlowerGrowingLvl { get; private set; }
        [field: SerializeField] public float CurrentUpGrowingLvlTime { get; private set; }
        [field: SerializeField] public bool IsFlowerNeedWater { get; private set; }
        [field: SerializeField] public bool IsPotTreatedByGrothAccelerator { get; private set; }
        [field: SerializeField] public bool IsWeedInPot { get; private set; }
        [field: SerializeField] public int WeedGrowingLvl { get; private set; }
        
        public PotForSaving() {}

        public PotForSaving(bool isSoilInsidePot, FlowerInfo plantedFlowerInfo, int flowerGrowingLvl, float currentUpGrowingLvlTime,
            bool isFlowerNeedWater, bool isPotTreatedByGrothAccelerator, bool isWeedInPot, int weedGrowingLvl)
        {
            IsValuesSaved = true;
            IsSoilInsidePot = isSoilInsidePot;
            PlantedFlowerInfo = plantedFlowerInfo;
            FlowerGrowingLvl = flowerGrowingLvl;
            CurrentUpGrowingLvlTime = currentUpGrowingLvlTime;
            IsFlowerNeedWater = isFlowerNeedWater;
            IsPotTreatedByGrothAccelerator = isPotTreatedByGrothAccelerator;
            IsWeedInPot = isWeedInPot;
            WeedGrowingLvl = weedGrowingLvl;
        }
    }
}