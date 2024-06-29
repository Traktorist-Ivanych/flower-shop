using FlowerShop.ComputerPages;
using FlowerShop.Tables;
using UnityEngine;
using Zenject;

namespace FlowerShop.Fertilizers
{
    [RequireComponent(typeof(UIButton))]
    public class BuyFertilizersButton : MonoBehaviour
    {
        [Inject] private readonly FertilizersSetting fertilizersSetting;
        [Inject] private readonly FertilizersTable fertilizersTable;
        
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
            //playerMoney.TakePlayerMoney(fertilizersSetting.IncreaseFertilizerAmountIAP);
            //fertilizersTable.IncreaseAvailableFertilizersUsesNumber();
        }
    }
}