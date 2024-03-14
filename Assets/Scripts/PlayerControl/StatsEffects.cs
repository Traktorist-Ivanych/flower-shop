using UnityEngine;

namespace PlayerControl
{
    public class StatsEffects : MonoBehaviour
    {
        [field: SerializeField] public ChangeEffectHandler MoneyChangeEffect { get; private set; }
        [field: SerializeField] public ChangeEffectHandler RatingChangeEffect { get; private set; }
    }
}