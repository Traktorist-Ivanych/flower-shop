using UnityEngine;

[RequireComponent (typeof(DinamicObjectMoving))]
public class CoffeCap : MonoBehaviour
{
    [SerializeField] private Transform coffeLiquidTransform;
    [SerializeField] private Transform coffeLiquidEmptyTransform;
    [SerializeField] private Transform coffeLiquidFullTransform;

    private DinamicObjectMoving dinamicObjectMoving;
    private MeshRenderer coffeLiquidRenderer;
    private Transform startPosition;
    private Transform endPosition;
    private bool isCoffeLiquidNeedForMoving;
    private bool hideCoffeLiquidAfterMoving;
    private const float coffeLiquidMovingTime = 0.5f;
    private float currentcoffeLiquidMovingTime;

    public DinamicObjectMoving DinamicObjectMoving
    {
        get => dinamicObjectMoving;
    }

    private void Start()
    {
        dinamicObjectMoving = GetComponent<DinamicObjectMoving>();
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
        dinamicObjectMoving.PutLittleDinamicObjectInPlayerHandsWithRotation();
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
        dinamicObjectMoving.PutLittleDinamicObjectOnTableWithRotation(coffeCapOnTableTransform);
    }
}
