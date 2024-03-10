using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.Flowers
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class FlowerInfoRecipeButton : MonoBehaviour
    {
        [Inject] private readonly FlowerInfoCanvasLiaison flowerInfoCanvasLiaison;
        [Inject] private readonly FlowersContainer flowersContainer;
        [Inject] private readonly FlowersSettings flowersSettings;
        
        private FlowerInfo flowerInfo;
        private bool isInteractive;

        [HideInInspector, SerializeField] private Button flowerButton;
        [HideInInspector, SerializeField] private Image flowerImage;
        
        private void OnValidate()
        {
            flowerButton = GetComponent<Button>();
            flowerImage = GetComponent<Image>();
        }

        private void OnEnable()
        {
            flowerButton.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            flowerButton.onClick.RemoveListener(OnButtonClick);
        }

        public void SetFlowerInfo(FlowerInfo targetFlowerInfo)
        {
            flowerInfo = targetFlowerInfo;
            
            if (flowerInfo.FlowerLvl == 1 || flowersContainer.IsFlowerInfoAvailable(flowerInfo))
            {
                isInteractive = true;
                flowerImage.sprite = flowerInfo.FlowerSprite;
            }
            else if (flowerInfo == flowersSettings.FlowerInfoEmpty)
            {
                isInteractive = false;
                flowerImage.sprite = flowersSettings.UnplayableFlower;
            }
            else
            {
                isInteractive = false;
                flowerImage.sprite = flowersSettings.UnknownFlower;
            }
        }

        private void OnButtonClick()
        {
            if (isInteractive)
            {
                flowerInfoCanvasLiaison.ShowFlowerInfo(flowerInfo, flowerInfo.FlowerSprite);
            }
        }
    }
}