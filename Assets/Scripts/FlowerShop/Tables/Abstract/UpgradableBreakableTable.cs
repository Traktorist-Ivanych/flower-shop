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

        public void UseBreakableTable()
        {
            breakableTableBaseComponent.UseBreakableTable();
        }

        public void FixBreakableFlowerTable(int minQuantity, int maxQuantity)
        {
            breakableTableBaseComponent.FixBreakableFlowerTable(minQuantity, maxQuantity);
        }

        public void SetActionsBeforeBrokenQuantity(int minQuantity, int maxQuantity)
        {
            breakableTableBaseComponent.SetActionsBeforeBrokenQuantity(minQuantity, maxQuantity);
        }

        private protected bool CanPlayerFixTable()
        {
            return playerPickableObjectHandler.CurrentPickableObject is RepairingAndUpgradingHammer &&
                   IsTableBroken;
        }
    }
}
