using System.Collections;
using FlowerShop.RepairsAndUpgrades;
using FlowerShop.Tables.BaseComponents;
using FlowerShop.Tables.Interfaces;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables.Abstract
{
    [RequireComponent (typeof(BreakableTableBaseComponent))]
    public abstract class UpgradableBreakableTable : UpgradableTable, IBreakableTable
    {
        [Inject] private protected readonly RepairsAndUpgradesSettings repairsAndUpgradesSettings;

        [HideInInspector, SerializeField] private BreakableTableBaseComponent breakableTableBaseComponent;

        public bool IsTableBroken => breakableTableBaseComponent.IsTableBroken;

        private void OnValidate()
        {
            breakableTableBaseComponent = GetComponent<BreakableTableBaseComponent>();
        }

        public override void ShowUpgradeIndicator()
        {
            if (IsTableBroken)
            {
                ShowBreakdownIndicator();
            }
            else
            {
                base.ShowUpgradeIndicator();
            }
        }

        public override void HideUpgradeIndicator()
        {
            if (IsTableBroken) 
            {
                HideBreakdownIndicator();
            }
            else
            {
                base.HideUpgradeIndicator();
            }
        }

        public void ShowBreakdownIndicator()
        {
            breakableTableBaseComponent.ShowBreakdownIndicator();
        }

        public void HideBreakdownIndicator()
        {
            breakableTableBaseComponent.HideBreakdownIndicator();
        }

        public void UseBreakableTable()
        {
            breakableTableBaseComponent.UseBreakableTable();
        }

        public void FixBreakableFlowerTable(int minQuantity, int maxQuantity)
        {
            breakableTableBaseComponent.FixBreakableFlowerTable(minQuantity, maxQuantity);
            StartCoroutine(ShowImprovableIndicatorAfterRepair());
        }

        public void SetActionsBeforeBrokenQuantity(int minQuantity, int maxQuantity)
        {
            breakableTableBaseComponent.SetActionsBeforeBrokenQuantity(minQuantity, maxQuantity);
        }

        private IEnumerator ShowImprovableIndicatorAfterRepair()
        {
            yield return new WaitForSeconds(repairsAndUpgradesSettings.TableRepairTime);
            base.ShowUpgradeIndicator();
        }
    }
}
