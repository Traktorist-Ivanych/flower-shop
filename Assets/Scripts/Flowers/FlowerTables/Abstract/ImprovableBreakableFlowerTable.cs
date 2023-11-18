using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent (typeof(BreakableFlowerTableBase))]
public abstract class ImprovableBreakableFlowerTable : ImprovableFlowerTable, IBreakableTable
{
    [Inject] private protected readonly GameConfiguration gameConfiguration;

    private BreakableFlowerTableBase breakableFlowerTablebase;

    public bool IsTableBroken 
    {
        get => breakableFlowerTablebase.IsTableBroken;
    }

    private protected override void Start()
    {
        base.Start();
        breakableFlowerTablebase = GetComponent<BreakableFlowerTableBase>();
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
        breakableFlowerTablebase.ShowBreakdownIndicator();
    }

    public void HideBreakdownIndicator()
    {
        breakableFlowerTablebase.HideBreakdownIndicator();
    }

    public void UseBreakableFlowerTable()
    {
        breakableFlowerTablebase.UseBreakableFlowerTable();
    }

    public void FixBreakableFlowerTable(int minQuantity, int maxQuantity)
    {
        breakableFlowerTablebase.FixBreakableFlowerTable(minQuantity, maxQuantity);
        StartCoroutine(ShowImprovableIndicatorAfterRepair());
    }

    public void SetActionsBeforeBrokenQuantity(int minQuantity, int maxQuantity)
    {
        breakableFlowerTablebase.SetActionsBeforeBrokenQuantity(minQuantity, maxQuantity);
    }

    private IEnumerator ShowImprovableIndicatorAfterRepair()
    {
        yield return new WaitForSeconds(gameConfiguration.TableRepairTime);
        base.ShowImprovableIndicator();
    }
}
