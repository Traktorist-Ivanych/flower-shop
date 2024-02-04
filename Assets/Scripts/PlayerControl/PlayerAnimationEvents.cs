using UnityEngine;

namespace PlayerControl
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        public delegate void AnimationEvent();

        private event AnimationEvent CurrentAnimationEvent;

        public void SetCurrentAnimationEvent(AnimationEvent currentAnimationEvent)
        {
            CurrentAnimationEvent = null;
            CurrentAnimationEvent += currentAnimationEvent;
        }

        private void InvokeCurrentAnimationEvent()
        {
            CurrentAnimationEvent?.Invoke();
            CurrentAnimationEvent = null;
        }
    }
}
