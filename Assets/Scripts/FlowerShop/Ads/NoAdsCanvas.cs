using FlowerShop.Help;
using TMPro;
using UnityEngine;
using Zenject;

namespace FlowerShop.Ads
{
    public class NoAdsCanvas : MonoBehaviour
    {
        [Inject] private readonly HelpTexts helpTexts;

        [SerializeField] private RectTransform adsPanelRectTransform;
        [SerializeField] private TextMeshProUGUI noAdsText;

        [field: SerializeField] public Canvas AdsCanvas { get; private set; }

        public void EnableCanvas()
        {
            AdsCanvas.enabled = true;

            noAdsText.ForceMeshUpdate();

            Vector2 panelRect = adsPanelRectTransform.sizeDelta;
            panelRect.y = noAdsText.renderedHeight + helpTexts.HeightCanvasFieldsForHelpText;
            adsPanelRectTransform.sizeDelta = panelRect;
        }
    }
}
