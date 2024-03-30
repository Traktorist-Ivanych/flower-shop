using FlowerShop.Education;
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
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly PlayerMoney playerMoney;
        [Inject] private readonly UpgradeCanvasLiaison upgradeCanvasLiaison;
        [Inject] private readonly RepairingAndUpgradingHammer repairingAndUpgradingHammer;
        [Inject] private readonly RepairsAndUpgradesSettings repairsAndUpgradesSettings;

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
            if (tableLvl < repairsAndUpgradesSettings.MaxUpgradableTableLvl)
            {
                return playerMoney.CurrentPlayerMoney >= upgradableTableInfo.GetUpgradableTablePrice(tableLvl);
            }

            return false;
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

            if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
            {
                educationHandler.CompleteEducationStep();
            }
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
