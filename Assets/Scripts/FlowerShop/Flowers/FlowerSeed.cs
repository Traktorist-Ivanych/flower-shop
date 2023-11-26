using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FlowerSeed : MonoBehaviour
{
    [FormerlySerializedAs("plantingFlower")] [SerializeField] private FlowerInfo plantingFlowerInfo;
    [SerializeField] private PlantingSeedsTable plantingSeedsTable;
    private Button seedButton;

    private void Start()
    {
        seedButton = GetComponent<Button>();
        seedButton.onClick.AddListener(SeedButtonClick);
    }

    private void SeedButtonClick()
    {
        plantingSeedsTable.PlantSeed(plantingFlowerInfo);
    }
}
