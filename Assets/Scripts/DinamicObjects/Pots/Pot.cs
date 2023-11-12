using UnityEngine;
using Zenject;

[RequireComponent (typeof(DinamicObjectMoving))]
[RequireComponent(typeof(PotObjects))]
public class Pot : MonoBehaviour, IDinamicObject, IGrowingRoom
{
    [Inject] private readonly CurrentPlayerDinamicObject playerDinamicObject;
    [Inject] private readonly GameConfiguration gameConfiguration;
    [Inject] private readonly FlowersContainer flowersContainer;

    [SerializeField] private IGrowingRoom.GroweringRoom groweringRoom;

    private Flower plantedFlower;
    private DinamicObjectMoving potMoving;
    private PotObjects potObjects;
    private float upGrowingLvlTime;
    private float currentUpGrowingLvlTime;
    private int flowerGrowingLvl;
    private int weedGrowingLvl;
    private bool isSoilInsidePot;
    private bool isPotOnGrowingTable;
    private bool isFlowerNeedWater;
    private bool isWeedInPot;

    public PotObjects PotObjects 
    { 
        get => potObjects; 
    }

    public IGrowingRoom.GroweringRoom GetGroweringRoom() { return groweringRoom; }

    public Flower PlantedFlower
    {
        get => plantedFlower;
    }

    public int FlowerGrowingLvl
    {
        get => flowerGrowingLvl;
    }

    public bool IsSoilInsidePot
    { 
        get => isSoilInsidePot;
    }

    public bool IsFlowerNeedWater
    {
        get => isFlowerNeedWater;
    }

    public bool IsWeedInPot
    {
        get => isWeedInPot;
    }

    private void Start()
    {
        potMoving = GetComponent<DinamicObjectMoving>();
        potObjects = GetComponent<PotObjects>();
        upGrowingLvlTime = gameConfiguration.UpGrowingLvlTime;
        plantedFlower = flowersContainer.EmptyFlower;
    }

    private void Update()
    {
        if (isWeedInPot)
        {
            if (weedGrowingLvl < 3 && ShouldGrowingLvlIncrease())
            {
                currentUpGrowingLvlTime = 0; 
                weedGrowingLvl++;
                potObjects.SetWeedLvlMesh(weedGrowingLvl);
            }
        }
        else if (isPotOnGrowingTable && !isFlowerNeedWater && flowerGrowingLvl < 3)
        {
            if (ShouldGrowingLvlIncrease())
            {
                currentUpGrowingLvlTime = 0;
                isFlowerNeedWater = true;
                potObjects.ShowWaterIndivator();
            }
        }
    }

    private bool ShouldGrowingLvlIncrease()
    {
        currentUpGrowingLvlTime += Time.deltaTime;
        return currentUpGrowingLvlTime >= upGrowingLvlTime;
    }

    public void FillPotWithSoil()
    {
        isSoilInsidePot = true;
        potObjects.ShowSoil();
    }

    public void PlantSeed(Flower flowerForPlanting)
    {
        plantedFlower = flowerForPlanting;
        flowerGrowingLvl = 0;
        potObjects.SetFlowerLvlMesh(plantedFlower, flowerGrowingLvl);
        potObjects.ShowFlower();
    }

    public void PlantWeed()
    {
        isWeedInPot = true;
        weedGrowingLvl = 1;
        currentUpGrowingLvlTime = 0;
        potObjects.SetWeedLvlMesh(weedGrowingLvl);
        potObjects.ShowWeed();
    }

    public void DeleteWeed()
    {
        isWeedInPot = false;
        weedGrowingLvl = 0;
        currentUpGrowingLvlTime = 0;
        potObjects.HideWeed();
    }

    public void CleanPot()
    {
        isSoilInsidePot = false;
        isFlowerNeedWater = false;
        isWeedInPot = false;
        plantedFlower = flowersContainer.EmptyFlower;
        weedGrowingLvl = 0;
        flowerGrowingLvl = 0;
        currentUpGrowingLvlTime = 0;
        potObjects.HideAllPotObjects();
    }

    public void TakePotInPlayerHandsFromGrowingTable()
    {
        isPotOnGrowingTable = false;
        TakePotInPlayerHands();
    }

    public void TakePotInPlayerHands()
    {
        playerDinamicObject.SetPlayerDinamicObject(this);
        potMoving.PutBigDinamicObjectInPlayerHands();
    }

    public void GivePotOnGrowingTableAndSetPlayerFree(Transform targetTransfom, int growingTableLvl)
    {
        isPotOnGrowingTable = true;
        if (currentUpGrowingLvlTime > 0)
        {
            float currentGrowingLvlTimeCoeff = currentUpGrowingLvlTime / upGrowingLvlTime;
            upGrowingLvlTime = gameConfiguration.UpGrowingLvlTime - gameConfiguration.UpGrowingLvlTableLvlTimeDelta * growingTableLvl;
            currentUpGrowingLvlTime *= currentGrowingLvlTimeCoeff;
        }
        else
        {
            upGrowingLvlTime = gameConfiguration.UpGrowingLvlTime - gameConfiguration.UpGrowingLvlTableLvlTimeDelta * growingTableLvl;
        }
        GivePotAndSetPlayerFree(targetTransfom);
    }

    public void GivePotAndKeepPlayerBusy(Transform targetTransfom)
    {
        potMoving.ShouldPlayerBecomeFree = false;
        GivePotAndSetPlayerFree(targetTransfom);
    }

    public void GivePotAndSetPlayerFree(Transform targetTransfom)
    {
        potMoving.PutBigDinamicObjectOnTable(targetTransfom);
    }

    public void PourFlower()
    {
        isFlowerNeedWater = false;
        potObjects.HideWaterIndicator();
        ++flowerGrowingLvl;
        potObjects.SetFlowerLvlMesh(plantedFlower, flowerGrowingLvl);

        (playerDinamicObject.GetCurrentPlayerDinamicObject() as WateringCan).PourPotWithWateringCan();
    }

    public void CrossFlower()
    {
        --flowerGrowingLvl;
        potObjects.SetFlowerLvlMesh(plantedFlower, flowerGrowingLvl);
    }
}
