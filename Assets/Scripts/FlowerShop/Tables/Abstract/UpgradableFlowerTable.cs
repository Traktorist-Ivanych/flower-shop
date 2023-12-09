using FlowerShop.Upgrades;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(UpgradableTableComponents))]
public abstract class UpgradableFlowerTable : FlowerTable, IUpgradableTable
{
    [Inject] private readonly RepairsAndUpgradesTable repairsAndUpgradesTable;

    private UpgradableTableComponents upgradableTableComponents;
    private protected int tableLvl;

    private protected virtual void Start()
    {
        upgradableTableComponents = GetComponent<UpgradableTableComponents>();
        AddUpgradableTableToList();
    }

    public void AddUpgradableTableToList()
    {
        repairsAndUpgradesTable.AddUpgradableTableToList(this);
    }

    public virtual void HideUpgradeIndicator()
    {
        upgradableTableComponents.HideUpgradeIndicator();
    }

    public virtual void UpgradeTable()
    {
        upgradableTableComponents.SetNextLvlMesh(tableLvl);
        tableLvl++;
        ShowUpgradeIndicator();
    }

    public void ShowUpgradeCanvas()
    {
        upgradableTableComponents.SetUpgradableTableInfoToCanvas(tableLvl);
    }

    public virtual void ShowUpgradeIndicator()
    {
        // should be in settings (comparing with maxLevel)
        if (tableLvl < 2)
        {
            upgradableTableComponents.ShowUpgradeIndicator();
        }
        else
        {
            upgradableTableComponents.HideUpgradeIndicator();
        }
    }
}
