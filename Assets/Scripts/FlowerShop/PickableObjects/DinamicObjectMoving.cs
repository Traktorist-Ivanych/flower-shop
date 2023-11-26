using PlayerControl;
using UnityEngine;
using Zenject;

public class DinamicObjectMoving : MonoBehaviour
{
    [Inject] private readonly PlayerComponents playerComponents;
    [Inject] private readonly PlayerBusyness playerBusyness;

    private Transform startTransform;
    private Transform finishTransform;
    private float currentMovingTime;
    private bool isDinamicObjectNeedForMoving;
    private bool shouldDinamicObjectRotate;

    public bool ShouldPlayerBecomeFree { get; set; }

    private void Start()
    {
        ShouldPlayerBecomeFree = true;
    }

    private void Update()
    {
        // to dotween animation, remake 'isDinamicObjectNeedForMoving' to property and start animation in set there
        if (isDinamicObjectNeedForMoving)
        {
            currentMovingTime += Time.deltaTime * 2;
            transform.position = Vector3.Lerp(startTransform.position, finishTransform.position, currentMovingTime);
            if (shouldDinamicObjectRotate)
            {
                transform.rotation = Quaternion.Lerp(startTransform.rotation, finishTransform.rotation, currentMovingTime);
            }
            
            if (currentMovingTime >= 1)
            {
                // move to reset method
                currentMovingTime = 0;
                isDinamicObjectNeedForMoving = false;
                transform.SetParent(finishTransform);
                transform.localRotation = Quaternion.Euler(Vector3.zero);

                if (ShouldPlayerBecomeFree)
                {
                    playerBusyness.SetPlayerFree();
                }

                ShouldPlayerBecomeFree = true;
                shouldDinamicObjectRotate = false;
            }
        }
    }

    // replace with settings (object size)
    public void PutLittleDinamicObjectInPlayerHandsWithRotation()
    {
        shouldDinamicObjectRotate = true;
        finishTransform = playerComponents.PlayerHandsForLittleObjectTransform;
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.TakeLittleObjectTrigger);
        MoveDinamicObject();
    }

    public void PutLittleDinamicObjectOnTableWithRotation(Transform tableTransform)
    {
        shouldDinamicObjectRotate = true;
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.GiveLittleObjectTrigger);
        finishTransform = tableTransform;
        MoveDinamicObject();
    }

    public void PutBigDinamicObjectInPlayerHands()
    {
        finishTransform = playerComponents.PlayerHandsTransform;
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.TakeBigObjectTrigger);
        MoveDinamicObject();
    }

    public void PutBigDinamicObjectOnTable(Transform tableTransform)
    {
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.GiveBigObjectTrigger);
        finishTransform = tableTransform;
        MoveDinamicObject();
    }

    private void MoveDinamicObject()
    {
        startTransform = transform;
        isDinamicObjectNeedForMoving = true;
    }
}
