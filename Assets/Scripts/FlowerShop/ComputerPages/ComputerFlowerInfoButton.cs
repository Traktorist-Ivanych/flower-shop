using FlowerShop.Customers;
using FlowerShop.Customers.VipAndComplaints;
using FlowerShop.Education;
using FlowerShop.Flowers;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.ComputerPages
{
    [RequireComponent(typeof(UIButton))]
    [RequireComponent(typeof(Image))]
    public class ComputerFlowerInfoButton : MonoBehaviour
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly CustomersSettings customersSettings;
        [Inject] private readonly FlowerInfoCanvasLiaison flowerInfoCanvasLiaison;
        [Inject] private readonly VipOrdersHandler vipOrdersHandler;

        [SerializeField] private bool vipOrderMode;

        private FlowerInfo flowerInfo;

        [HideInInspector, SerializeField] private UIButton flowerButton;
        [HideInInspector, SerializeField] private Image flowerImage;
        
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

        public void SetFlowerInfo(FlowerInfo targetFlowerInfo)
        {
            flowerInfo = targetFlowerInfo;
            flowerImage.sprite = flowerInfo.FlowerSprite512;
        }

        private void OnButtonClick()
        {
            if (flowerInfo.FlowerLvl > 0)
            {
                if (vipOrderMode)
                {
                    string VipOrderPriceMultiplerText = " * " + 
                        vipOrdersHandler.CurrentVipOrderPriceMultipler.ToString(CultureInfo.CurrentCulture);

                    flowerInfoCanvasLiaison.ShowFlowerInfo(flowerInfo, VipOrderPriceMultiplerText);
                }
                else
                {
                    flowerInfoCanvasLiaison.ShowFlowerInfo(true, flowerInfo);
                }
            }

            if (educationHandler.IsEducationActive)
            {
                if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
                {
                    educationHandler.CompleteEducationStep();
                }
            }
        }
    }
}