using System.Collections.Generic;
using System.Linq;
using FlowerShop.PickableObjects;
using FlowerShop.Tables.Abstract;
using UnityEngine;

// it is not exactly flowerTable, but just table for pots (maybe rename flowerTable to just table)
public class PotsRack : UpgradableTable
{
    [SerializeField] private Transform PotObjectsTransform;
    [SerializeField] private List<Pot> pots;
    [SerializeField] private List<MeshRenderer> potsRenderers;

    private delegate void PotsRackAction();
    private event PotsRackAction PotsRackEvent;

    private int currentFreePots = 8;

    public override void ExecuteClickableAbility()
    {
        if (playerBusyness.IsPlayerFree)
        {
            if (playerPickableObjectHandler.IsPickableObjectNull && currentFreePots > 0)
            {
                SetPlayerDestination();
                PotsRackEvent = null;
                PotsRackEvent += GivePot;
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is Pot)
            {
                Pot currentPlayerPot = playerPickableObjectHandler.CurrentPickableObject as Pot;
                if (!currentPlayerPot.IsSoilInsidePot && currentPlayerPot.GrowingRoom == growingRoom)
                {
                    SetPlayerDestination();
                    PotsRackEvent = null;
                    PotsRackEvent += TakePot;
                }
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer && tableLvl < 2)
            {
                SetPlayerDestination();
                PotsRackEvent = null;
                PotsRackEvent += ShowUpgradeCanvas;
            }
        }
    }

    public override void ExecutePlayerAbility()
    {
        PotsRackEvent?.Invoke();
    }

    private void GivePot()
    {
        potsRenderers[currentFreePots - 1].enabled = false;
        currentFreePots--;
        pots.Last().TakeInPlayerHandsAndSetPlayerFree();
        pots.Remove(pots.Last());
    }

    private void TakePot()
    {
        currentFreePots++;
        potsRenderers[currentFreePots - 1].enabled = true;
        Pot takingPot = playerPickableObjectHandler.CurrentPickableObject as Pot;
        pots.Add(takingPot);
        takingPot.PutOnTableAndSetPlayerFree(PotObjectsTransform);
        playerPickableObjectHandler.ResetPickableObject();
    }

    public override void UpgradeTable()
    {
        base.UpgradeTable();

        currentFreePots += 8;
        for (int i = 0; i < currentFreePots; i++)
        {
            potsRenderers[i].enabled = true;
        }
    }
}
