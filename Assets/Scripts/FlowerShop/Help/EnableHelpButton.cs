using FlowerShop.ComputerPages;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.Help
{
    [RequireComponent(typeof(UIButton))]
    public class EnableHelpButton : MonoBehaviour
    {
        [Inject] private readonly HelpCanvasLiaison helpCanvasLiaison;
        [Inject] private readonly HelpTexts helpTexts;

        [SerializeField] private Image helpIndicatorImage;

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

        private void Start()
        {
            if (helpCanvasLiaison.ShouldHelpCanvasDisplay)
            {
                helpIndicatorImage.sprite = helpTexts.EnableSprite;
            }
        }

        private void OnButtonClick()
        {
            if (helpCanvasLiaison.ShouldHelpCanvasDisplay)
            {
                helpCanvasLiaison.DisableHelpCanvasDisplaying();
                helpIndicatorImage.sprite = helpTexts.DisableSprite;
            }
            else
            {
                helpCanvasLiaison.EnableHelpCanvasDisplaying();
                helpIndicatorImage.sprite = helpTexts.EnableSprite;
            }
        }
    }
}
