using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ImprovementButton : MonoBehaviour
{
    [Inject] private readonly PlayerMoney playerMoney;
    [Inject] private readonly PlayerBusyness playerBusyness;
    [Inject] private readonly ModelViewAll modelViewAll;
    [Inject] private readonly Hammer hammer;

    private Button improvementButton;

    private void Start()
    {
        improvementButton = GetComponent<Button>();
        improvementButton.onClick.AddListener(OnImprovementButtonClick);
    }

    public void OnImprovementButtonClick()
    {
        if (modelViewAll.ImprovementModelView.PriceInt <= playerMoney.CurrentPlayerMoney)
        {
            playerMoney.TakePlayerMoney(modelViewAll.ImprovementModelView.PriceInt);
            StartCoroutine(hammer.ImproveTable());
            modelViewAll.ImprovementCanvas.enabled = false;
        }
        else
        {
            playerBusyness.SetPlayerFree();
        }
    }
}
