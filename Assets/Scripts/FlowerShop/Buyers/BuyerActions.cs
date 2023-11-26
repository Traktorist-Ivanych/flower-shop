using UnityEngine;

[RequireComponent (typeof(Animator))]
public class BuyerActions : MonoBehaviour
{
    [SerializeField] private Transform buyerHandsTransform;
    [SerializeField] private Transform buyerSoilTransform;
    [Tooltip("")]
    [SerializeField] private MeshRenderer buyerSoilRenderer;
    [SerializeField] private MeshRenderer buyerFlowerRenderer;

    private MeshFilter buyerSoilMeshFilter;
    private MeshFilter buyerFlowerMeshFilter;
    private Transform startSoilPosition;
    private float currentMovingTime;
    private bool isSoilNeedForMoving;

    private void Start()
    {
        buyerSoilMeshFilter = buyerSoilRenderer.GetComponent<MeshFilter>();
        buyerFlowerMeshFilter = buyerFlowerRenderer.GetComponent<MeshFilter>();
    }

    private void Update()
    {
        if (isSoilNeedForMoving)
        {
            currentMovingTime += Time.deltaTime * 3.5f;
            buyerSoilTransform.SetPositionAndRotation(
                Vector3.Lerp(startSoilPosition.position, buyerHandsTransform.position, currentMovingTime), 
                Quaternion.Slerp(startSoilPosition.rotation, buyerHandsTransform.rotation, currentMovingTime));

            if (currentMovingTime >= 1)
            {
                isSoilNeedForMoving = false;
                currentMovingTime = 0;
            }
        }
    }

    public void BuyFlower(FlowerSaleTable buyerFlowerSaleTable)
    {
        startSoilPosition = buyerFlowerSaleTable.TablePotTransform;
        buyerSoilTransform.SetPositionAndRotation(startSoilPosition.position, startSoilPosition.rotation);
        isSoilNeedForMoving = true;
        buyerSoilRenderer.enabled = true;
        buyerFlowerRenderer.enabled = true;
        buyerSoilMeshFilter.mesh = buyerFlowerSaleTable.SalableSoilMeshFilter.mesh;
        buyerFlowerMeshFilter.mesh = buyerFlowerSaleTable.SalableFlowerMeshFilter.mesh;
        buyerFlowerSaleTable.SaleFlower();
    }

    public void ClearFlowerInHands()
    {
        buyerSoilRenderer.enabled = false;
        buyerFlowerRenderer.enabled = false;
    }
}
