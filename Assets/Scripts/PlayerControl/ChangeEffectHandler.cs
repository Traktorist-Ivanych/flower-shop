using UnityEngine;

namespace PlayerControl
{
    public class ChangeEffectHandler : MonoBehaviour
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