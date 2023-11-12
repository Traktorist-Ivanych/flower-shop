using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent (typeof(BreakableFlowerTableBaseLogic))]
public abstract class ImprovableBreakableFlowerTable : ImprovableFlowerTable, IBreakableTable
{
    [Inject] private protected readonly GameConfiguration gameConfiguration;

    private BreakableFlowerTableBaseLogic breakableBaseLogic;

    public bool IsTableBroken 
    {
        get => breakableBaseLogic.IsTableBroken;
    }

    private protected override void Start()
    {
        base.Start();
        breakableBaseLogic = GetComponent<BreakableFlowerTableBaseLogic>();
    }

    public override void ShowImprovableIndicator()
    {
        if (IsTableBroken)
        {
            ShowBreakdownIndicator();
        }
        else
        {
            base.ShowImprovableIndicator();
        }
    }

    public override void HideImprovableIndicator()
    {
        if (IsTableBroken) 
        {
            HideBreakdownIndicator();
        }
        else
        {
            base.HideImprovableIndicator();
        }
    }

    public void ShowBreakdownIndicator()
    {
        breakableBaseLogic.ShowBreakdownIndicator();
    }

    public void HideBreakdownIndicator()
    {
        breakableBaseLogic.HideBreakdownIndicator();
    }

    public void UseBreakableFlowerTable()
    {
        breakableBaseLogic.UseBreakableFlowerTable();
    }

    public void FixBreakableFlowerTable(int minQuantity, int maxQuantity)
    {
        breakableBaseLogic.FixBreakableFlowerTable(minQuantity, maxQuantity);
        StartCoroutine(ShowImprovableIndicatorAfterRepair());
    }

    public void SetActionsBeforeBrokenQuantity(int minQuantity, int maxQuantity)
    {
        breakableBaseLogic.SetActionsBeforeBrokenQuantity(minQuantity, maxQuantity);
    }

    private IEnumerator ShowImprovableIndicatorAfterRepair()
    {
        yield return new WaitForSeconds(gameConfiguration.TableRepairTime);
        base.ShowImprovableIndicator();
    }
}
