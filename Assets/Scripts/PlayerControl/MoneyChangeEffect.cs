using TMPro;
using UnityEngine;
using Zenject;

namespace PlayerControl
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class MoneyChangeEffect : MonoBehaviour
    {
        [Inject] private readonly PlayerControlSettings playerControlSettings;
        
        [HideInInspector, SerializeField] private TextMeshProUGUI moneyChangeText;
        
        private bool isEffectActive;
        private float currentEffectTime;
        
        private void OnValidate()
        {
            moneyChangeText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (isEffectActive)
            {
                currentEffectTime += Time.deltaTime;
                
                Vector2 moneyTextAnchoredPosition = moneyChangeText.rectTransform.anchoredPosition;
                
                Vector2 targetPosition = new Vector2(
                    x: moneyTextAnchoredPosition.x, 
                    y: moneyTextAnchoredPosition.y -
                    playerControlSettings.MoneyEffectMovingSpeed * Time.deltaTime);
                
                moneyChangeText.rectTransform.anchoredPosition = targetPosition;

                if (currentEffectTime >= playerControlSettings.MoneyEffectStartHidingTime)
                {
                    Color newColor = moneyChangeText.color;
                    newColor.a = (playerControlSettings.MoneyEffectTimeDuration - currentEffectTime) / 
                                 (playerControlSettings.MoneyEffectTimeDuration - 
                                 playerControlSettings.MoneyEffectStartHidingTime);
                    
                    moneyChangeText.color = newColor;
                }

                if (currentEffectTime >= playerControlSettings.MoneyEffectTimeDuration)
                {
                    DisableEffect();
                }
            }
        }

        public void StartEffect(Color textColor, string text)
        {
            moneyChangeText.color = textColor;
            moneyChangeText.text = text;
            moneyChangeText.rectTransform.anchoredPosition = Vector2.zero;
            isEffectActive = true;
            moneyChangeText.enabled = true;
            currentEffectTime = 0;
        }

        private void DisableEffect()
        {
            isEffectActive = false;
            moneyChangeText.enabled = false;
            currentEffectTime = 0;
        }
    }
}