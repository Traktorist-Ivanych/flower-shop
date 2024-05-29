using FlowerShop.Flowers;
using Input;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;
using Zenject;

namespace FlowerShop.Help
{
    [Binding]
    public class TableInfoCanvasLiaison : MonoBehaviour, INotifyPropertyChanged
    {
        [Inject] private readonly HelpTexts helpTexts;
        [Inject] private readonly PlayerInputActions playerInputActions;

        [SerializeField] private RectTransform tableInfoDescriptionTextRectTransform;
        [SerializeField] private TextMeshProUGUI tableInfoDescriptionText;

        [SerializeField, HideInInspector] private Canvas tableInfoCanvas;
        [SerializeField, HideInInspector] private ScrollRect tableInfoCanvasScrollRect;

        public event PropertyChangedEventHandler PropertyChanged;

        [Binding]
        public string TableName { get; private set; }

        [Binding]
        public Sprite TableGrowingRoomSprite { get; private set; }

        [Binding]
        public Sprite TableImprovementIndicatorSprite { get; private set; }

        [Binding]
        public Sprite TableBrokenIndicatorSprite { get; private set; }

        private void OnValidate()
        {
            tableInfoCanvas = GetComponent<Canvas>();
            tableInfoCanvasScrollRect = GetComponentInChildren<ScrollRect>();
        }

        public void ShowCanvas(TableInfo tableInfo, GrowingRoom tableGrowingRoom)
        {
            playerInputActions.EnableCanvasControlMode();
            tableInfoCanvas.enabled = true;

            TableName = tableInfo.TableName.GetLocalizedString();
            OnPropertyChanged(nameof(TableName));

            TableGrowingRoomSprite = tableGrowingRoom.RoomColorSprite;
            OnPropertyChanged(nameof(TableGrowingRoomSprite));

            TableImprovementIndicatorSprite = tableInfo.TableImprovementIndicatorSprite;
            OnPropertyChanged(nameof(TableImprovementIndicatorSprite));

            TableBrokenIndicatorSprite = tableInfo.TableBrokenIndicatorSprite;
            OnPropertyChanged(nameof(TableBrokenIndicatorSprite));

            tableInfoDescriptionText.text = tableInfo.TableDescription.GetLocalizedString();
            tableInfoDescriptionText.ForceMeshUpdate();

            Vector2 panelRect = tableInfoDescriptionTextRectTransform.sizeDelta;
            panelRect.y = tableInfoDescriptionText.renderedHeight + helpTexts.HeightCanvasFieldsForTableInfoText;
            tableInfoDescriptionTextRectTransform.sizeDelta = panelRect;

            tableInfoCanvasScrollRect.verticalNormalizedPosition = 1;
        }

        public void HideCanvas()
        {
            playerInputActions.DisableCanvasControlMode();
            tableInfoCanvas.enabled = false;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
