using FlowerShop.Education;
using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Tables;
using UnityEngine;

namespace FlowerShop.CutScene
{
    public class CutSceneSignalsHandler : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        [SerializeField] private PotsRack potsRack;
        [SerializeField] private SoilPreparationTable soilPreparationTable;
        [SerializeField] private WateringTable wateringTable;
        [SerializeField] private FertilizersTable fertilizersTable;
        [SerializeField] private CustomerAccessControllerTable customerAccessControllerTable;
        [SerializeField] private FlowersSaleTable flowersSaleTable;
        [SerializeField] private FlowersSaleTable[] flowersSaleTables;
        [SerializeField] private FlowersGrowingTable flowersGrowingTableForPour;
        [SerializeField] private FlowersGrowingTable flowersGrowingTableMaxLvl1;
        [SerializeField] private FlowersGrowingTable flowersGrowingTableMaxLvl2;
        [SerializeField] private FlowersGrowingTable flowersGrowingTableMaxLvl3;
        [SerializeField] private FlowersGrowingTable flowersGrowingTableSecondLvl1;
        [SerializeField] private FlowersGrowingTable flowersGrowingTableSecondLvl2;
        [SerializeField] private FlowersGrowingTable flowersGrowingTableSecondLvl3;
        [SerializeField] private FlowersGrowingTable flowersGrowingTableSecondLvl4;
        [SerializeField] private FlowersGrowingTable flowersGrowingTableFirstLvl1;
        [SerializeField] private FlowersGrowingTable flowersGrowingTableFirstLvl2;
        [SerializeField] private FlowersCrossingTable flowersCrossingTableWithFlower;
        [SerializeField] private FlowersCrossingTable flowersCrossingTable;
        [SerializeField] private FlowersCrossingTableProcess flowersCrossingTableProcess;
        [SerializeField] private Pot potForPour;
        [SerializeField] private Pot potReadyForCrossing;

        [SerializeField] private Pot potForDecoration1;
        [SerializeField] private Pot potForDecoration2;
        [SerializeField] private Pot potForDecoration3;
        [SerializeField] private Pot potForDecoration4;
        [SerializeField] private Pot potForDecoration5;
        [SerializeField] private Pot potForDecoration6;
        [SerializeField] private Pot potForDecoration7;

        [SerializeField] private FlowerInfo flowerForDecoration1;
        [SerializeField] private FlowerInfo flowerForDecoration2;
        [SerializeField] private FlowerInfo flowerForDecoration3;
        [SerializeField] private FlowerInfo flowerForDecoration4;
        [SerializeField] private FlowerInfo flowerForDecoration5;
        [SerializeField] private FlowerInfo flowerForDecoration6;
        [SerializeField] private FlowerInfo flowerForDecoration7;

        [SerializeField] private FlowerInfo crossingFlowerInfoForPour;
        [SerializeField] private FlowerInfo crossingFlowerInfoReady;
        [SerializeField] private FlowerInfo[] flowerInfosForSale;

        [SerializeField] private EducationHandler educationHandler;

        private void Awake()
        {
            EducationHandlerForSaving educationHandlerForSaving = 
                new(false, 0, true, true, true);
            educationHandler.Load(educationHandlerForSaving);



            BoolForSaving boolForLoading = new(true);
            customerAccessControllerTable.Load(boolForLoading);



            for (int i = 0; i < flowersSaleTables.Length; i++)
            {
                FlowerInfoReferenceForSaving flowerInfoReferenceForLoading = new(flowerInfosForSale[i].UniqueKey);
                flowersSaleTables[i].Load(flowerInfoReferenceForLoading);
            }



            FlowersGrowingTableForSaving flowersGrowingTableForSaving1 =
                new("Empty", 2, 10);
            flowersGrowingTableMaxLvl1.Load(flowersGrowingTableForSaving1);

            FlowersGrowingTableForSaving flowersGrowingTableForSaving2 =
                new(potForDecoration1.UniqueKey, 2, 10);
            flowersGrowingTableMaxLvl2.Load(flowersGrowingTableForSaving2);

            FlowersGrowingTableForSaving flowersGrowingTableForSaving3 =
                new(potForDecoration2.UniqueKey, 2, 10);
            flowersGrowingTableMaxLvl3.Load(flowersGrowingTableForSaving3);

            FlowersGrowingTableForSaving flowersGrowingTableForSaving11 =
                new("Empty", 1, 10);
            flowersGrowingTableSecondLvl1.Load(flowersGrowingTableForSaving11);

            FlowersGrowingTableForSaving flowersGrowingTableForSaving12 =
                new(potForDecoration3.UniqueKey, 1, 10);
            flowersGrowingTableSecondLvl2.Load(flowersGrowingTableForSaving12);

            FlowersGrowingTableForSaving flowersGrowingTableForSaving13 =
                new(potForDecoration4.UniqueKey, 1, 10);
            flowersGrowingTableSecondLvl3.Load(flowersGrowingTableForSaving13);

            FlowersGrowingTableForSaving flowersGrowingTableForSaving14 =
                new(potForDecoration5.UniqueKey, 1, 10);
            flowersGrowingTableSecondLvl4.Load(flowersGrowingTableForSaving14);

            FlowersGrowingTableForSaving flowersGrowingTableForSaving21 =
                new(potForDecoration6.UniqueKey, 0, 10);
            flowersGrowingTableFirstLvl1.Load(flowersGrowingTableForSaving21);

            FlowersGrowingTableForSaving flowersGrowingTableForSaving22 =
                new(potForDecoration7.UniqueKey, 0, 10);
            flowersGrowingTableFirstLvl2.Load(flowersGrowingTableForSaving22);

            PotForSaving potForPourSaving1 =
                new(true, flowerForDecoration1.UniqueKey, 2, 0, false, false, false, 0);
            potForDecoration1.Load(potForPourSaving1);

            PotForSaving potForPourSaving2 =
                new(true, flowerForDecoration2.UniqueKey, 3, 0, false, false, false, 0);
            potForDecoration2.Load(potForPourSaving2);

            PotForSaving potForPourSaving3 =
                new(true, flowerForDecoration3.UniqueKey, 2, 15, false, false, false, 0);
            potForDecoration3.Load(potForPourSaving3);

            PotForSaving potForPourSaving4 =
                new(true, flowerForDecoration4.UniqueKey, 1, 30, false, false, false, 0);
            potForDecoration4.Load(potForPourSaving4);

            PotForSaving potForPourSaving5 =
                new(true, flowerForDecoration5.UniqueKey, 3, 0, false, false, false, 0);
            potForDecoration5.Load(potForPourSaving5);

            PotForSaving potForPourSaving6 =
                new(true, flowerForDecoration6.UniqueKey, 1, 50, false, false, false, 0);
            potForDecoration5.Load(potForPourSaving6);

            PotForSaving potForPourSaving7 =
                new(true, flowerForDecoration7.UniqueKey, 2, 30, false, false, false, 0);
            potForDecoration5.Load(potForPourSaving7);



            WateringTableForSaving wateringTableForLoading = new(1, 10, 6, false);
            wateringTable.Load(wateringTableForLoading);



            PotForSaving potForPourSaving =
                new(true, crossingFlowerInfoForPour.UniqueKey, 2, 0, true, false, false, 0);
            potForPour.Load(potForPourSaving);

            FlowersGrowingTableForSaving flowersGrowingTableForSaving =
                new(potForPour.UniqueKey, 2, 10);
            flowersGrowingTableForPour.Load(flowersGrowingTableForSaving);



            PotForSaving potReadyForCrossingSaving =
                new(true, crossingFlowerInfoReady.UniqueKey, 3, 0, false, false, false, 0);
            potReadyForCrossing.Load(potReadyForCrossingSaving);

            FlowersCrossingTableForSaving flowersCrossingTableForLoading =
                new(potReadyForCrossing.UniqueKey);
            flowersCrossingTableWithFlower.Load(flowersCrossingTableForLoading);



            FlowersCrossingTableProcessForSaving flowersCrossingTableProcessForLoading =
                new(2, 10, 0, false, "Empty", false);
            flowersCrossingTableProcess.Load(flowersCrossingTableProcessForLoading);



            SoilPreparationTableForSaving soilPreparationTableForLoading =
                new(2, 10);
            soilPreparationTable.Load(soilPreparationTableForLoading);



            PotsRackForSaving potsRackForLoading = new(1);
            potsRack.Load(potsRackForLoading);
        }

        public void InteractWithWateringTable()
        {
            wateringTable.ExecuteClickableAbility();
        }

        public void InteractWithGrowingTable()
        {
            flowersGrowingTableForPour.ExecuteClickableAbility();
        }

        public void InteractWithFlowersCrossingTable()
        {
            flowersCrossingTable.ExecuteClickableAbility();
        }

        public void InteractWithFlowersCrossingTableProcess()
        {
            flowersCrossingTableProcess.ExecuteClickableAbility();
        }

        public void InteractWithPotsRack()
        {
            potsRack.ExecuteClickableAbility();
        }

        public void InteractWithSoilPreparationTable()
        {
            soilPreparationTable.ExecuteClickableAbility();
        }

        public void InteractWithGrowingTableForGrowingCrossedFlower()
        {
            flowersGrowingTableMaxLvl1.ExecuteClickableAbility();
        }

        public void InteractWithFertilizersTable()
        {
            fertilizersTable.TryInteractWithTableForCutScene();
        }

        public void InteractWithFertilizersTableForPutFertilizer()
        {
            fertilizersTable.ExecuteClickableAbility();
        }

        public void InteractWithFlowersSaleTable()
        {
            flowersSaleTable.ExecuteClickableAbility();
        }

        public void PlayAudioClip()
        {
            audioSource.Play();
        }
    }
}
