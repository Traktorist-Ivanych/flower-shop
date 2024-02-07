using FlowerShop.Sounds;
using UnityEngine;
using Zenject;

namespace FlowerShop.Environment
{
    [RequireComponent(typeof(HingeJoint))]
    public class Door : MonoBehaviour
    {
        [Inject] private readonly SoundsHandler soundsHandler;
        
        [HideInInspector, SerializeField] private HingeJoint doorJoint;

        private bool isDoorOpen;
        
        private void OnValidate()
        {
            doorJoint = GetComponent<HingeJoint>();
        }

        private void Update()
        {
            if (doorJoint.angle is >= 1 or <= -1 && !isDoorOpen)
            {
                soundsHandler.PlayOpenDoorAudio();
                isDoorOpen = true;
            }
            else if (doorJoint.angle is < 1 and > -1 && isDoorOpen)
            {
                soundsHandler.PlayCloseDoorAudio();
                isDoorOpen = false;
            }
        }
    }
}
