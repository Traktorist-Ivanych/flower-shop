using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Buyers
{
    [RequireComponent(typeof(BuyerActions))]
    public class BuyerMoving : MonoBehaviour
    {
        private FlowerSaleTablesForByers flowerSaleTablesForByers;
        private BuyersSpawner buyersSpawner;
        private GameConfiguration gameConfiguration;

        [Inject] 
        private void Construct(GameConfiguration gameConfiguration, BuyersSpawner buyersSpawner, FlowerSaleTablesForByers flowerSaleTablesForByers)
        {
            this.flowerSaleTablesForByers = flowerSaleTablesForByers;
            this.buyersSpawner = buyersSpawner;
            this.gameConfiguration = gameConfiguration;
        }

        [SerializeField] private Transform waitingTransform;

        private BuyerActions buyerActions;
        private FlowerSaleTable buyerFlowerSaleTable;
        private NavMeshAgent buyerAgent;
        private Animator buyerAnimator;
        private Vector3 targetToLookAt;
        private float rotationSpeed;
        private bool needForRotation;
        private bool isBuyerBusy;

        private static class AnimatorKeys
        {
            public static readonly int IsPlayerWalk = Animator.StringToHash("IsPlayerWalk");
            public static readonly int Think = Animator.StringToHash("Think");
            public static readonly int Clear = Animator.StringToHash("Clear");
            public static readonly int WalkSpeed = Animator.StringToHash("WalkSpeed");
            public static readonly int No = Animator.StringToHash("No");
            public static readonly int Yes = Animator.StringToHash("Yes");
        }

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
                buyerAnimator.SetBool(AnimatorKeys.IsPlayerWalk, false);

                // Move to navmesh settings (0.1f)
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
                            buyerAnimator.SetTrigger(AnimatorKeys.Think);
                        }
                        else
                        {
                            buyerAnimator.SetTrigger(AnimatorKeys.Clear);
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
                buyerAnimator.SetBool(AnimatorKeys.IsPlayerWalk, true);
                buyerAnimator.SetFloat(AnimatorKeys.WalkSpeed, buyerAgent.velocity.magnitude / gameConfiguration.PlayerNavAgentSpeed);
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
                buyerAnimator.SetTrigger(AnimatorKeys.No);
            }
            else
            {
                buyerAnimator.SetTrigger(AnimatorKeys.Yes);
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
}
