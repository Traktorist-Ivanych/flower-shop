using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace PlayerControl
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class PlayerMoving : MonoBehaviour
    {
        [Inject] private readonly PlayerControlSettings playerControlSettings;

        [SerializeField] private NavMeshAgent playerAgent;
        [SerializeField] private Animator playerAnimator;
    
        public delegate void PlayerHasArrived();
        public event PlayerHasArrived PlayerHasArrivedEvent;
    
        private Vector3 targetToLookAt;
        private float rotationSpeed;
        private bool needForRotation;

        private void OnValidate()
        {
            playerAgent = GetComponent<NavMeshAgent>();
            playerAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (playerAgent.velocity.magnitude <= playerControlSettings.MinMovableMagnitude)
            {
                playerAnimator.SetBool(PlayerAnimatorParameters.IsPlayerWalkBool, false);

                if (playerAgent.remainingDistance < playerControlSettings.RemainingDistance && needForRotation)
                {
                    Vector3 relativeTargetDirection = new(targetToLookAt.x - transform.position.x, 0,
                        targetToLookAt.z - transform.position.z);

                    transform.rotation = Quaternion.Slerp(
                        transform.rotation, 
                        Quaternion.LookRotation(relativeTargetDirection),
                        Time.deltaTime * rotationSpeed);

                    if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(relativeTargetDirection)) < 2.5f)
                    {
                        transform.rotation = Quaternion.LookRotation(relativeTargetDirection);
                        needForRotation = false;
                        PlayerHasArrivedEvent?.Invoke();
                    }
                }
            }
            else
            {
                playerAnimator.SetBool(PlayerAnimatorParameters.IsPlayerWalkBool, true);
            }
        }

        public void SetPlayerDestination(Vector3 destinationTarget, Vector3 transmittedTargetToLookAt)
        {
            playerAgent.destination = destinationTarget;
            targetToLookAt = transmittedTargetToLookAt;
            needForRotation = true;
        }

        public void SetNotTablePlayerDestination(Vector3 destinationTarget)
        {
            playerAgent.destination = destinationTarget;
            needForRotation = false;
        }

        public void SetCoffeeNavAgentSetting()
        {
            playerAgent.speed = playerControlSettings.PlayerNavAgentCoffeeSpeed;
            playerAgent.angularSpeed = playerControlSettings.PlayerNavAgentCoffeeAngularSpeed;
            playerAgent.acceleration = playerControlSettings.PlayerNavAgentCoffeeAcceleration;
            rotationSpeed = playerControlSettings.PlayerMovingCoffeeRotation;
            SetWalkSpeedAnimatorParameter();
        }

        public void SetOrdinaryNavAgentSetting()
        {
            playerAgent.speed = playerControlSettings.PlayerNavAgentSpeed;
            playerAgent.angularSpeed = playerControlSettings.PlayerNavAgentAngularSpeed;
            playerAgent.acceleration = playerControlSettings.PlayerNavAgentAcceleration;
            rotationSpeed = playerControlSettings.PlayerMovingRotation;
            SetWalkSpeedAnimatorParameter();
        }

        private void SetWalkSpeedAnimatorParameter()
        {
            float currentWalkSpeed = playerAgent.speed / playerControlSettings.PlayerNavAgentSpeed; 
            playerAnimator.SetFloat(PlayerAnimatorParameters.WalkSpeed, currentWalkSpeed);
        }
    }
}
