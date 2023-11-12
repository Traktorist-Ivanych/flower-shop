using UnityEngine;

[CreateAssetMenu(fileName = "New ImprovementTable", menuName = "Improvement Table", order = 51)]
public class ImprovementTable : ScriptableObject
{
    [SerializeField] private Mesh[] improvementMeshes = new Mesh[2];
    [SerializeField] private Sprite[] improvementSprites = new Sprite[2];
    [SerializeField] private string tableName;
    [SerializeField] private string[] improvementDescriptions = new string[2];
    [SerializeField] private int[] improvementPrices = new int[2];

    public Mesh GetImprovementMesh(int index)
    {
        return improvementMeshes[index];
    }

    public Sprite GetImprovementSprite(int index) 
    {
        return improvementSprites[index];
    }

    public string TableName
    {
        get => tableName;
    }

    public string GetImprovementDescription(int index)
    {
        return improvementDescriptions[index];
    }

    public int GetImprovementPrice(int index) 
    {
        return improvementPrices[index];
    }
}
