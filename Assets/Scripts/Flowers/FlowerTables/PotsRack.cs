using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// it is not exactly flowerTable, but just table for pots (maybe rename flowerTable to just table)
public class PotsRack : ImprovableFlowerTable
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
            if (playerDinamicObject.IsPlayerDynamicObjectNull() && currentFreePots > 0)
            {
                SetPlayerDestination();
                PotsRackEvent = null;
                PotsRackEvent += GivePot;
            }
            else if (playerDinamicObject.GetCurrentPlayerDinamicObject() is Pot)
            {
                Pot currentPlayerPot = playerDinamicObject.GetCurrentPlayerDinamicObject() as Pot;
                if (!currentPlayerPot.IsSoilInsidePot && currentPlayerPot.GetGroweringRoom() == groweringRoom)
                {
                    SetPlayerDestination();
                    PotsRackEvent = null;
                    PotsRackEvent += TakePot;
                }
            }
            else if (playerDinamicObject.GetCurrentPlayerDinamicObject() is Hammer && tableLvl < 2)
            {
                SetPlayerDestination();
                PotsRackEvent = null;
                PotsRackEvent += ShowImprovableCanvas;
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
        pots.Last().TakePotInPlayerHands();
        pots.Remove(pots.Last());
    }

    private void TakePot()
    {
        currentFreePots++;
        potsRenderers[currentFreePots - 1].enabled = true;
        Pot takingPot = playerDinamicObject.GetCurrentPlayerDinamicObject() as Pot;
        pots.Add(takingPot);
        takingPot.GivePotAndSetPlayerFree(PotObjectsTransform);
        playerDinamicObject.ClearPlayerDinamicObject();
    }

    public override void ImproveTable()
    {
        base.ImproveTable();

        currentFreePots += 8;
        for (int i = 0; i < currentFreePots; i++)
        {
            potsRenderers[i].enabled = true;
        }
    }
}
