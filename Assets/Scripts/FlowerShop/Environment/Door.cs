using FlowerShop.Sounds;
using UnityEngine;
using Zenject;

namespace FlowerShop.Environment
{
    [RequireComponent(typeof(HingeJoint))]
    public class Door : MonoBehaviour
    {
        [Inject] private readonly EnvironmentSettings environmentSettings;
        [Inject] private readonly SoundsHandler soundsHandler;
        
        [HideInInspector, SerializeField] private HingeJoint doorJoint;

        private bool isDoorOpen;
        
        private void OnValidate()
        {
            doorJoint = GetComponent<HingeJoint>();
        }

        private void Update()
        {
            if ((doorJoint.angle >= environmentSettings.ClosedDoorAngle ||
                doorJoint.angle <= -environmentSettings.ClosedDoorAngle) &&
                !isDoorOpen)
            {
                soundsHandler.PlayOpenDoorAudio();
                isDoorOpen = true;
            }
            else if (doorJoint.angle < environmentSettings.ClosedDoorAngle &&
                     doorJoint.angle > -environmentSettings.ClosedDoorAngle && 
                     isDoorOpen)
            {
                soundsHandler.PlayCloseDoorAudio();
                isDoorOpen = false;
            }
        }
    }
}
