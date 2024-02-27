using FlowerShop.PickableObjects;
using FlowerShop.RepairsAndUpgrades;
using FlowerShop.Sounds;
using FlowerShop.Tables.BaseComponents;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables.Abstract
{
    [RequireComponent(typeof(UpgradableTableBaseComponent))]
    public abstract class UpgradableTable : Table, IUpgradableTable
    {
        [Inject] private protected readonly RepairsAndUpgradesSettings repairsAndUpgradesSettings;
        [Inject] private readonly PlayerMoney playerMoney;
        [Inject] private readonly RepairsAndUpgradesTable repairsAndUpgradesTable;
        [Inject] private readonly SoundsHandler soundsHandler;

        [HideInInspector, SerializeField] private protected UpgradableTableBaseComponent upgradableTableBaseComponent;
        
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

        public virtual void UpgradeTableFinish()
        {
            playerMoney.TakePlayerMoney(upgradableTableBaseComponent.GetUpgradePrice(tableLvl));
            
            tableLvl++;
            LoadLvlMesh();
            soundsHandler.PlayUpgradeFinishAudio();
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

        private protected bool CanPlayerUpgradeTableForSelectableEffect()
        {
            return upgradableTableBaseComponent.CanPlayerBuyUpgrade(tableLvl) && CanPlayerUpgradeTable();
        }

        private protected bool CanPlayerUpgradeTable()
        {
            return playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer &&
                   tableLvl < repairsAndUpgradesSettings.MaxUpgradableTableLvl;
        }
    }
}
