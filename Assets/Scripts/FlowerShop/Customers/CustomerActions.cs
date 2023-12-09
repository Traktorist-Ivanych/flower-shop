using DG.Tweening;
using FlowerShop.FlowerSales;
using FlowerShop.Settings;
using UnityEngine;
using Zenject;

namespace FlowerShop.Customers
{
    public class CustomerActions : MonoBehaviour
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        
        [SerializeField] private Transform customerHandsTransform;
        [SerializeField] private MeshRenderer customerSoilRenderer;
        [SerializeField] private MeshRenderer customerFlowerRenderer;

        [HideInInspector, SerializeField] private Transform customerSoilTransform;
        [HideInInspector, SerializeField] private MeshFilter customerSoilMeshFilter;
        [HideInInspector, SerializeField] private MeshFilter customerFlowerMeshFilter;

        private void OnValidate()
        {
            customerSoilTransform = customerSoilRenderer.GetComponent<Transform>();
            customerSoilMeshFilter = customerSoilRenderer.GetComponent<MeshFilter>();
            customerFlowerMeshFilter = customerFlowerRenderer.GetComponent<MeshFilter>();
        }

        public void BuyFlower(FlowerSaleTable customerFlowerSaleTable)
        {
            customerSoilTransform.SetPositionAndRotation(
                position: customerFlowerSaleTable.TablePotTransform.position, 
                rotation: customerFlowerSaleTable.TablePotTransform.rotation);

            customerSoilTransform.DOMove(
                endValue: customerHandsTransform.position,
                duration: actionsWithTransformSettings.MovingPickableObjectTime);
            
            customerSoilTransform.DORotate(
                endValue: customerHandsTransform.rotation.eulerAngles,
                duration: actionsWithTransformSettings.MovingPickableObjectTime);
            
            customerSoilRenderer.enabled = true;
            customerFlowerRenderer.enabled = true;
            customerSoilMeshFilter.mesh = customerFlowerSaleTable.SalableSoilMeshFilter.mesh;
            customerFlowerMeshFilter.mesh = customerFlowerSaleTable.SalableFlowerMeshFilter.mesh;
            customerFlowerSaleTable.SaleFlower();
        }

        public void ClearFlowerInHands()
        {
            customerSoilRenderer.enabled = false;
            customerFlowerRenderer.enabled = false;
        }
    }
}
