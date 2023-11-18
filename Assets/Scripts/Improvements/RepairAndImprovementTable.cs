using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RepairAndImprovementTable : FlowerTable
{
    [Inject] private readonly GameConfiguration gameConfiguration;

    [SerializeField] private Transform hammerOnTableTransform;
    [SerializeField] private Hammer hammer;

    private delegate void RepairAndImprovementTableAction();
    private event RepairAndImprovementTableAction RepairAndImprovementTableEvent;

    private readonly List<IImprovableTable> improvableTables = new();

    public override void ExecuteClickableAbility()
    {
        if (playerBusyness.IsPlayerFree)
        {
            if (playerDinamicObject.IsPlayerDynamicObjectNull())
            {
                SetPlayerDestination();
                RepairAndImprovementTableEvent = null;
                RepairAndImprovementTableEvent += TakeHammerInPlayerHands;
            }
            else if (playerDinamicObject.GetCurrentPlayerDinamicObject() is Hammer)
            {
                SetPlayerDestination();
                RepairAndImprovementTableEvent = null;
                RepairAndImprovementTableEvent += PutHammerOnWeedingTable;
            }
        }
    }

    public override void ExecutePlayerAbility()
    {
        RepairAndImprovementTableEvent?.Invoke();
    }

    public void AddImprovementTableToList(IImprovableTable improvableTable)
    {
        improvableTables.Add(improvableTable);
    }

    private void TakeHammerInPlayerHands()
    {
        hammer.TakeHammer();
        StartCoroutine(ShowAllImprovableIndicators());
    }

    private void PutHammerOnWeedingTable()
    {
        playerDinamicObject.ClearPlayerDinamicObject();
        hammer.GiveHammer(hammerOnTableTransform);

        foreach (IImprovableTable improvableTable in improvableTables)
        {
            improvableTable.HideImprovableIndicator();
        }
    }

    private IEnumerator ShowAllImprovableIndicators()
    {
        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay);
        foreach (IImprovableTable improvableTable in improvableTables)
        {
            improvableTable.ShowImprovableIndicator();
        }
    }
}
