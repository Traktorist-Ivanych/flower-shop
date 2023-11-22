using UnityEngine;

[RequireComponent (typeof(DinamicObjectMoving))]
public class CoffeCap : MonoBehaviour
{
    [SerializeField] private Transform coffeLiquidTransform;
    [SerializeField] private Transform coffeLiquidEmptyTransform;
    [SerializeField] private Transform coffeLiquidFullTransform;

    private DinamicObjectMoving dynamicObjectMoving;
    // correct CoffeE
    private MeshRenderer coffeLiquidRenderer;
    private Transform startPosition;
    private Transform endPosition;
    private bool isCoffeLiquidNeedForMoving;
    private bool hideCoffeLiquidAfterMoving;
    // shouldn't be constant - setting
    private const float coffeLiquidMovingTime = 0.5f;
    private float currentcoffeLiquidMovingTime;

    public DinamicObjectMoving DynamicObjectMoving
    {
        get => dynamicObjectMoving;
    }

    private void Start()
    {
        dynamicObjectMoving = GetComponent<DinamicObjectMoving>();
        coffeLiquidRenderer = coffeLiquidTransform.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (isCoffeLiquidNeedForMoving)
        {
            currentcoffeLiquidMovingTime += Time.deltaTime;
            float lerpT = currentcoffeLiquidMovingTime / coffeLiquidMovingTime;

            coffeLiquidTransform.position = Vector3.Lerp(startPosition.position, endPosition.position, lerpT);
            coffeLiquidTransform.localScale = Vector3.Lerp(startPosition.localScale, endPosition.localScale, lerpT);

            if (currentcoffeLiquidMovingTime >= coffeLiquidMovingTime)
            {
                currentcoffeLiquidMovingTime = 0;
                isCoffeLiquidNeedForMoving = false;
                if (hideCoffeLiquidAfterMoving)
                {
                    coffeLiquidRenderer.enabled = false;
                    hideCoffeLiquidAfterMoving = false;
                }
            }
        }
    }

    public void FillCoffeCap()
    {
        coffeLiquidRenderer.enabled = true;
        isCoffeLiquidNeedForMoving = true;
        startPosition = coffeLiquidEmptyTransform;
        endPosition = coffeLiquidFullTransform;
    }

    public void SetCoffeCapFull()
    {
        coffeLiquidRenderer.enabled = true;
        coffeLiquidTransform.position = coffeLiquidFullTransform.position;
    }

    public void TakeCoffeCapInPlayerHands()
    {
        dynamicObjectMoving.PutLittleDinamicObjectInPlayerHandsWithRotation();
    }

    public void EmptyCoffeCap()
    {
        isCoffeLiquidNeedForMoving = true;
        startPosition = coffeLiquidFullTransform;
        endPosition = coffeLiquidEmptyTransform;
        hideCoffeLiquidAfterMoving = true;
    }

    public void PutCoffeCapOnTable(Transform coffeCapOnTableTransform)
    {
        dynamicObjectMoving.PutLittleDinamicObjectOnTableWithRotation(coffeCapOnTableTransform);
    }
}
