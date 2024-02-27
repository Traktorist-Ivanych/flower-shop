using FlowerShop.PickableObjects;
using FlowerShop.Tables.BaseComponents;
using FlowerShop.Tables.Interfaces;
using UnityEngine;

namespace FlowerShop.Tables.Abstract
{
    [RequireComponent(typeof(BreakableTableBaseComponent))]
    public abstract class BreakableTable : Table, IBreakableTable
    {
        [HideInInspector, SerializeField] private BreakableTableBaseComponent breakableTableBaseComponent;

        public bool IsTableBroken => breakableTableBaseComponent.IsTableBroken;

        private protected virtual void OnValidate()
        {
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
    }
}
