using System.Collections.Generic;
using UnityEngine;

namespace FlowerShop.Tables.Interfaces
{
    public interface ISpecialSaleTable
    {
        public List<Transform> ToFlowerPathPoints { get; }
        public List<Transform> FinishWithFlowerPathPoints { get; }
        public Transform TablePotTransform { get; }
        public MeshFilter SoilMeshFilter { get; }
        public MeshFilter FlowerMeshFilter { get; }

        public void ExecuteSpecialSale();
    }
}