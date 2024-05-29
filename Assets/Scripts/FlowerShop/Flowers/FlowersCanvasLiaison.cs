using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FlowerShop.Flowers
{
    [RequireComponent(typeof(Canvas))]
    public class FlowersCanvasLiaison : MonoBehaviour
    {
        [SerializeField] private List<FlowerInfoButton> flowerInfoButtons;
        [SerializeField] private List<RareFlowerInfoButton> rareFlowerInfoButtons;

        [SerializeField, HideInInspector] private ScrollRect flowersCanvasScrollRect;
        [SerializeField, HideInInspector] private Canvas flowersCanvas;

        private bool isScrollBarShowFirstTime;

        private void OnValidate()
        {
            flowersCanvas = GetComponent<Canvas>();
            flowersCanvasScrollRect = GetComponentInChildren<ScrollRect>();
        }

        public void ShowFlowersCanvas()
        {
            flowersCanvas.enabled = true;
            if (!isScrollBarShowFirstTime)
            {
                isScrollBarShowFirstTime = true;
                flowersCanvasScrollRect.verticalNormalizedPosition = 1;
            }
        }

        public void HideFlowersCanvas()
        {
            flowersCanvas.enabled = false;
        }

        public void ShowCrossedFlowerInfo(FlowerInfo crossedFlowerInfo)
        {
            foreach (FlowerInfoButton flowerInfoButton in flowerInfoButtons)
            {
                flowerInfoButton.TrySetIsCrossedTrue(crossedFlowerInfo);
            }
        }

        public void SetInCollectionInicator(FlowerInfo inCollectionFlowerInfo)
        {
            foreach (FlowerInfoButton flowerInfoButton in flowerInfoButtons)
            {
                flowerInfoButton.TrySetInCollectionIndicator(inCollectionFlowerInfo);
            }
        }

        public void ShowRareFlowerInfo(FlowerInfo crossedFlowerInfo)
        {
            foreach (RareFlowerInfoButton rareFlowerInfoButton in rareFlowerInfoButtons)
            {
                rareFlowerInfoButton.TrySetIsCrossedTrue(crossedFlowerInfo);
            }
        }

        public void SetRareInCollectionInicator(FlowerInfo inCollectionFlowerInfo)
        {
            foreach (RareFlowerInfoButton flowerInfoButton in rareFlowerInfoButtons)
            {
                flowerInfoButton.TrySetInCollectionIndicator(inCollectionFlowerInfo);
            }
        }

    }
}