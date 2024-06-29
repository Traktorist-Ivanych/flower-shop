using FlowerShop.PickableObjects;
using FlowerShop.RepairsAndUpgrades;
using FlowerShop.Tables.BaseComponents;
using FlowerShop.Tables.Interfaces;
using System.Collections;
using UnityEngine;

namespace FlowerShop.Tables.Abstract
{
    [RequireComponent(typeof(BreakableTableBaseComponent))]
    public abstract class UpgradableBreakableTable : UpgradableTable, IBreakableTable
    {
        [HideInInspector, SerializeField] private protected BreakableTableBaseComponent breakableTableBaseComponent;

        public bool IsTableBroken => breakableTableBaseComponent.IsTableBroken;

        private protected override void OnValidate()
        {
            base.OnValidate();

            breakableTableBaseComponent = GetComponent<BreakableTableBaseComponent>();
        }

        public void UseBreakableTable()
        {
            breakableTableBaseComponent.UseBreakableTable();
        }

        private protected void ForciblyBrokenTable()
        {
            breakableTableBaseComponent.BrokenTable();
        }

        public void FixBreakableFlowerTable(int minQuantity, int maxQuantity)
        {
            StartCoroutine(breakableTableBaseComponent.FixBreakableFlowerTable(minQuantity, maxQuantity));
            StartCoroutine(HideBrokenIndicator());
        }

        public void SetActionsBeforeBrokenQuantity(int minQuantity, int maxQuantity)
        {
            breakableTableBaseComponent.SetActionsBeforeBrokenQuantity(minQuantity, maxQuantity);
        }
        public override void ShowIndicator()
        {
            if (IsTableBroken)
            {
                breakableTableBaseComponent.ShowBrokenIndicator();
            }
            else
            {
                base.ShowIndicator();
            }
        }

        public override void HideIndicator()
        {
            base.HideIndicator();

            breakableTableBaseComponent.HideBrokenIndicator();
        }

        private protected bool CanPlayerFixTable()
        {
            return playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer &&
                   IsTableBroken;
        }

        private IEnumerator HideBrokenIndicator() 
        {
            yield return new WaitForSeconds(repairsAndUpgradesSettings.TableRepairTime);
            HideIndicator();

            base.ShowIndicator();
        }

    }
}
