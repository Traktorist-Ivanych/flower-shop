using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(BuyerActions))]
public class BuyerMoving : MonoBehaviour
{
    [Inject] private readonly FlowerSaleTablesForByers flowerSaleTablesForByers;
    [Inject] private readonly BuyersSpawner buyersSpawner;
    [Inject] private readonly GameConfiguration gameConfiguration;

    [SerializeField] private Transform waitingTransform;

    private BuyerActions buyerActions;
    private FlowerSaleTable buyerFlowerSaleTable;
    private NavMeshAgent buyerAgent;
    private Animator buyerAnimator;
    private Vector3 targetToLookAt;
    private float rotationSpeed;
    private bool needForRotation;
    private bool isBuyerBusy;

    private void Start()
    {
        buyerActions = GetComponent<BuyerActions>();
        buyerAgent = GetComponent<NavMeshAgent>();
        buyerAnimator = GetComponent<Animator>();
        rotationSpeed = gameConfiguration.PlayerMovingRotation;

        if (!isBuyerBusy)
        {
            buyersSpawner.AddBuyerMoving(this);
        }
    }

    private void Update()
    {
        if (buyerAgent.velocity == Vector3.zero)
        {
            buyerAnimator.SetBool("IsPlayerWalk", false);

            if (buyerAgent.remainingDistance < 0.1f && needForRotation)
            {
                Vector3 relativeTargetDiraction = new(targetToLookAt.x - transform.position.x, 0,
                                                       targetToLookAt.z - transform.position.z);

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativeTargetDiraction),
                                                      Time.deltaTime * rotationSpeed);

                if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(relativeTargetDiraction)) < 2.5f)
                {
                    transform.rotation = Quaternion.LookRotation(relativeTargetDiraction);
                    needForRotation = false;
                    if (isBuyerBusy)
                    {
                        buyerAnimator.SetTrigger("Think");
                    }
                    else
                    {
                        buyerAnimator.SetTrigger("Clear");
                        buyerActions.ClearFlowerInHands();
                        buyerAgent.isStopped = true;
                        transform.position = waitingTransform.position;
                        buyersSpawner.AddBuyerMoving(this);
                    }
                }
            }
        }
        else
        {
            buyerAnimator.SetBool("IsPlayerWalk", true);
            buyerAnimator.SetFloat("WalkSpeed", buyerAgent.velocity.magnitude / gameConfiguration.PlayerNavAgentSpeed);
        }
    }

    public void SetBuyerStartDestination(Transform startTransform, FlowerSaleTable targetFlowerSaleTable)
    {
        buyerFlowerSaleTable = targetFlowerSaleTable;
        transform.SetPositionAndRotation(startTransform.position, startTransform.rotation);
        SetBuyerDestination();
        isBuyerBusy = true;
        buyerAgent.isStopped = false;
        buyersSpawner.RemoveBuyerMoving(this);
    }

    public void BuyerThink()
    {
        if (gameConfiguration.IsByerBuyingFlower())
        {
            buyerAnimator.SetTrigger("No");
        }
        else
        {
            buyerAnimator.SetTrigger("Yes");
        }
    }

    public void BuyerThinkNo()
    {
        FlowerSaleTable nextSaleTable = flowerSaleTablesForByers.GetSaleTableWithFlower();
        flowerSaleTablesForByers.AddSaleTableWithFlower(buyerFlowerSaleTable);

        buyerFlowerSaleTable = nextSaleTable;
        if (buyerFlowerSaleTable != null)
        {
            SetBuyerDestination();
        }
        else
        {
            SetBuyerEndTransform();
        }
    }

    public void BuyerThinkYes()
    {
        buyerActions.BuyFlower(buyerFlowerSaleTable);
        StartCoroutine(SetBuyerEndTransformWithFlower());
    }

    private void SetBuyerDestination()
    {
        buyerAgent.destination = buyerFlowerSaleTable.BuyerDestinationTarget.position;
        targetToLookAt = buyerFlowerSaleTable.TargetToLookAt.position;
        needForRotation = true;
    }

    private void SetBuyerEndTransform()
    {
        buyerAgent.destination = buyersSpawner.GetEndTransform().position;
        needForRotation = true;
        isBuyerBusy = false;
    }

    private IEnumerator SetBuyerEndTransformWithFlower()
    {
        yield return new WaitForSeconds(gameConfiguration.PotMovingActionDelay);
        SetBuyerEndTransform();
    }
}
