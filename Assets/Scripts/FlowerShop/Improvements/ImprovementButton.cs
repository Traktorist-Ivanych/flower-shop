using PlayerControl;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ImprovementButton : MonoBehaviour
{
    [Inject] private readonly PlayerMoney playerMoney;
    [Inject] private readonly ImprovementCanvasLiaison improvementCanvasLiaison;
    [Inject] private readonly Hammer hammer;

    [SerializeField] private Button improvementButton;

    private void OnValidate()
    {
        improvementButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        improvementButton.onClick.AddListener(OnImprovementButtonClick);
    }

    private void OnDisable()
    {
        improvementButton.onClick.RemoveListener(OnImprovementButtonClick);
    }

    private void OnImprovementButtonClick()
    {
        if (improvementCanvasLiaison.PriceInt <= playerMoney.CurrentPlayerMoney)
        {
            playerMoney.TakePlayerMoney(improvementCanvasLiaison.PriceInt);
            StartCoroutine(hammer.ImproveTable());
            improvementCanvasLiaison.ImprovementCanvas.enabled = false;
        }
    }
}
