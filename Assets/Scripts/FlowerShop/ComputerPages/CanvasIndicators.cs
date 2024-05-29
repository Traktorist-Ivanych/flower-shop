using UnityEngine;
using UnityEngine.UI;

namespace FlowerShop.ComputerPages
{
    public class CanvasIndicators : MonoBehaviour
    {
        [field: SerializeField] public Image ComplaintIndicatorImage { get; private set; }
        [field: SerializeField] public Image ComplaintIndicatorImageBackGround { get; private set; }
        [field: SerializeField] public Image VipIndicatorImage { get; private set; }
        [field: SerializeField] public Image VipIndicatorImageBackGround { get; private set; }

        public void ShowComplaintIndicator()
        {
            ComplaintIndicatorImage.enabled = true;
            ComplaintIndicatorImageBackGround.enabled = true;
        }

        public void HideComplaintIndicator()
        {
            ComplaintIndicatorImage.enabled = false;
            ComplaintIndicatorImageBackGround.enabled = false;
        }

        public void ShowVipIndicator()
        {
            VipIndicatorImage.enabled = true;
            VipIndicatorImageBackGround.enabled = true;
        }

        public void HideVipIndicator()
        {
            VipIndicatorImage.enabled = false;
            VipIndicatorImageBackGround.enabled = false;
        }
    }
}