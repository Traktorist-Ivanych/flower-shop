using FlowerShop.Coffee;
using FlowerShop.Sounds;
using FlowerShop.Tables;
using PlayerControl;
using UnityEngine;
using UnityEngine.Purchasing;
using Zenject;

namespace FlowerShop.Purchasing
{
    public class PurchasingHandler : MonoBehaviour
    {
        [Inject] private readonly CoffeeCanvasLiaison coffeeCanvasLiaison;
        [Inject] private readonly CoffeeTable coffeeTable;
        [Inject] private readonly FertilizersTable fertilizersTable;
        [Inject] private readonly PlayerCoffeeEffect playerCoffeeEffect;
        [Inject] private readonly PlayerControlSettings playerControlSettings;
        [Inject] private readonly PlayerMoney playerMoney;
        [Inject] private readonly SoundsHandler soundsHandler;
        
        public void OnPurchaseComplete(Product product)
        {
            switch (product.definition.id)
            {
                case "com.royallily.flowershop.add10000coins":
                    Add10000Coins();
                    break;
                case "com.royallily.flowershop.add25000coins":
                    Add25000Coins();
                    break;
                case "com.royallily.flowershop.addfertilizers":
                    AddFertilizers();
                    break;
                case "com.royallily.flowershop.addcoffeeeffect":
                    AddCoffeeEffect();
                    break;
            }
        }

        private void Add10000Coins()
        {
            playerMoney.AddPlayerMoney(playerControlSettings.IAPMoneyRewardFirst);
            soundsHandler.PlayAddMoneyAudio();
        }

        private void Add25000Coins()
        {
            playerMoney.AddPlayerMoney(playerControlSettings.IAPMoneyRewardSecond);
            soundsHandler.PlayAddMoneyAudio();
        }

        private void AddFertilizers()
        {
            fertilizersTable.IncreaseAvailableFertilizersUsesNumberByIAP();
        }

        private void AddCoffeeEffect()
        {
            playerCoffeeEffect.PurchaseCoffeeEffect();
            coffeeCanvasLiaison.DisableCanvas();
            StartCoroutine(coffeeTable.MakePurchasedCoffeeProcess());
        }
    }
}
