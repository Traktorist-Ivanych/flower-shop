using UnityEngine;

namespace PlayerControl
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        public delegate void AnimationEvent();

        private event AnimationEvent CurrentAnimationEvent;

        public void SetCurrentAnimationEvent(AnimationEvent currentEvent)
        {
            CurrentAnimationEvent = null;
            CurrentAnimationEvent += currentEvent;
        }

        public void SetCurrentAnimationEvents(AnimationEvent firstEvent, AnimationEvent secondEvent)
        {
            CurrentAnimationEvent = null;
            CurrentAnimationEvent += firstEvent;
            CurrentAnimationEvent += secondEvent;
        }

        private void InvokeCurrentAnimationEvent()
        {
            CurrentAnimationEvent?.Invoke();
            CurrentAnimationEvent = null;
        }
    }
}
