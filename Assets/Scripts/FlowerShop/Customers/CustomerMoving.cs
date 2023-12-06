using System.Collections;
using FlowerShop.Settings;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace FlowerShop.Customers
{
    [RequireComponent(typeof(CustomerActions))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class CustomerMoving : MonoBehaviour
    {
        [Inject] private readonly FlowerSaleTablesForCustomers flowerSaleTablesForCustomers;
        [Inject] private readonly CustomersSpawner customersSpawner;
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly CustomersSettings customersSettings;

        [SerializeField] private Transform waitingTransform;

        [HideInInspector, SerializeField] private CustomerActions customerActions;
        [HideInInspector, SerializeField] private NavMeshAgent buyerAgent;
        [HideInInspector, SerializeField] private Animator buyerAnimator;
        
        private FlowerSaleTable customerFlowerSaleTable;
        private Vector3 targetToLookAt;
        private bool needForRotation;
        private bool isCustomerBusy;

        private void OnValidate()
        {
            customerActions = GetComponent<CustomerActions>();
            buyerAgent = GetComponent<NavMeshAgent>();
            buyerAnimator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (!isCustomerBusy)
            {
                customersSpawner.AddBuyerMoving(this);
            }
        }

        private void Update()
        {
            if (buyerAgent.velocity == Vector3.zero)
            {
                buyerAnimator.SetBool(CustomerAnimatorKeys.IsPlayerWalk, false);

                if (buyerAgent.remainingDistance < customersSettings.RemainingDistanceToStartRotation && needForRotation)
                {
                    RotateCustomer();
                }
            }
            else
            {
                buyerAnimator.SetBool(CustomerAnimatorKeys.IsPlayerWalk, true);
                
                buyerAnimator.SetFloat(
                    id: CustomerAnimatorKeys.WalkSpeed, 
                    value: buyerAgent.velocity.magnitude / customersSettings.NavAgentSpeed);
            }
        }

        public void SetBuyerStartDestination(Transform startTransform, FlowerSaleTable targetFlowerSaleTable)
        {
            customerFlowerSaleTable = targetFlowerSaleTable;
            transform.SetPositionAndRotation(startTransform.position, startTransform.rotation);
            SetBuyerDestination();
            isCustomerBusy = true;
            buyerAgent.isStopped = false;
            customersSpawner.RemoveBuyerMoving(this);
        }

        public void CustomerThink()
        {
            buyerAnimator.SetTrigger(customersSettings.IsCustomerBuyingFlower()
                ? CustomerAnimatorKeys.Yes
                : CustomerAnimatorKeys.No);
        }

        public void CustomerThinkNo()
        {
            FlowerSaleTable nextSaleTable = flowerSaleTablesForCustomers.GetSaleTableWithFlower();
            flowerSaleTablesForCustomers.AddSaleTableWithFlower(customerFlowerSaleTable);

            customerFlowerSaleTable = nextSaleTable;
            
            if (customerFlowerSaleTable == null)
            {
                SetBuyerEndTransform();
            }
            else
            {
                SetBuyerDestination();
            }
        }

        public void CustomerThinkYes()
        {
            customerActions.BuyFlower(customerFlowerSaleTable);
            StartCoroutine(SetBuyerEndTransformWithFlower());
        }

        private void RotateCustomer()
        {
            Vector3 relativeTargetDirection = new(
                x: targetToLookAt.x - transform.position.x, 
                y: 0,
                z: targetToLookAt.z - transform.position.z);

            transform.rotation = Quaternion.Slerp(
                a: transform.rotation, 
                b: Quaternion.LookRotation(relativeTargetDirection),
                t: Time.deltaTime * customersSettings.RotationSpeed);

            float currentAngelBetweenCustomerAndRelativeTargetDirection =
                Quaternion.Angle(transform.rotation, Quaternion.LookRotation(relativeTargetDirection));

            if (currentAngelBetweenCustomerAndRelativeTargetDirection < customersSettings.AngelToStopRotation)
            {
                transform.rotation = Quaternion.LookRotation(relativeTargetDirection);
                needForRotation = false;
                if (isCustomerBusy)
                {
                    buyerAnimator.SetTrigger(CustomerAnimatorKeys.Think);
                }
                else
                {
                    buyerAnimator.SetTrigger(CustomerAnimatorKeys.Clear);
                    customerActions.ClearFlowerInHands();
                    buyerAgent.isStopped = true;
                    transform.position = waitingTransform.position;
                    customersSpawner.AddBuyerMoving(this);
                }
            }
        }

        private void SetBuyerDestination()
        {
            buyerAgent.destination = customerFlowerSaleTable.CustomerDestinationTarget.position;
            targetToLookAt = customerFlowerSaleTable.TargetToLookAt.position;
            needForRotation = true;
        }

        private IEnumerator SetBuyerEndTransformWithFlower()
        {
            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            SetBuyerEndTransform();
        }

        private void SetBuyerEndTransform()
        {
            buyerAgent.destination = customersSpawner.GetEndTransform().position;
            needForRotation = true;
            isCustomerBusy = false;
        }
    }
}
