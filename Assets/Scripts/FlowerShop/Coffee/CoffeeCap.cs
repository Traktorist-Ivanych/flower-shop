using FlowerShop.PickableObjects.Moving;
using PlayerControl;
using UnityEngine;
using Zenject;

[RequireComponent (typeof(ObjectMoving))]
public class CoffeeCap : MonoBehaviour
{
    [Inject] private readonly PlayerComponents playerComponents;
    
    [SerializeField] private Transform coffeeLiquidTransform;
    [SerializeField] private Transform coffeeLiquidEmptyTransform;
    [SerializeField] private Transform coffeeLiquidFullTransform;

    [HideInInspector, SerializeField] private ObjectMoving objectMoving;
    [HideInInspector, SerializeField] private MeshRenderer coffeeLiquidRenderer;
    
    private Transform startPosition;
    private Transform endPosition;
    private bool isCoffeeLiquidNeedForMoving;
    private bool hideCoffeeLiquidAfterMoving;
    // shouldn't be constant - setting
    private const float CoffeeLiquidMovingTime = 0.5f;
    private float currentCoffeeLiquidMovingTime;

    private void OnValidate()
    {
        objectMoving = GetComponent<ObjectMoving>();
        coffeeLiquidRenderer = coffeeLiquidTransform.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (isCoffeeLiquidNeedForMoving)
        {
            currentCoffeeLiquidMovingTime += Time.deltaTime;
            float lerpT = currentCoffeeLiquidMovingTime / CoffeeLiquidMovingTime;

            coffeeLiquidTransform.position = Vector3.Lerp(startPosition.position, endPosition.position, lerpT);
            coffeeLiquidTransform.localScale = Vector3.Lerp(startPosition.localScale, endPosition.localScale, lerpT);

            if (currentCoffeeLiquidMovingTime >= CoffeeLiquidMovingTime)
            {
                currentCoffeeLiquidMovingTime = 0;
                isCoffeeLiquidNeedForMoving = false;
                if (hideCoffeeLiquidAfterMoving)
                {
                    coffeeLiquidRenderer.enabled = false;
                    hideCoffeeLiquidAfterMoving = false;
                }
            }
        }
    }

    public void FillCoffeeCap()
    {
        coffeeLiquidRenderer.enabled = true;
        isCoffeeLiquidNeedForMoving = true;
        startPosition = coffeeLiquidEmptyTransform;
        endPosition = coffeeLiquidFullTransform;
    }

    public void SetCoffeeCapFull()
    {
        coffeeLiquidRenderer.enabled = true;
        coffeeLiquidTransform.position = coffeeLiquidFullTransform.position;
    }

    public void TakeInPlayerHandsAndKeepPlayerBusy()
    {
        objectMoving.MoveObject(targetFinishTransform: playerComponents.PlayerHandsForCoffeeTransform, 
                                movingObjectAnimatorTrigger: PlayerAnimatorParameters.TakeLittleObjectTrigger, 
                                setPlayerFree: false);
    }

    public void EmptyCoffeeCap()
    {
        isCoffeeLiquidNeedForMoving = true;
        startPosition = coffeeLiquidFullTransform;
        endPosition = coffeeLiquidEmptyTransform;
        hideCoffeeLiquidAfterMoving = true;
    }

    public void PutOnTableAndSetPlayerFree(Transform targetTransform)
    {
        objectMoving.MoveObject(targetFinishTransform: targetTransform, 
                                movingObjectAnimatorTrigger: PlayerAnimatorParameters.GiveLittleObjectTrigger, 
                                setPlayerFree: true);
    }
}
