using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class PlayerMoving : MonoBehaviour
{
    [Inject] private readonly GameConfiguration gameConfiguration;

    public delegate void PlayerHasArrived();
    public event PlayerHasArrived PlayerHasArrivedEvent;

    private NavMeshAgent playerAgent;
    private Animator playerAnimator;
    private Vector3 targetToLookAt;
    private float rotationSpeed;
    private bool needForRotation;

    private void Start()
    {
        playerAgent = GetComponent<NavMeshAgent>();
        playerAgent.speed = gameConfiguration.PlayerNavAgentSpeed;
        playerAgent.angularSpeed = gameConfiguration.PlayerNavAgentAngularSpeed;
        playerAgent.acceleration = gameConfiguration.PlayerNavAgentAcceleration;

        playerAnimator = GetComponent<Animator>();

        SetOrdinaryRotationSpeed();
    }

    private void Update()
    {
        if (playerAgent.velocity == Vector3.zero)
        {
            playerAnimator.SetBool("IsPlayerWalk", false);

            if (playerAgent.remainingDistance < 0.1f && needForRotation)
            {
                Vector3 relativeTargetDiraction = new(targetToLookAt.x - transform.position.x, 0,
                                                       targetToLookAt.z - transform.position.z);

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativeTargetDiraction),
                                                      Time.deltaTime * rotationSpeed);

                if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(relativeTargetDiraction)) < 2.5f)
                {
                    transform.rotation = Quaternion.LookRotation(relativeTargetDiraction);
                    needForRotation = false;
                    PlayerHasArrivedEvent?.Invoke();
                }
            }
        }
        else
        {
            playerAnimator.SetBool("IsPlayerWalk", true);
        }
    }

    public void SetPlayerDestination(Vector3 destinationTarget, Vector3 TargetToLookAt)
    {
        playerAgent.destination = destinationTarget;
        targetToLookAt = TargetToLookAt;
        needForRotation = true;
    }

    public void SetCoffeRotationSpeed()
    {
        rotationSpeed = gameConfiguration.PlayerMovingCoffeRotation;
    }

    public void SetOrdinaryRotationSpeed()
    {
        rotationSpeed = gameConfiguration.PlayerMovingRotation;
    }
}
