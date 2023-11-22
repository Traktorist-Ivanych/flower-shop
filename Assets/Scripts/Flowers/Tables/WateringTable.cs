using System.Collections;
using UnityEngine;
using Zenject;

public class WateringTable : ImprovableBreakableFlowerTable
{
    [SerializeField] private Transform wateringCanTableTransform;
    [SerializeField] private WateringCan wateringCan;
    [SerializeField] private ParticleSystem waterPS;

    private delegate void WateringTableAction();
    private event WateringTableAction WateringTableEvent;

    private protected override void Start()
    {
        base.Start();

        SetActionsBeforeBrokenQuantity(
            gameConfiguration.WateringTableMinQuantity * (tableLvl + 1),
            gameConfiguration.WateringTableMaxQuantity * (tableLvl + 1));
    }

    public override void ExecuteClickableAbility()
    {
        if (playerBusyness.IsPlayerFree)
        {
            if (PlayerPickableObjectHandler.IsPlayerPickableObjectNull() && !wateringCan.IsWateringCanNeedForReplenish() && !IsTableBroken)
            {
                SetPlayerDestination();
                WateringTableEvent = null;
                WateringTableEvent += TakeWateringCanFromTable;
            }
            // try to do with one check/cast
            else if ((PlayerPickableObjectHandler.GetCurrentPlayerPickableObject() is WateringCan) &&
                     (PlayerPickableObjectHandler.GetCurrentPlayerPickableObject() as WateringCan).Equals(wateringCan))
            {
                SetPlayerDestination();
                WateringTableEvent = null;
                WateringTableEvent += PutWateringCanOnTable;
            }
            else if (PlayerPickableObjectHandler.GetCurrentPlayerPickableObject() is Hammer)
            {
                if (IsTableBroken)
                {
                    SetPlayerDestination();
                    WateringTableEvent = null;
                    WateringTableEvent += FixWateringTable;
                }
                else if (tableLvl < 2)
                {
                    SetPlayerDestination();
                    WateringTableEvent = null;
                    WateringTableEvent += ShowImprovableCanvas;
                }
            }
        }
    }

    public override void ExecutePlayerAbility()
    {
        WateringTableEvent?.Invoke();
    }

    public override void ImproveTable()
    {
        base.ImproveTable();
        wateringCan.ImproveWateringCan();
    }

    private void TakeWateringCanFromTable()
    {
        wateringCan.TakeWateringCan();
    }

    private void PutWateringCanOnTable()
    {
        wateringCan.GiveWateringCan(wateringCanTableTransform);
        PlayerPickableObjectHandler.ClearPlayerPickableObject();

        if (wateringCan.IsWateringCanNeedForReplenish())
        {
            StartCoroutine(ReplenishWateringCan());
        }
    }

    private IEnumerator ReplenishWateringCan()
    {
        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay);
        waterPS.Play();
        yield return new WaitForSeconds(wateringCan.ReplenishWateringCanTime());
        waterPS.Stop();
        wateringCan.ReplenishWateringCan();
        UseBreakableFlowerTable();
    }

    private void FixWateringTable()
    {
        FixBreakableFlowerTable(
            gameConfiguration.WateringTableMinQuantity * (tableLvl + 1),
            gameConfiguration.WateringTableMaxQuantity * (tableLvl + 1));
    }
}
