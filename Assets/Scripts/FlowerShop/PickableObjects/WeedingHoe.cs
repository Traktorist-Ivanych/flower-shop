using System.Collections;
using FlowerShop.Flowers;
using FlowerShop.PickableObjects.Moving;
using PlayerControl;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace FlowerShop.PickableObjects
{
    [RequireComponent(typeof(LittlePickableObjectMovingAndRotating))]
    public class WeedingHoe : MonoBehaviour, IPickableObject
    {
        [Inject] private readonly PlayerPickableObjectHandler playerPickableObjectHandler;
        [Inject] private readonly PlayerBusyness playerBusyness;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly GameConfiguration gameConfiguration;
        
        [SerializeField] private Mesh[] weedingHoeLvlMeshes = new Mesh[2];
        [field: SerializeField] public GrowingRoom GrowingRoom { get; private set; }
        
        [HideInInspector, SerializeField] private LittlePickableObjectMovingAndRotating littlePickableObjectMovingAndRotating;
        [HideInInspector, SerializeField] private MeshFilter weedingHoeMeshFilter;
    
        private int weedingHoeLvl;

        private void OnValidate()
        {
            littlePickableObjectMovingAndRotating = GetComponent<LittlePickableObjectMovingAndRotating>();
            weedingHoeMeshFilter = GetComponent<MeshFilter>();
        }

        public void TakeInPlayerHands()
        {
            playerPickableObjectHandler.CurrentPickableObject = this;
            littlePickableObjectMovingAndRotating.TakeLittlePickableObjectInPlayerHandsWithRotation();
        }

        public void PutOnTable(Transform targetTransform)
        {
            littlePickableObjectMovingAndRotating.PutLittlePickableObjectOnTableWithRotation(targetTransform);
        }

        public IEnumerator DeleteWeed(Pot potForDeletingWeed, WeedPlanter weedPlanterToAddPotIntoList)
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartWeedingTrigger);
            yield return new WaitForSeconds(gameConfiguration.WeedingTime - gameConfiguration.WeedingTimeLvlDelta * weedingHoeLvl);
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.FinishWeedingTrigger);
            potForDeletingWeed.DeleteWeed();
            weedPlanterToAddPotIntoList.AddPotInPlantingWeedList(potForDeletingWeed);
            playerBusyness.SetPlayerFree();
        }

        public void Improve()
        {
            weedingHoeLvl++;
            weedingHoeMeshFilter.mesh = weedingHoeLvlMeshes[weedingHoeLvl - 1];
        }
    }
}
