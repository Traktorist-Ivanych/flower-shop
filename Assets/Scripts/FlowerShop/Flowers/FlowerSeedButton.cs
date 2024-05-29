using FlowerShop.ComputerPages;
using FlowerShop.Education;
using FlowerShop.Tables;
using Input;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using Zenject;

namespace FlowerShop.Flowers
{
    [RequireComponent(typeof(UIButton))]
    public class FlowerSeedButton : MonoBehaviour
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly PlayerInputActions playerInputActions;
        
        [SerializeField] private FlowerInfo plantingFlowerInfo;
        [SerializeField] private PlantingSeedsTable plantingSeedsTable;
        [SerializeField] private LocalizeStringEvent localizeStringEvent;

        [HideInInspector, SerializeField] private UIButton seedButton;

        private void OnValidate()
        {
            seedButton = GetComponent<UIButton>();
        }

        private void Awake()
        {
            localizeStringEvent.StringReference = plantingFlowerInfo.LocalizedFlowerName;
        }

        private void OnEnable()
        {
            seedButton.OnClickEvent += OnSeedButtonClick;
        }

        private void OnDisable()
        {
            seedButton.OnClickEvent -= OnSeedButtonClick;
        }

        private void OnSeedButtonClick()
        {
            if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
            {
                educationHandler.CompleteEducationStep();
                PlantSeed();
            }
            else if (!educationHandler.IsEducationActive)
            {
                PlantSeed();
            }
        }

        private void PlantSeed()
        {
            plantingSeedsTable.PlantSeed(plantingFlowerInfo);
            playerInputActions.DisableCanvasControlMode();
        }
    }
}
