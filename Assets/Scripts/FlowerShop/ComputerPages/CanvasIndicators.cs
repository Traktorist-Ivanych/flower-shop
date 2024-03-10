using UnityEngine;
using UnityEngine.UI;

namespace FlowerShop.ComputerPages
{
    public class CanvasIndicators : MonoBehaviour
    {
        [field: SerializeField] public Image ComplaintIndicatorImage { get; private set; }
        [field: SerializeField] public Image VipIndicatorImage { get; private set; }
    }
}