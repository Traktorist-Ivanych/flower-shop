using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ImprovementButton : MonoBehaviour
{
    [Inject] private readonly PlayerMoney playerMoney;
    [Inject] private readonly PlayerBusyness playerBusyness;
    [Inject] private readonly AllCanvasLiaisons allCanvasLiaisons;
    [Inject] private readonly Hammer hammer;

    private Button improvementButton;

    private void Start()
    {
        improvementButton = GetComponent<Button>();
        // when we do some addListner we should removeListener in destroy or etc, if we subscribe to event somewhere we should ALWAYS unsubscibe
        improvementButton.onClick.AddListener(OnImprovementButtonClick);
    }

    public void OnImprovementButtonClick()
    {
        if (allCanvasLiaisons.ImprovementCanvasLiaison.PriceInt <= playerMoney.CurrentPlayerMoney)
        {
            playerMoney.TakePlayerMoney(allCanvasLiaisons.ImprovementCanvasLiaison.PriceInt);
            StartCoroutine(hammer.ImproveTable());
            allCanvasLiaisons.ImprovementCanvas.enabled = false;
        }
        else
        {
            playerBusyness.SetPlayerFree();
        }
    }
}
