using FlowerShop.ComputerPages;
using FlowerShop.Education;
using FlowerShop.Help;
using FlowerShop.PickableObjects;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.FlowersSale
{
    public class OpenRatingInfoHelpCanvasButton : MonoBehaviour
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly HelpCanvasLiaison helpCanvasLiaison;
        [Inject] private readonly HelpTexts helpTexts;
        [Inject] private readonly PlayerBusyness playerBusyness;
        [Inject] private readonly PlayerPickableObjectHandler playerPickableObjectHandler;

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
            if (educationHandler.IsEducationActive)
            {
                if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
                {
                    if (playerBusyness.IsPlayerFree && playerPickableObjectHandler.CurrentPickableObject is InfoBook)
                    {
                        helpCanvasLiaison.EnableCanvasAndSetHelpTextForcibly(helpTexts.RatingHelpText);
                        educationHandler.CompleteEducationStep();
                    }
                }
            }
            else
            {
                if (playerBusyness.IsPlayerFree && playerPickableObjectHandler.CurrentPickableObject is InfoBook)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpTextForcibly(helpTexts.RatingHelpText);
                }
            }
        }
    }
}
