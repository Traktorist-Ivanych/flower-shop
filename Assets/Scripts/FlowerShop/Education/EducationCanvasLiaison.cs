using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

namespace FlowerShop.Education
{
    public class EducationCanvasLiaison : MonoBehaviour
    {
        [Inject] private readonly EducationSettings educationSettings;

        [SerializeField] private RectTransform educationPanelRectTransform;
        [SerializeField] private TextMeshProUGUI educationText;

        [field: SerializeField] public Canvas EducationCanvas { get; private set; }

        public void SetEducationText(LocalizedString localizedText)
        {
            educationText.text = localizedText.GetLocalizedString();
            educationText.ForceMeshUpdate();

            Vector2 panelRect = educationPanelRectTransform.sizeDelta;
            panelRect.y = educationText.renderedHeight + educationSettings.HeightCanvasFieldsForEducationText;
            educationPanelRectTransform.sizeDelta = panelRect;
        }
    }
}