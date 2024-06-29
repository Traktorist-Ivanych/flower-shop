using UnityEngine;
using UnityEngine.Localization;

namespace FlowerShop.Help
{
    [CreateAssetMenu(fileName = "TableInfo",
        menuName = "Settings/Table Info",
        order = 21)]
    public class TableInfo : ScriptableObject
    {
        [field: SerializeField] public LocalizedString TableName { get; private set; }
        [field: SerializeField] public Sprite TableImprovementIndicatorSprite { get; private set; }
        [field: SerializeField] public Sprite TableBrokenIndicatorSprite { get; private set; }
        [field: SerializeField] public LocalizedString TableDescription { get; private set; }
    }
}
