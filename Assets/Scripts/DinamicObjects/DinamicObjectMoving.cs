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
    private bool shouldPlayerBecomeFree;
    private bool shouldDinamicObjectRotate;

    public bool ShouldPlayerBecomeFree
    {
        get { return shouldPlayerBecomeFree; }
        set { shouldPlayerBecomeFree = value; }
    }

    private void Start()
    {
        ShouldPlayerBecomeFree = true;
    }

    private void Update()
    {
        if (isDinamicObjectNeedForMoving)
        {
            currentMovingTime += Time.deltaTime * 2;
            transform.position = Vector3.Lerp(startTransform.position, finishTransform.position, currentMovingTime);
            if (shouldDinamicObjectRotate)
            {
                transform.rotation = Quaternion.Lerp(startTransform.rotation, finishTransform.rotation, currentMovingTime);
            }

            if (currentMovingTime > 1)
            {
                currentMovingTime = 0;
                isDinamicObjectNeedForMoving = false;
                transform.SetParent(finishTransform);
                transform.localRotation = Quaternion.Euler(Vector3.zero);

                if (shouldPlayerBecomeFree)
                {
                    playerBusyness.SetPlayerFree();
                }

                shouldPlayerBecomeFree = true;
                shouldDinamicObjectRotate = false;
            }
        }
    }

    public void PutLittleDinamicObjectInPlayerHandsWithRotation()
    {
        shouldDinamicObjectRotate = true;
        finishTransform = playerComponents.PlayerHandsForLittleObjectTransform;
        playerComponents.PlayerAnimator.SetTrigger("TakeLittleObject");
        MoveDinamicObject();
    }

    public void PutLittleDinamicObjectOnTableWithRotation(Transform tableTransform)
    {
        shouldDinamicObjectRotate = true;
        playerComponents.PlayerAnimator.SetTrigger("GiveLittleObject");
        finishTransform = tableTransform;
        MoveDinamicObject();
    }

    public void PutBigDinamicObjectInPlayerHands()
    {
        finishTransform = playerComponents.PlayerHandsTransform;
        playerComponents.PlayerAnimator.SetTrigger("Take");
        MoveDinamicObject();
    }

    public void PutBigDinamicObjectOnTable(Transform tableTransform)
    {
        playerComponents.PlayerAnimator.SetTrigger("Give");
        finishTransform = tableTransform;
        MoveDinamicObject();
    }

    private void MoveDinamicObject()
    {
        startTransform = transform;
        isDinamicObjectNeedForMoving = true;
    }
}
