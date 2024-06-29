using FlowerShop.ComputerPages;
using UnityEngine;
using Zenject;

namespace FlowerShop.Education
{
    [RequireComponent(typeof(UIButton))]
    public class EducationContinueButton : MonoBehaviour
    {
        [Inject] private readonly EducationHandler educationHandler;
        
        [HideInInspector, SerializeField] private UIButton uiButton;

        private void OnValidate()
        {
            uiButton = GetComponent<UIButton>();
        }

        private void OnEnable()
        {
            uiButton.OnClickEvent += OnButtonClick;
        }

        private void OnDisable()
        {
            uiButton.OnClickEvent -= OnButtonClick;
        }

        private void OnButtonClick()
        {
            if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
            {
                educationHandler.CompleteEducationStep();
            }
            else if (!educationHandler.IsEducationActive)
            {
                educationHandler.HideEducationDescription();
            }
        }
    }
}