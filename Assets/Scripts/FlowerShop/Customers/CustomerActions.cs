using DG.Tweening;
using FlowerShop.FlowerSales;
using FlowerShop.Settings;
using FlowerShop.Tables;
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

        public void BuyFlower(FlowersSaleTable customerFlowersSaleTable)
        {
            customerSoilTransform.SetPositionAndRotation(
                position: customerFlowersSaleTable.TablePotTransform.position, 
                rotation: customerFlowersSaleTable.TablePotTransform.rotation);

            customerSoilTransform.DOMove(
                endValue: customerHandsTransform.position,
                duration: actionsWithTransformSettings.MovingPickableObjectTime);
            
            customerSoilTransform.DORotate(
                endValue: customerHandsTransform.rotation.eulerAngles,
                duration: actionsWithTransformSettings.MovingPickableObjectTime);
            
            customerSoilRenderer.enabled = true;
            customerFlowerRenderer.enabled = true;
            customerSoilMeshFilter.mesh = customerFlowersSaleTable.SalableSoilMeshFilter.mesh;
            customerFlowerMeshFilter.mesh = customerFlowersSaleTable.SalableFlowerMeshFilter.mesh;
            customerFlowersSaleTable.SaleFlower();
        }

        public void ClearFlowerInHands()
        {
            customerSoilRenderer.enabled = false;
            customerFlowerRenderer.enabled = false;
        }
    }
}
