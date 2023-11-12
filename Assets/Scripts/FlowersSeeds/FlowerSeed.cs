using UnityEngine;
using UnityEngine.UI;

public class FlowerSeed : MonoBehaviour, IFlower
{
    [SerializeField] private Flower plantingFlower;
    [SerializeField] private PlantingSeedsTable plantingSeedsTable;
    private Button seedButton;

    private void Start()
    {
        seedButton = GetComponent<Button>();
        seedButton.onClick.AddListener(SeedButtonClick);
    }

    private void SeedButtonClick()
    {
        plantingSeedsTable.PlantSeed(plantingFlower);
    }
}
