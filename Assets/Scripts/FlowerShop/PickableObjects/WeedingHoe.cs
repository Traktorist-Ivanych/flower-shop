using System.Collections;
using FlowerShop.Flowers;
using FlowerShop.PickableObjects.Moving;
using FlowerShop.Weeds;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.PickableObjects
{
    [RequireComponent(typeof(ObjectMoving))]
    public class WeedingHoe : MonoBehaviour, IPickableObject
    {
        [Inject] private readonly PlayerPickableObjectHandler playerPickableObjectHandler;
        [Inject] private readonly PlayerBusyness playerBusyness;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly WeedSettings weedSettings;
        
        [SerializeField] private Mesh[] weedingHoeLvlMeshes = new Mesh[2];

        [HideInInspector, SerializeField] private ObjectMoving objectMoving; 
        [HideInInspector, SerializeField] private MeshFilter weedingHoeMeshFilter;
        
        private int weedingHoeLvl;
        
        [field: SerializeField] public GrowingRoom GrowingRoom { get; private set; }

        private void OnValidate()
        {
            objectMoving = GetComponent<ObjectMoving>();
            weedingHoeMeshFilter = GetComponent<MeshFilter>();
        }

        public void TakeInPlayerHandsAndSetPlayerFree()
        {
            playerPickableObjectHandler.CurrentPickableObject = this;
            objectMoving.MoveObject(
                targetFinishTransform: playerComponents.PlayerHandsForLittleObjectTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.TakeLittleObjectTrigger,
                setPlayerFree: true);
        }

        public void PutOnTableAndSetPlayerFree(Transform targetTransform)
        {
            objectMoving.MoveObject(
                targetFinishTransform: targetTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.GiveLittleObjectTrigger, 
                setPlayerFree: true);
        }

        public IEnumerator DeleteWeed(Pot potForDeletingWeed, WeedPlanter weedPlanterToAddPotIntoList)
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartWeedingTrigger);
            yield return new WaitForSeconds(weedSettings.WeedingTime - weedSettings.WeedingTimeLvlDelta * weedingHoeLvl);
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.FinishWeedingTrigger);
            potForDeletingWeed.DeleteWeed();
            weedPlanterToAddPotIntoList.AddPotInPlantingWeedList(potForDeletingWeed);
            playerBusyness.SetPlayerFree();
        }

        public void Upgrade(int tableLvl)
        {
            weedingHoeLvl = tableLvl;
            weedingHoeMeshFilter.mesh = weedingHoeLvlMeshes[weedingHoeLvl - 1];
        }

        public void LoadInPlayerHands()
        {
            objectMoving.SetParentAndParentPositionAndRotation(playerComponents.PlayerHandsForLittleObjectTransform);
            playerPickableObjectHandler.CurrentPickableObject = this;
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.LoadToHoldLittleObject);
        }
    }
}
