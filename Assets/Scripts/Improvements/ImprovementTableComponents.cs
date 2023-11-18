using UnityEngine;
using Zenject;

[RequireComponent(typeof(IImprovableTable))]
public class ImprovementTableComponents : MonoBehaviour
{
    [Inject] private readonly AllCanvasLiaisons allCanvasLiaisons;
    [Inject] private readonly Hammer hammer;

    [SerializeField] private MeshRenderer improvementIndicatorRenderer;
    [SerializeField] private MeshFilter tableMeshFilter;
    [SerializeField] private ImprovementTable improvementTable;

    private IImprovableTable improvableTable;

    private void Start()
    {
        improvableTable = GetComponent<IImprovableTable>();
    }

    public void ShowImprovementIndicator()
    {
        improvementIndicatorRenderer.enabled = true;
    }

    public void HideImprovementIndicator()
    {
        improvementIndicatorRenderer.enabled = false;
    }

    public void SetImprovementTableInfoToCanvas(int nextTableLvl)
    {
        // good job, you can do it even almost always - easier to read
        allCanvasLiaisons.ImprovementCanvasLiaison.SetImprovementInfo(
            tableName: improvementTable.TableName,
            description: improvementTable.GetImprovementDescription(nextTableLvl),
            priceInt: improvementTable.GetImprovementPrice(nextTableLvl),
            tableSprite: improvementTable.GetImprovementSprite(nextTableLvl));

        hammer.ImprovableTable = improvableTable;
    }

    public void SetNextLvlMesh(int nextTableLvl)
    {
        tableMeshFilter.mesh = improvementTable.GetImprovementMesh(nextTableLvl);
    }
}
