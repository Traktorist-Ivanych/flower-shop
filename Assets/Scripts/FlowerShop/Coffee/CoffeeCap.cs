using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent (typeof(DinamicObjectMoving))]
public class CoffeeCap : MonoBehaviour
{
    [SerializeField] private Transform coffeeLiquidTransform;
    [SerializeField] private Transform coffeeLiquidEmptyTransform;
    [SerializeField] private Transform coffeeLiquidFullTransform;

    private DinamicObjectMoving dynamicObjectMoving;
    private MeshRenderer coffeeLiquidRenderer;
    private Transform startPosition;
    private Transform endPosition;
    private bool isCoffeeLiquidNeedForMoving;
    private bool hideCoffeeLiquidAfterMoving;
    // shouldn't be constant - setting
    private const float CoffeeLiquidMovingTime = 0.5f;
    private float currentCoffeeLiquidMovingTime;

    public DinamicObjectMoving DynamicObjectMoving
    {
        get => dynamicObjectMoving;
    }

    private void Start()
    {
        dynamicObjectMoving = GetComponent<DinamicObjectMoving>();
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

    public void FillCoffeCap()
    {
        coffeeLiquidRenderer.enabled = true;
        isCoffeeLiquidNeedForMoving = true;
        startPosition = coffeeLiquidEmptyTransform;
        endPosition = coffeeLiquidFullTransform;
    }

    public void SetCoffeCapFull()
    {
        coffeeLiquidRenderer.enabled = true;
        coffeeLiquidTransform.position = coffeeLiquidFullTransform.position;
    }

    public void TakeCoffeCapInPlayerHands()
    {
        dynamicObjectMoving.PutLittleDinamicObjectInPlayerHandsWithRotation();
    }

    public void EmptyCoffeCap()
    {
        isCoffeeLiquidNeedForMoving = true;
        startPosition = coffeeLiquidFullTransform;
        endPosition = coffeeLiquidEmptyTransform;
        hideCoffeeLiquidAfterMoving = true;
    }

    public void PutCoffeCapOnTable(Transform coffeCapOnTableTransform)
    {
        dynamicObjectMoving.PutLittleDinamicObjectOnTableWithRotation(coffeCapOnTableTransform);
    }
}
