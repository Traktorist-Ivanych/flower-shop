using UnityEngine;

namespace FlowerShop.Flowers
{
    [CreateAssetMenu(
        fileName = "NewFlowersSettings", 
        menuName = "Settings/Flowers Settings", 
        order = 7)]
    public class FlowersSettings : ScriptableObject
    {
        [field: SerializeField] public int MaxGradesCount { get; private set; }
        [field: SerializeField] public float AllUniqueFlowersCount { get; private set; }
        [field: SerializeField] public int SoilPrice { get; private set; }
        [field: SerializeField] public int FirstLvlFlowersPrice { get; private set; }
        [field: SerializeField] public int RareFlowerLvl { get; private set; }
        [field: SerializeField] public int MinFlowerLvl { get; private set; }

        [field: Tooltip("Determines from what level (inclusive) flower can be crossed")]
        [field: SerializeField] public int CrossingFlowerMinLvl { get; private set; }
        [field: SerializeField] public int CrossingFlowerMaxLvl { get; private set; }
        [field: SerializeField] public int MaxFlowerGrowingLvl { get; private set; }
        
        [field: Tooltip("Determines what level will be set when flower is planted")]
        [field: SerializeField] public int PrimaryFlowerGrowingLvl { get; private set; }
        [field: SerializeField] public FlowerName FlowerNameEmpty { get; private set; }
        [field: SerializeField] public FlowerInfo FlowerInfoEmpty { get; private set; }
        [field: SerializeField] public GrowingRoom GrowingRoomAny { get; private set; }
        [field: SerializeField] public GrowingRoom GrowingRoomDecorative { get; private set; }
        [field: SerializeField] public GrowingRoom GrowingRoomExotic { get; private set; }
        [field: SerializeField] public GrowingRoom GrowingRoomWild { get; private set; }
        [field: SerializeField] public Sprite InCollectionIndicator { get; private set; }
        [field: SerializeField] public Sprite UnknownFlower { get; private set; }
        [field: SerializeField] public Sprite UnknownFlower128 { get; private set; }
        [field: SerializeField] public Sprite UnplayableFlower { get; private set; }
        [field: SerializeField] public string UnknownWord { get; private set; }
        [field: SerializeField] public string QuestionMark { get; private set; }

        [Header("Rare Flowers")]
        [SerializeField] private AnimationCurve rareFlowerCurve;
        [SerializeField] private float appearanceRareFlowerSuccessBorder;

        [field: SerializeField] public int GuaranteedRareFlowerCrossingMaxIndex {  get; private set; }

        public bool ShouldRareFlowerAppearance()
        {
            float currentTime = Random.Range(0, 1f);
            return rareFlowerCurve.Evaluate(currentTime) >= appearanceRareFlowerSuccessBorder;
        }
    }
}