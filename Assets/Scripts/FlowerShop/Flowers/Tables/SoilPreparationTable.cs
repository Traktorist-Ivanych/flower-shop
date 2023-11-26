using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(ImprovementTableComponents))]
public class SoilPreparationTable : ImprovableBreakableFlowerTable
{
    [SerializeField] private Transform potOnTableTransform;
    [SerializeField] private Transform[] gearsTransform;

    private delegate void SoilPreparationTableAction();
    private event SoilPreparationTableAction SoilPreparationTableEvent;

    private Pot potToSoilPreparation;
    private float soilPreparationTime;
    private float currentSoilPreparationTime;
    private bool isSoilBeingPrepared;

    public int TableLvl
    {
        get => tableLvl;
    }

    private protected override void Start()
    {
        base.Start();
        SetSoilPreparationTime();

        SetActionsBeforeBrokenQuantity(
            gameConfiguration.SoilPreparationMinQuantity * (tableLvl + 1),
            gameConfiguration.SoilPreparationMaxQuantity * (tableLvl + 1));
    }

    private void Update()
    {
        if (isSoilBeingPrepared)
        {
            currentSoilPreparationTime -= Time.deltaTime;

            foreach (Transform geatTransform in gearsTransform)
            {
                geatTransform.Rotate(Vector3.forward, gameConfiguration.ObjectsRotateDegreesPerSecond * Time.deltaTime);
            }

            if (currentSoilPreparationTime <= 0)
            {
                currentSoilPreparationTime = soilPreparationTime;
                isSoilBeingPrepared = false;
                potToSoilPreparation.TakePotInPlayerHands();
                UseBreakableFlowerTable();
            }
        }
    }

    public override void ExecuteClickableAbility()
    {
        if (playerBusyness.IsPlayerFree)
        {
            if (playerPickableObjectHandler.CurrentPickableObject is Pot)
            {
                potToSoilPreparation = playerPickableObjectHandler.CurrentPickableObject as Pot;
                if (potToSoilPreparation.GrowingRoom == growingRoom && !potToSoilPreparation.IsSoilInsidePot &&
                    !IsTableBroken)
                {
                    SetPlayerDestination();
                    SoilPreparationTableEvent = null;
                    SoilPreparationTableEvent += delegate { StartCoroutine(SoilPreparation()); };
                }
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is Hammer)
            {
                if (IsTableBroken)
                {
                    SetPlayerDestination();
                    SoilPreparationTableEvent = null;
                    SoilPreparationTableEvent += FixSoilPreparationTable;
                }
                else if (tableLvl < 2)
                {
                    SetPlayerDestination();
                    SoilPreparationTableEvent = null;
                    SoilPreparationTableEvent += ShowImprovableCanvas;
                }
            }
        }
    }

    public override void ExecutePlayerAbility()
    {
        SoilPreparationTableEvent?.Invoke();
    }

    public override void ImproveTable()
    {
        base.ImproveTable();
        SetSoilPreparationTime();
    }

    private void SetSoilPreparationTime()
    {
        soilPreparationTime = gameConfiguration.SoilPreparationTime - tableLvl * gameConfiguration.SoilPreparationLvlTimeDelta;
        currentSoilPreparationTime = soilPreparationTime;
    }

    private IEnumerator SoilPreparation()
    {
        potToSoilPreparation.GivePotAndKeepPlayerBusy(potOnTableTransform);

        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay);
        potToSoilPreparation.FillPotWithSoil();
        isSoilBeingPrepared = true;
    }

    private void FixSoilPreparationTable()
    {
        FixBreakableFlowerTable(
            gameConfiguration.SoilPreparationMinQuantity * (tableLvl + 1),
            gameConfiguration.SoilPreparationMaxQuantity * (tableLvl + 1));
    }
}
