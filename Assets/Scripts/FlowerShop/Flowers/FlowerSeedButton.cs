using FlowerShop.Education;
using FlowerShop.Tables;
using Input;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.Flowers
{
    public class FlowerSeedButton : MonoBehaviour
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly PlayerInputActions playerInputActions;
        
        [SerializeField] private FlowerInfo plantingFlowerInfo;
        [SerializeField] private PlantingSeedsTable plantingSeedsTable;
        
        [HideInInspector, SerializeField] private Button seedButton;

        private void OnValidate()
        {
            seedButton = GetComponent<Button>();
        }

        private void OnEnable()
        {
            seedButton.onClick.AddListener(OnSeedButtonClick);
        }

        private void OnDisable()
        {
            seedButton.onClick.RemoveListener(OnSeedButtonClick);
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
