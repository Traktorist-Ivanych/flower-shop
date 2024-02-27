using FlowerShop.PickableObjects;
using FlowerShop.RepairsAndUpgrades;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables.BaseComponents
{
    [RequireComponent(typeof(IUpgradableTable))]
    public class UpgradableTableBaseComponent : MonoBehaviour
    {
        [Inject] private readonly PlayerMoney playerMoney;
        [Inject] private readonly UpgradeCanvasLiaison upgradeCanvasLiaison;
        [Inject] private readonly RepairingAndUpgradingHammer repairingAndUpgradingHammer;

        [SerializeField] private MeshFilter tableMeshFilter; 
        [SerializeField] private UpgradableTableInfo upgradableTableInfo;
        [SerializeField] private ParticleSystem UpgradeTableSuccessPS;

        private IUpgradableTable upgradableTable;

        private void Start()
        {
            upgradableTable = GetComponent<IUpgradableTable>();
        }

        public bool CanPlayerBuyUpgrade(int tableLvl)
        {
            return playerMoney.CurrentPlayerMoney >= upgradableTableInfo.GetUpgradableTablePrice(tableLvl);
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
