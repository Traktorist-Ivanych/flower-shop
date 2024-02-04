using FlowerShop.PickableObjects;
using FlowerShop.RepairsAndUpgrades;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables.BaseComponents
{
    [RequireComponent(typeof(IUpgradableTable))]
    public class UpgradableTableBaseComponent : MonoBehaviour
    {
        [Inject] private readonly UpgradeCanvasLiaison upgradeCanvasLiaison;
        [Inject] private readonly RepairingAndUpgradingHammer repairingAndUpgradingHammer;

        [SerializeField] private MeshRenderer upgradeIndicatorRenderer;
        [SerializeField] private MeshFilter tableMeshFilter; 
        [SerializeField] private UpgradableTableInfo upgradableTableInfo;
        [SerializeField] private ParticleSystem UpgradeTableSuccessPS;

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

            repairingAndUpgradingHammer.UpgradableTable = upgradableTable;
        }

        public void FinishUpgradeTableProcessEffects()
        {
            UpgradeTableSuccessPS.Play();
        }

        public void SetNextLvlMesh(int nextTableLvl)
        {
            if (nextTableLvl > 0)
            {
                tableMeshFilter.mesh = upgradableTableInfo.GetUpgradableTableMesh(nextTableLvl - 1);
            }
        }

        public int GetUpgradePrice(int nextTableLvl)
        {
            return upgradableTableInfo.GetUpgradableTablePrice(nextTableLvl);
        }
    }
}
