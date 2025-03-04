using UnityEngine;
using UnityEngine.Localization;

namespace FlowerShop.RepairsAndUpgrades
{
    [CreateAssetMenu(fileName = "NewUpgradableTable", menuName = "Upgradable Table Info", order = 51)]
    public class UpgradableTableInfo : ScriptableObject
    {
        [SerializeField] private Mesh[] upgradeMeshes = new Mesh[2];
        [SerializeField] private Sprite[] upgradeSprites = new Sprite[2];
        [SerializeField] private LocalizedString[] localizedUpgradeDescriptions = new LocalizedString[2];
        [SerializeField] private string[] upgradeDescriptions = new string[2];
        [SerializeField] private int[] upgradePrices = new int[2];

        [field: SerializeField] public LocalizedString LocalizedTableName { get; private set; }
        [field: SerializeField] public string TableName { get; private set; }
        
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
            return localizedUpgradeDescriptions[index].GetLocalizedString();
        }

        public int GetUpgradableTablePrice(int index) 
        {
            return upgradePrices[index];
        }
    }
}
