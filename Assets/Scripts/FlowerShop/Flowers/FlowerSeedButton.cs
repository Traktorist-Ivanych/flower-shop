using FlowerShop.Tables;
using UnityEngine;
using UnityEngine.UI;

namespace FlowerShop.Flowers
{
    public class FlowerSeedButton : MonoBehaviour
    {
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
            plantingSeedsTable.PlantSeed(plantingFlowerInfo);
        }
    }
}
