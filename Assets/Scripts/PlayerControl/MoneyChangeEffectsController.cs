using UnityEngine;

namespace PlayerControl
{
    public class MoneyChangeEffectsController : MonoBehaviour
    {
        [SerializeField] private MoneyChangeEffect[] moneyChangeEffects;

        private int currentIndex;

        public void DisplayEffect(Color textColor, string moneyText)
        {
            moneyChangeEffects[currentIndex].StartEffect(textColor, moneyText);

            currentIndex++;

            if (currentIndex >= moneyChangeEffects.Length)
            {
                currentIndex = 0;
            }
        }
    }
}