using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityWeld.Binding;

namespace FlowerShop.Education
{
    [Binding]
    public class EducationCanvasLiaison : MonoBehaviour, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        [SerializeField] private RectTransform educationPanelRectTransform;

        [Binding]
        [field: TextArea] public string EducationText { get; private set; }
        
        [field: SerializeField] public Canvas EducationCanvas { get; private set; }

        public void SetEducationText(string text, Vector2 textPanelCoordinates)
        {
            Vector2 panelPosition = educationPanelRectTransform.anchoredPosition;
            panelPosition.y = textPanelCoordinates.x;
            educationPanelRectTransform.anchoredPosition = panelPosition;
            Vector2 panelRect = educationPanelRectTransform.sizeDelta;
            panelRect.y = textPanelCoordinates.y;
            educationPanelRectTransform.sizeDelta = panelRect;
            
            EducationText = text;
            OnPropertyChanged(nameof(EducationText));
        }
        
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}