using FlowerShop.Tables;
using PlayerControl;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.Fertilizers
{
    [RequireComponent(typeof(Button))]
    public class BuyFertilizersButton : MonoBehaviour
    {
        [Inject] private readonly FertilizersSetting fertilizersSetting;
        [Inject] private readonly FertilizersTable fertilizersTable;
        [Inject] private readonly PlayerMoney playerMoney;
        
        [HideInInspector, SerializeField] private Button buyButton;

        private void OnValidate()
        {
            buyButton = GetComponent<Button>();
        }

        private void OnEnable()
        {
            buyButton.onClick.AddListener(TryToBuyFertilizers);
        }

        private void OnDisable()
        {
            buyButton.onClick.RemoveListener(TryToBuyFertilizers);
        }

        private void TryToBuyFertilizers()
        {
            if (playerMoney.CurrentPlayerMoney >= fertilizersSetting.FertilizersPrice)
            {
                playerMoney.TakePlayerMoney(fertilizersSetting.FertilizersPrice);
                fertilizersTable.AddFertilizers();
            }
        }
    }
}