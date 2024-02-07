using System.Collections;
using FlowerShop.PickableObjects;
using FlowerShop.Tables.BaseComponents;
using FlowerShop.Tables.Interfaces;
using UnityEngine;

namespace FlowerShop.Tables.Abstract
{
    [RequireComponent (typeof(BreakableTableBaseComponent))]
    public abstract class UpgradableBreakableTable : UpgradableTable, IBreakableTable
    {
        [HideInInspector, SerializeField] private protected BreakableTableBaseComponent breakableTableBaseComponent;

        public bool IsTableBroken => breakableTableBaseComponent.IsTableBroken;

        private protected override void OnValidate()
        {
            base.OnValidate();
            
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
            if (playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer)
            {
                HideUpgradeIndicator();
                ShowBreakdownIndicator();
            }
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
