using UnityEngine;

namespace FlowerShop.Upgrades
{
    [CreateAssetMenu(fileName = "NewUpgradableTable", menuName = "Upgradable Table Info", order = 51)]
    public class UpgradableTableInfo : ScriptableObject
    {
        [field: SerializeField] public string TableName { get; private set; }
        [SerializeField] private Mesh[] upgradeMeshes = new Mesh[2];
        [SerializeField] private Sprite[] upgradeSprites = new Sprite[2];
        [SerializeField] private string[] upgradeDescriptions = new string[2];
        [SerializeField] private int[] upgradePrices = new int[2];

        public Mesh GetUpgradableTableMesh(int index)
        {
            return upgradeMeshes[index];
        }

        public Sprite GetUpgradableTableSprite(int index) 
        {
            return upgradeSprites[index];
        }

        public string GetUpgradableTableDescription(int index)
        {
            return upgradeDescriptions[index];
        }

        public int GetUpgradableTablePrice(int index) 
        {
            return upgradePrices[index];
        }
    }
}
