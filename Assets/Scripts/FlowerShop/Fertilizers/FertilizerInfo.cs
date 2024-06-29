using UnityEngine;
using UnityEngine.Localization;

namespace FlowerShop.Fertilizers
{
    [CreateAssetMenu(fileName = "FertilizerInfo", menuName = "Fertilizer Info", order = 59)]
    public class FertilizerInfo : ScriptableObject
    {
        [field: SerializeField] public LocalizedString LocalizedFertilizerName { get; private set; }
        [field: SerializeField] public string FertilizerName { get; private set; }
        [field: SerializeField] public Sprite FertilizerSprite { get; private set; }
        [field: SerializeField] public LocalizedString LocalizedFertilizerDescription { get; private set; }
        [field: SerializeField] public string FertilizerDescription { get; private set; }
    }
}