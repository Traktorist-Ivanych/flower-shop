using System.Collections;
using System.Collections.Generic;
using FlowerShop.FlowersSale;
using FlowerShop.Settings;
using FlowerShop.Tables;
using FlowerShop.Tables.Interfaces;
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
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly CustomersObserver customersObserver;
        [Inject] private readonly CustomersSettings customersSettings;
        [Inject] private readonly CustomersSpawner customersSpawner;
        [Inject] private readonly FlowersSaleTablesForCustomers flowersSaleTablesForCustomers;

        [SerializeField] private Transform waitingTransform;

        [HideInInspector, SerializeField] private CustomerActions customerActions;
        [HideInInspector, SerializeField] private NavMeshAgent navAgent;
        [HideInInspector, SerializeField] private Animator animator;

        private delegate void OnArrive();
        private event OnArrive OnArriveEvent;
        
        private FlowersSaleTable customerFlowersSaleTable;
        private ISpecialSaleTable specialSaleTable;
        private List<Transform> currentPathPoints;
        private Transform currentDestinationTarget;
        private NavMeshPath tempPath;
        private int currentPathPointIndex;
        private bool isCustomerMoveForSpecialSale;

        private void OnValidate()
        {
            customerActions = GetComponent<CustomerActions>();
            navAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        private void Awake()
        {
            customersSpawner.AddBuyerMoving(this);
            navAgent.isStopped = true;
        }

        private void Update()
        {
            PlayWalkOrStayAnimations();

            if (navAgent.isStopped) return;

            TrySetDestinationOrRotateCustomer();
        }

        public void SpawnCustomerForSpecialSale(Transform startTransform, List<Transform> pathPoints, 
            ISpecialSaleTable targetSpecialSaleTable)
        {
            specialSaleTable = targetSpecialSaleTable;
            isCustomerMoveForSpecialSale = true;
            SpawnCustomer(startTransform, pathPoints);
        }
        
        public void SpawnCustomer(Transform startTransform, List<Transform> pathPoints)
        {
            navAgent.Warp(startTransform.position);
            currentPathPoints = pathPoints;
            currentPathPointIndex = 0;
            navAgent.isStopped = false;
            customersObserver.AddActiveCustomer(this);
            customersSpawner.RemoveCustomerMoving(this);
            SetDestination(currentPathPoints[currentPathPointIndex]);
            SetOnArriveEvent(PlayLookAroundAnimation);
        }

        private void PlayWalkOrStayAnimations()
        {
            if (navAgent.velocity == Vector3.zero)
            {
                animator.SetBool(CustomerAnimatorKeys.IsPlayerWalk, false);
            }
            else
            {
                animator.SetBool(CustomerAnimatorKeys.IsPlayerWalk, true);
                
                animator.SetFloat(
                    id: CustomerAnimatorKeys.WalkSpeed, 
                    value: navAgent.velocity.magnitude / customersSettings.NavAgentSpeed);
            }
        }

        private void TrySetDestinationOrRotateCustomer()
        {
            if (currentPathPointIndex < currentPathPoints.Count - 1)
            {
                if (navAgent.remainingDistance <= customersSettings.MinRemainingDistanceBetweenPathPoints)
                {
                    currentPathPointIndex++;
                    SetDestination(currentPathPoints[currentPathPointIndex]);
                }
            }
            else if (navAgent.remainingDistance < customersSettings.RemainingDistanceToStartRotation)
            {
                RotateCustomer();
            }
        }

        private void RotateCustomer()
        {
            transform.rotation = Quaternion.Slerp(
                a: transform.rotation, 
                b: currentDestinationTarget.rotation,
                t: Time.deltaTime * customersSettings.RotationSpeed);

            float currentAngelBetweenCustomerAndRelativeTargetDirection =
                Quaternion.Angle(transform.rotation, currentDestinationTarget.rotation);

            if (currentAngelBetweenCustomerAndRelativeTargetDirection < customersSettings.AngelToStopRotation)
            {
                transform.rotation = currentDestinationTarget.rotation;
                
                InvokeOnCustomerArriveEvent();
            }
        }

        private void SetDestination(Transform destinationTarget)
        {
            currentDestinationTarget = destinationTarget;
            navAgent.destination = currentDestinationTarget.position;
        }

        private void InvokeOnCustomerArriveEvent()
        {
            navAgent.isStopped = true;
            OnArriveEvent?.Invoke();
        }

        private void SetOnArriveEvent(OnArrive onArrive)
        {
            navAgent.isStopped = false;
            OnArriveEvent = null;
            OnArriveEvent += onArrive;
        }

        private void PlayLookAroundAnimation()
        {
            animator.SetTrigger(CustomerAnimatorKeys.LookAround);
        }

        private void LookAroundEndAnimationEvent()
        {
            if (isCustomerMoveForSpecialSale)
            {
                currentPathPoints = specialSaleTable.ToFlowerPathPoints;
                currentPathPointIndex = 0;
                SetDestination(currentPathPoints[currentPathPointIndex]);
                SetOnArriveEvent(PlayThinkAnimation);
            }
            else
            {
                FlowersSaleTable currentFlowersSaleTable = flowersSaleTablesForCustomers.GetSaleTableWithFlower();

                if (customerFlowersSaleTable)
                {
                    flowersSaleTablesForCustomers.AddSaleTableWithFlower(customerFlowersSaleTable);
                }
            
                customerFlowersSaleTable = currentFlowersSaleTable;

                if (customerFlowersSaleTable)
                {
                    currentPathPoints = customerFlowersSaleTable.ToFlowerPathPoints;
                    currentPathPointIndex = 0;
                    SetDestination(currentPathPoints[currentPathPointIndex]);
                    SetOnArriveEvent(PlayThinkAnimation);
                }
                else
                {
                    currentPathPoints = customersSpawner.GetFinishPathPoints();
                    currentPathPointIndex = 0;
                    SetDestination(currentPathPoints[currentPathPointIndex]);
                    SetOnArriveEvent(ResetCustomer);
                }
            }
        }
        
        private void PlayThinkAnimation()
        {
            animator.SetTrigger(CustomerAnimatorKeys.Think);
        }

        private void CustomerThinkEndAnimationEvent()
        {
            if (isCustomerMoveForSpecialSale)
            {
                animator.SetTrigger(CustomerAnimatorKeys.Yes);
            }
            else
            {
                animator.SetTrigger(customersSettings.IsCustomerBuyingFlower()
                    ? CustomerAnimatorKeys.Yes
                    : CustomerAnimatorKeys.No);
            }
        }

        private void CustomerThinkNoEndAnimationEvent()
        {
            currentPathPoints = customerFlowersSaleTable.OutFlowerPathPoints;
            currentPathPoints.Add(customersSpawner.GetNextLookAroundPathPoint());
            currentPathPointIndex = 0;
            SetDestination(currentPathPoints[currentPathPointIndex]);
            SetOnArriveEvent(PlayLookAroundAnimation);
        }

        private void CustomerThinkYesEndAnimationEvent()
        {
            if (isCustomerMoveForSpecialSale)
            {
                isCustomerMoveForSpecialSale = false;
                customerActions.ExecuteSpecialSale(specialSaleTable);
                StartCoroutine(
                    SetBuyerEndTransformWithFlower(specialSaleTable.FinishWithFlowerPathPoints));
            }
            else
            {
                customerActions.BuyFlower(customerFlowersSaleTable);
                StartCoroutine(
                    SetBuyerEndTransformWithFlower(customerFlowersSaleTable.FinishWithFlowerPathPoints));
            }
        }

        private IEnumerator SetBuyerEndTransformWithFlower(List<Transform> finishPathPoints)
        {
            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            
            currentPathPoints = finishPathPoints;
            currentPathPointIndex = 0;
            SetDestination(currentPathPoints[currentPathPointIndex]);
            SetOnArriveEvent(ResetCustomer);
        }

        private void ResetCustomer()
        {
            animator.SetTrigger(CustomerAnimatorKeys.Clear);
            customerActions.ClearFlowerInHands();
            navAgent.Warp(waitingTransform.position);
            navAgent.isStopped = true;
            customersSpawner.AddBuyerMoving(this);
            customersObserver.RemoveInactiveCustomer(this);
        }
    }
}
