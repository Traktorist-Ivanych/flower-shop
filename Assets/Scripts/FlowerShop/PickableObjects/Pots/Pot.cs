using FlowerShop.Flowers;
using PlayerControl;
using UnityEngine;
using Zenject;

[RequireComponent (typeof(OldPickableObjectMoving))]
[RequireComponent(typeof(PotObjects))]
public class Pot : MonoBehaviour, IPickableObject
{
    [Inject] private readonly PlayerPickableObjectHandler playerPickableObjectHandler;
    [Inject] private readonly GameConfiguration gameConfiguration;
    [Inject] private readonly FlowersContainer flowersContainer;

    [field: SerializeField] public GrowingRoom GrowingRoom { get; private set; }
    
    private FlowerInfo plantedFlowerInfo;
    private OldPickableObjectMoving potMoving;
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

    public FlowerInfo PlantedFlowerInfo
    {
        get => plantedFlowerInfo;
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
        // move to awake because we don't need to recashing it on disable/enable
        potMoving = GetComponent<OldPickableObjectMoving>();
        potObjects = GetComponent<PotObjects>();
        upGrowingLvlTime = gameConfiguration.UpGrowingLvlTime;
        plantedFlowerInfo = flowersContainer.EmptyFlowerInfo;
    }

    private void Update()
    {
        if (isWeedInPot)
        {
            // 3 should be in settings
            if (weedGrowingLvl < 3 && ShouldGrowingLvlIncrease())
            {
                currentUpGrowingLvlTime = 0; 
                weedGrowingLvl++;
                potObjects.SetWeedLvlMesh(weedGrowingLvl);
            }
        }
        else if (isPotOnGrowingTable && !isFlowerNeedWater && flowerGrowingLvl < 3 && ShouldGrowingLvlIncrease())
        {
            currentUpGrowingLvlTime = 0;
            isFlowerNeedWater = true;
            potObjects.ShowWaterIndicator();
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

    public void PlantSeed(FlowerInfo flowerInfoForPlanting)
    {
        plantedFlowerInfo = flowerInfoForPlanting;
        flowerGrowingLvl = 0;
        potObjects.SetFlowerLvlMesh(plantedFlowerInfo, flowerGrowingLvl);
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
        plantedFlowerInfo = flowersContainer.EmptyFlowerInfo;
        weedGrowingLvl = 0;
        flowerGrowingLvl = 0;
        currentUpGrowingLvlTime = 0;
        potObjects.HideAllPotObjects();
    }

    public void TakePotInPlayerHandsFromGrowingTable()
    {
        isPotOnGrowingTable = false;
        TakeInPlayerHands();
    }

    public void TakeInPlayerHands()
    {
        playerPickableObjectHandler.CurrentPickableObject = this;
        potMoving.PutBigPickableObjectInPlayerHands();
    }

    public void GivePotOnGrowingTableAndSetPlayerFree(Transform targetTransform, int growingTableLvl)
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
        GivePotAndSetPlayerFree(targetTransform);
    }

    public void GivePotAndKeepPlayerBusy(Transform targetTransform)
    {
        potMoving.ShouldPlayerBecomeFree = false;
        GivePotAndSetPlayerFree(targetTransform);
    }

    public void GivePotAndSetPlayerFree(Transform targetTransform)
    {
        potMoving.PutBigPickableObjectOnTable(targetTransform);
    }
    
    public void PutOnTable(Transform targetTransform)
    {
        potMoving.PutBigPickableObjectOnTable(targetTransform);   
    }

    public void PourFlower()
    {
        isFlowerNeedWater = false;
        potObjects.HideWaterIndicator();
        ++flowerGrowingLvl;
        potObjects.SetFlowerLvlMesh(plantedFlowerInfo, flowerGrowingLvl);

        ((WateringCan)playerPickableObjectHandler.CurrentPickableObject).PourPotWithWateringCan();
    }

    public void CrossFlower()
    {
        --flowerGrowingLvl;
        potObjects.SetFlowerLvlMesh(plantedFlowerInfo, flowerGrowingLvl);
    }
}
