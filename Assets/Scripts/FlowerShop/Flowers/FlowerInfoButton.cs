using FlowerShop.ComputerPages;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.Flowers
{
    [RequireComponent(typeof(UIButton))]
    [RequireComponent(typeof(Image))]
    public class FlowerInfoButton : MonoBehaviour
    {
        [Inject] private readonly FlowerInfoCanvasLiaison flowerInfoCanvasLiaison;
        [Inject] private readonly FlowersContainer flowersContainer;
        [Inject] private readonly FlowersSettings flowersSettings;
        
        [SerializeField] private FlowerInfo flowerInfo;
        [SerializeField] private Image inCollcectionIndicator;

        [HideInInspector, SerializeField] private UIButton flowerButton;
        [HideInInspector, SerializeField] private Image flowerImage;

        private bool isCrossed;

        private void OnValidate()
        {
            flowerButton = GetComponent<UIButton>();
            flowerImage = GetComponent<Image>();
        }

        private void OnEnable()
        {
            flowerButton.OnClickEvent += OnButtonClick;
        }

        private void OnDisable()
        {
            flowerButton.OnClickEvent -= OnButtonClick;
        }

        private void Start()
        {
            if (flowerInfo.FlowerLvl > 1)
            {
                if (flowersContainer.IsFlowerInfoAvailable(flowerInfo))
                {
                    SetIsCrossedTrue();
                }
                else
                {
                    flowerImage.sprite = flowersSettings.UnknownFlower128;
                }
            }
        }

        public void TrySetIsCrossedTrue(FlowerInfo crossedFlowerInfo)
        {
            if (crossedFlowerInfo.Equals(flowerInfo))
            {
                SetIsCrossedTrue();
            }
        }

        public void TrySetInCollectionIndicator(FlowerInfo inCollectionFlowerInfo)
        {
            if (inCollectionFlowerInfo.Equals(flowerInfo))
            {
                inCollcectionIndicator.sprite = flowersSettings.InCollectionIndicator;
            }
        }

        private void OnButtonClick()
        {
            flowerInfoCanvasLiaison.ShowFlowerInfo(isCrossed, flowerInfo);
        }

        private void SetIsCrossedTrue()
        {
            isCrossed = true;
            flowerImage.sprite = flowerInfo.FlowerSprite128;
        }
    }
}
