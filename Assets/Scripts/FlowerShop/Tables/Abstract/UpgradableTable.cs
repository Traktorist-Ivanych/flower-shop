using FlowerShop.RepairsAndUpgrades;
using FlowerShop.Tables.BaseComponents;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables.Abstract
{
    [RequireComponent(typeof(UpgradableTableBaseComponent))]
    public abstract class UpgradableTable : Table, IUpgradableTable
    {
        [Inject] private readonly PlayerMoney playerMoney;
        [Inject] private protected readonly RepairsAndUpgradesSettings repairsAndUpgradesSettings;
        [Inject] private readonly RepairsAndUpgradesTable repairsAndUpgradesTable;

        [HideInInspector, SerializeField] private UpgradableTableBaseComponent upgradableTableBaseComponent;
        
        private protected int tableLvl;

        private protected virtual void OnValidate()
        {
            upgradableTableBaseComponent = GetComponent<UpgradableTableBaseComponent>();
        }
        
        private protected virtual void Awake()
        {
            AddUpgradableTableToList();
        }

        public void AddUpgradableTableToList()
        {
            repairsAndUpgradesTable.AddUpgradableTableToList(this);
        }

        public virtual void HideUpgradeIndicator()
        {
            upgradableTableBaseComponent.HideUpgradeIndicator();
        }

        public void UpgradeTableStart()
        {
            upgradableTableBaseComponent.StartUpgradeTableProcessEffects();
        }

        public virtual void UpgradeTableFinish()
        {
            playerMoney.TakePlayerMoney(upgradableTableBaseComponent.GetUpgradePrice(tableLvl));
            
            tableLvl++;
            LoadLvlMesh();
            ShowUpgradeIndicator();
            upgradableTableBaseComponent.FinishUpgradeTableProcessEffects();
        }

        private protected void LoadLvlMesh()
        {
            upgradableTableBaseComponent.SetNextLvlMesh(tableLvl);
        }

        public void ShowUpgradeCanvas()
        {
            upgradableTableBaseComponent.SetUpgradableTableInfoToCanvas(tableLvl);
        }

        public virtual void ShowUpgradeIndicator()
        {
            if (tableLvl < repairsAndUpgradesSettings.MaxUpgradableTableLvl)
            {
                upgradableTableBaseComponent.ShowUpgradeIndicator();
            }
            else
            {
                upgradableTableBaseComponent.HideUpgradeIndicator();
            }
        }
    }
}
