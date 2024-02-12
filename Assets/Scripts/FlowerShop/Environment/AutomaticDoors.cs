using UnityEngine;

namespace FlowerShop.Environment
{
    public class AutomaticDoors : MonoBehaviour
    {
        [Header("DoorsTransforms")]
        [SerializeField] private Transform automaticDoorLeft;
        [SerializeField] private Transform automaticDoorRight;
        [Header("DoorsSidesTransforms")]
        [SerializeField] private Transform doorLeftOpen;
        [SerializeField] private Transform doorLeftClose;
        [SerializeField] private Transform doorRightOpen;
        [SerializeField] private Transform doorRightClose;

        public void OpenDoors()
        {
            
        }
    }
}
