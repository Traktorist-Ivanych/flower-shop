using FlowerShop.Flowers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.ComputerPages
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class ComputerFlowerInfoButton : MonoBehaviour
    {
        [Inject] private readonly FlowerInfoCanvasLiaison flowerInfoCanvasLiaison;
        
        private FlowerInfo flowerInfo;

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
            flowerImage.sprite = flowerInfo.FlowerSprite;
        }

        private void OnButtonClick()
        {
            flowerInfoCanvasLiaison.ShowFlowerInfo(flowerInfo, flowerInfo.FlowerSprite);
        }
    }
}