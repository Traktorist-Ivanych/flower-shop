using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent (typeof(BreakableFlowerTableBase))]
public abstract class UpgradableBreakableFlowerTable : UpgradableFlowerTable, IBreakableTable
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

    public override void ShowUpgradeIndicator()
    {
        if (IsTableBroken)
        {
            ShowBreakdownIndicator();
        }
        else
        {
            base.ShowUpgradeIndicator();
        }
    }

    public override void HideUpgradeIndicator()
    {
        if (IsTableBroken) 
        {
            HideBreakdownIndicator();
        }
        else
        {
            base.HideUpgradeIndicator();
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
        base.ShowUpgradeIndicator();
    }
}
