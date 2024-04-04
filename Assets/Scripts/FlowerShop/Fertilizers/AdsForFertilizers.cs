using FlowerShop.ComputerPages;
using UnityEngine;

namespace FlowerShop.Fertilizers
{
    [RequireComponent(typeof(UIButton))]
    public class AdsForFertilizers : MonoBehaviour
    {
        [HideInInspector, SerializeField] private UIButton uiButton;

        private void OnValidate()
        {
            uiButton = GetComponent<UIButton>();
        }

        private void OnEnable()
        {
            uiButton.OnClickEvent += OnButtonClick;
        }
        
        private void OnDisable()
        {
            uiButton.OnClickEvent -= OnButtonClick;
        }

        private void OnButtonClick()
        {
            
        }
    }
}