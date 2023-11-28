using FlowerShop.PickableObjects;
using UnityEngine;
using Zenject;

namespace FlowerShop.Upgrades
{
    [RequireComponent(typeof(IUpgradableTable))]
    public class UpgradableTableComponents : MonoBehaviour
    {
        [Inject] private readonly UpgradeCanvasLiaison upgradeCanvasLiaison;
        [Inject] private readonly UpgradingAndRepairingHammer upgradingAndRepairingHammer;

        [SerializeField] private MeshRenderer upgradeIndicatorRenderer;
        [SerializeField] private MeshFilter tableMeshFilter; 
        [SerializeField] private UpgradableTableInfo upgradableTableInfo;

        private IUpgradableTable upgradableTable;

        private void Start()
        {
            upgradableTable = GetComponent<IUpgradableTable>();
        }

        public void ShowUpgradeIndicator()
        {
            upgradeIndicatorRenderer.enabled = true;
        }

        public void HideUpgradeIndicator()
        {
            upgradeIndicatorRenderer.enabled = false;
        }

        public void SetUpgradableTableInfoToCanvas(int nextTableLvl)
        {
            upgradeCanvasLiaison.SetUpgradableTableInfo(
                tableName: upgradableTableInfo.TableName,
                description: upgradableTableInfo.GetUpgradableTableDescription(nextTableLvl),
                priceInt: upgradableTableInfo.GetUpgradableTablePrice(nextTableLvl),
                tableSprite: upgradableTableInfo.GetUpgradableTableSprite(nextTableLvl));

            upgradingAndRepairingHammer.UpgradableTable = upgradableTable;
        }

        public void SetNextLvlMesh(int nextTableLvl)
        {
            tableMeshFilter.mesh = upgradableTableInfo.GetUpgradableTableMesh(nextTableLvl);
        }
    }
}
