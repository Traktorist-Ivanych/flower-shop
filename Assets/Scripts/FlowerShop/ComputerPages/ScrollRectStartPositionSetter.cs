using UnityEngine;
using UnityEngine.UI;

namespace FlowerShop.ComputerPages
{
    public class ScrollRectStartPositionSetter : MonoBehaviour
    {
        [SerializeField, HideInInspector] private ScrollRect scrollRect;

        private bool isScrollRectStartFirstTime;

        private void OnValidate()
        {
            scrollRect = GetComponentInChildren<ScrollRect>();
        }

        public void ScrollRectStartFirstTime()
        {
            if (!isScrollRectStartFirstTime)
            {
                isScrollRectStartFirstTime = true;
                scrollRect.verticalNormalizedPosition = 1;
            }
        }
    }
}
