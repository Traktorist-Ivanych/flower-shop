using DG.Tweening;
using FlowerShop.Sounds;
using UnityEngine;
using Zenject;

namespace FlowerShop.Environment
{
    public class AutomaticDoors : MonoBehaviour
    {
        [Inject] private readonly SoundsHandler soundsHandler;
        
        [SerializeField] private AnimationCurve doorMoving;
        [SerializeField] private float doorOpenTime;
        
        [Header("DoorsTransforms")]
        [SerializeField] private Transform automaticDoorLeft;
        [SerializeField] private Transform automaticDoorRight;
        [Header("DoorsSidesTransforms")]
        [SerializeField] private Transform doorLeftOpen;
        [SerializeField] private Transform doorLeftClose;
        [SerializeField] private Transform doorRightOpen;
        [SerializeField] private Transform doorRightClose;
        
        public bool IsOpen { get; private set; }
        
        public void Open()
        {
            soundsHandler.PlayOpenCloseAutomaticDoorAudio();
            IsOpen = true;
            MoveDoor(automaticDoorLeft, doorLeftOpen);
            MoveDoor(automaticDoorRight, doorRightOpen);
        }

        public void Close()
        {
            soundsHandler.PlayOpenCloseAutomaticDoorAudio();
            IsOpen = false;
            MoveDoor(automaticDoorLeft, doorLeftClose);
            MoveDoor(automaticDoorRight, doorRightClose);
        }

        private void MoveDoor(Transform door, Transform endPosition)
        {
            door.DOMove(endValue: endPosition.position, doorOpenTime).SetEase(doorMoving);
        }
    }
}
