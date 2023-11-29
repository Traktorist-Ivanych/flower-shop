using FlowerShop.PickableObjects.Moving;
using UnityEngine;

[RequireComponent (typeof(LittlePickableObjectMoving))]
public class CoffeeCap : MonoBehaviour
{
    [SerializeField] private Transform coffeeLiquidTransform;
    [SerializeField] private Transform coffeeLiquidEmptyTransform;
    [SerializeField] private Transform coffeeLiquidFullTransform;
    [HideInInspector, SerializeField] private LittlePickableObjectMoving littlePickableObjectMoving;
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
        littlePickableObjectMoving = GetComponent<LittlePickableObjectMoving>();
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

    public void TakeCoffeeCapInPlayerHands()
    {
        littlePickableObjectMoving.TakeLittlePickableObjectInPlayerHandsAndKeepPlayerBusy();
    }

    public void EmptyCoffeeCap()
    {
        isCoffeeLiquidNeedForMoving = true;
        startPosition = coffeeLiquidFullTransform;
        endPosition = coffeeLiquidEmptyTransform;
        hideCoffeeLiquidAfterMoving = true;
    }

    public void PutCoffeeCapOnTable(Transform coffeeCapOnTableTransform)
    {
        littlePickableObjectMoving.PutLittlePickableObjectOnTable(coffeeCapOnTableTransform);
    }
}
