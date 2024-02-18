using UnityEngine;

namespace FlowerShop.Customers
{
    public static class CustomerAnimatorKeys
    {
        public static readonly int IsPlayerWalk = Animator.StringToHash("IsCustomerWalk");
        public static readonly int Think = Animator.StringToHash("Think");
        public static readonly int Clear = Animator.StringToHash("Clear");
        public static readonly int WalkSpeed = Animator.StringToHash("WalkSpeed");
        public static readonly int No = Animator.StringToHash("No");
        public static readonly int Yes = Animator.StringToHash("Yes");
        public static readonly int LookAround = Animator.StringToHash("LookAround");
    }
}