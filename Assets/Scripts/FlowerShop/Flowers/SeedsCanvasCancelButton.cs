using FlowerShop.ComputerPages;
using FlowerShop.Education;
using FlowerShop.Tables;
using Input;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(UIButton))]
public class SeedsCanvasCancelButton : MonoBehaviour
{
    [Inject] private readonly EducationHandler educationHandler;
    [Inject] private readonly PlayerInputActions playerInputActions;

    [SerializeField] private PlantingSeedsTable plantingSeedsTable;

    [HideInInspector, SerializeField] private UIButton button;

    private void OnValidate()
    {
        button = GetComponent<UIButton>();
    }

    private void OnEnable()
    {
        button.OnClickEvent += OnButtonClick;
    }

    private void OnDisable()
    {
        button.OnClickEvent -= OnButtonClick;
    }

    private void OnButtonClick()
    {
        if (!educationHandler.IsEducationActive)
        {
            playerInputActions.DisableCanvasControlMode();
            plantingSeedsTable.CancelPlantingSeed();
        }
    }
}
