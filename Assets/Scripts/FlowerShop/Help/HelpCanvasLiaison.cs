using FlowerShop.Saves.SaveData;
using Input;
using Saves;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

namespace FlowerShop.Help
{
    public class HelpCanvasLiaison : MonoBehaviour, ISavableObject
    {
        [Inject] private readonly HelpTexts helpTexts;
        [Inject] private readonly PlayerInputActions playerInputActions;

        [SerializeField] private RectTransform helpPanelRectTransform;
        [SerializeField] private TextMeshProUGUI helpText;

        [field: SerializeField] public Canvas HelpCanvas { get; private set; }
        [field: SerializeField] public string UniqueKey { get; private set; }
        public bool ShouldHelpCanvasDisplay { get; private set; }

        private void Awake()
        {
            Load();
        }

        public void EnableCanvasAndSetHelpText(LocalizedString localizedText)
        {
            if (ShouldHelpCanvasDisplay)
            {
                playerInputActions.EnableCanvasControlMode();
                HelpCanvas.enabled = true;

                helpText.text = localizedText.GetLocalizedString();
                helpText.ForceMeshUpdate();

                Vector2 panelRect = helpPanelRectTransform.sizeDelta;
                panelRect.y = helpText.renderedHeight + helpTexts.HeightCanvasFieldsForHelpText;
                helpPanelRectTransform.sizeDelta = panelRect;
            }
        }

        public void EnableCanvasAndSetHelpTextForcibly(LocalizedString localizedText)
        {
            playerInputActions.EnableCanvasControlMode();
            HelpCanvas.enabled = true;

            helpText.text = localizedText.GetLocalizedString();
            helpText.ForceMeshUpdate();

            Vector2 panelRect = helpPanelRectTransform.sizeDelta;
            panelRect.y = helpText.renderedHeight + helpTexts.HeightCanvasFieldsForHelpText;
            helpPanelRectTransform.sizeDelta = panelRect;
        }

        public void EnableHelpCanvasDisplaying()
        {
            ShouldHelpCanvasDisplay = true;

            Save();
        }

        public void DisableHelpCanvasDisplaying()
        {
            ShouldHelpCanvasDisplay = false;

            Save();
        }

        public void Save()
        {
            BoolForSaving boolForSaving = new(ShouldHelpCanvasDisplay);
            SavesHandler.Save(UniqueKey, boolForSaving);
        }

        public void Load()
        {
            BoolForSaving boolForSaving = SavesHandler.Load<BoolForSaving>(UniqueKey);

            if (boolForSaving.IsValuesSaved)
            {
                ShouldHelpCanvasDisplay = boolForSaving.SavingBool;
            }
        }
    }
}
