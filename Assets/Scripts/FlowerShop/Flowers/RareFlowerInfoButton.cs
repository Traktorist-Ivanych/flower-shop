using FlowerShop.ComputerPages;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace FlowerShop.Flowers
{
    [RequireComponent(typeof(UIButton))]
    public class RareFlowerInfoButton : MonoBehaviour
    {
        [Inject] private readonly FlowerInfoCanvasLiaison flowerInfoCanvasLiaison;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly RareFlowersHandler rareFlowersHandler;

        [SerializeField] private FlowerInfo flowerInfo;
        [SerializeField] private Image inCollcectionIndicator;

        [HideInInspector, SerializeField] private UIButton uIButton;
        [HideInInspector, SerializeField] private Image flowerImage;

        private bool isReceived;

        private void OnValidate()
        {
            uIButton = GetComponent<UIButton>();
            flowerImage = GetComponent<Image>();
        }

        private void OnEnable()
        {
            uIButton.OnClickEvent += OnButtonClick;
        }

        private void OnDisable()
        {
            uIButton.OnClickEvent -= OnButtonClick;
        }

        private void Start()
        {
            if (rareFlowersHandler.IsRareFlowerReceived(flowerInfo))
            {
                SetIsCrossedTrue();
            }
            else
            {
                flowerImage.sprite = flowersSettings.UnknownFlower128;
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
            flowerInfoCanvasLiaison.ShowFlowerInfo(isReceived, flowerInfo);
        }

        private void SetIsCrossedTrue()
        {
            isReceived = true;
            flowerImage.sprite = flowerInfo.FlowerSprite128;
        }
    }
}
