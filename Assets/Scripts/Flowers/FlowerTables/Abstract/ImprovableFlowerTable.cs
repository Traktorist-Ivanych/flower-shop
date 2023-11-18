using UnityEngine;
using Zenject;

[RequireComponent(typeof(ImprovementTableComponents))]
public abstract class ImprovableFlowerTable : FlowerTable, IImprovableTable
{
    [Inject] private readonly RepairAndImprovementTable repairAndImprovementTable;

    private ImprovementTableComponents improvementTableComponents;
    private protected int tableLvl;

    private protected virtual void Start()
    {
        improvementTableComponents = GetComponent<ImprovementTableComponents>();
        AddImprovableTableToList();
    }

    public void AddImprovableTableToList()
    {
        repairAndImprovementTable.AddImprovementTableToList(this);
    }

    public virtual void HideImprovableIndicator()
    {
        improvementTableComponents.HideImprovementIndicator();
    }

    public virtual void ImproveTable()
    {
        improvementTableComponents.SetNextLvlMesh(tableLvl);
        tableLvl++;
        ShowImprovableIndicator();
    }

    public void ShowImprovableCanvas()
    {
        improvementTableComponents.SetImprovementTableInfoToCanvas(tableLvl);
    }

    public virtual void ShowImprovableIndicator()
    {
        if (tableLvl < 2)
        {
            improvementTableComponents.ShowImprovementIndicator();
        }
        else
        {
            improvementTableComponents.HideImprovementIndicator();
        }
    }
}
