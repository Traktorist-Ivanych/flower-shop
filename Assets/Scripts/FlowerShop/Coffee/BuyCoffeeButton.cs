using FlowerShop.ComputerPages;
using FlowerShop.Tables;
using UnityEngine;
using Zenject;

namespace FlowerShop.Coffee
{
    [RequireComponent(typeof(UIButton))]
    public class BuyCoffeeButton : MonoBehaviour
    {
        [Inject] private readonly CoffeeCanvasLiaison coffeeCanvasLiaison;
        [Inject] private readonly CoffeeTable coffeeTable;

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
            //coffeeCanvasLiaison.DisableCanvas();
            //StartCoroutine(coffeeTable.MakeCoffeeProcess());
        }
    }
}
