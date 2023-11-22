using UnityEngine;
using Zenject;

[RequireComponent(typeof(Animator))]
public class FlowerSaleTable : FlowerTable
{
    [Inject] private readonly FlowerSaleTablesForByers flowerSaleTablesForByers;
    [Inject] private readonly FlowersForSaleCoefCalculator flowersForSaleCoefCalculator;
    [Inject] private readonly PlayerComponents playerComponents;
    [Inject] private readonly PlayerMoney playerMoney;

    [SerializeField] private Transform tablePotTransform;
    [SerializeField] private Transform buyerDestinationTarget;
    [SerializeField] private MeshRenderer salableSoilRenderer;
    [SerializeField] private MeshRenderer salableFlowerRenderer;

    private Pot potForSale;
    private Animator saleTableAnimator;
    private Flower flowerForSale;
    private MeshFilter salableSoilMeshFilter;
    private MeshFilter salableFlowerMeshFilter;
    private bool isFlowerOnSaleTable;

    public Transform BuyerDestinationTarget
    {
        get => buyerDestinationTarget;
    }

    public Transform TargetToLookAt
    {
        get => targetToLookAt;
    }

    public Transform TablePotTransform
    {
        get => tablePotTransform;
    }

    public MeshFilter SalableSoilMeshFilter
    {
        get => salableSoilMeshFilter;
    }

    public MeshFilter SalableFlowerMeshFilter
    {
        get => salableFlowerMeshFilter;
    }

    public Flower FlowerForSale
    {
        get => flowerForSale;
    }

    private void Start()
    {
        saleTableAnimator = GetComponent<Animator>();
        salableSoilMeshFilter = salableSoilRenderer.GetComponent<MeshFilter>();
        salableFlowerMeshFilter = salableFlowerRenderer.GetComponent<MeshFilter>();
    }

    public override void ExecuteClickableAbility()
    {
        if (playerBusyness.IsPlayerFree && !isFlowerOnSaleTable && 
            PlayerPickableObjectHandler.GetCurrentPlayerPickableObject() is Pot)
        {
            potForSale = PlayerPickableObjectHandler.GetCurrentPlayerPickableObject() as Pot;
            if (potForSale.FlowerGrowingLvl >= 3 && !potForSale.IsWeedInPot)
            {
                SetPlayerDestination();
            }
        }
    }

    public override void ExecutePlayerAbility()
    {
        flowerForSale = potForSale.PlantedFlower;
        salableSoilRenderer.enabled = true;
        salableSoilMeshFilter.mesh = potForSale.PotObjects.SoilMeshFilter.mesh;
        salableFlowerRenderer.enabled = true;
        salableFlowerMeshFilter.mesh = potForSale.PotObjects.FlowerMeshFilter.mesh;
        potForSale.CleanPot();

        saleTableAnimator.SetTrigger("PutSoilOnTable");
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.ThrowTrigger);

        isFlowerOnSaleTable = true;

        flowersForSaleCoefCalculator.AddFlowerSaleTableWithFLowerInList(this);
    }

    public void SetPlayerFree()
    {
        playerBusyness.SetPlayerFree();
        flowerSaleTablesForByers.AddSaleTableWithFlower(this);
    }

    public void SaleFlower()
    {
        salableSoilRenderer.enabled = false;
        salableFlowerRenderer.enabled = false;
        flowersForSaleCoefCalculator.RemoveFlowerSaleTableWithoutFLowerFromList(this);
        isFlowerOnSaleTable = false;
        playerMoney.AddPlayerMoney(flowerForSale.FlowerSellingPrice);
    }
}
