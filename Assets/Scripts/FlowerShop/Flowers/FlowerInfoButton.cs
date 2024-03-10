using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.Flowers
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class FlowerInfoButton : MonoBehaviour
    {
        [Inject] private readonly FlowerInfoCanvasLiaison flowerInfoCanvasLiaison;
        [Inject] private readonly FlowersContainer flowersContainer;
        [Inject] private readonly FlowersSettings flowersSettings;
        
        [SerializeField] private FlowerInfo flowerInfo;

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
                    flowerImage.sprite = flowersSettings.UnknownFlower;
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

        private void OnButtonClick()
        {
            flowerInfoCanvasLiaison.ShowFlowerInfo(flowerInfo, flowerImage.sprite);
        }

        private void SetIsCrossedTrue()
        {
            flowerImage.sprite = flowerInfo.FlowerSprite;
        }
    }
}
