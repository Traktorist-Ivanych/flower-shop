using System.Collections;
using FlowerShop.Achievements;
using FlowerShop.Education;
using FlowerShop.Effects;
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
        [Inject] private readonly ActionProgressbar playerActionProgressbar;
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly PlayerPickableObjectHandler playerPickableObjectHandler;
        [Inject] private readonly PlayerBusyness playerBusyness;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly SelectedTableEffect selectedTableEffect;
        [Inject] private readonly WeedKiller weedKiller;
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
            selectedTableEffect.ActivateEffectWithDelay();
            
            objectMoving.MoveObject(
                targetFinishTransform: playerComponents.PlayerHandsForLittleObjectTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.TakeLittleObjectTrigger,
                setPlayerFree: true);
        }

        public void PutOnTableAndSetPlayerFree(Transform targetTransform)
        {
            selectedTableEffect.ActivateEffectWithDelay();
            
            objectMoving.MoveObject(
                targetFinishTransform: targetTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.GiveLittleObjectTrigger, 
                setPlayerFree: true);
        }

        public IEnumerator DeleteWeed(Pot potForDeletingWeed, WeedPlanter weedPlanterToAddPotIntoList)
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartWeedingTrigger);

            float currentWeedingTime = weedSettings.WeedingTime - weedSettings.WeedingTimeLvlDelta * weedingHoeLvl;
            playerActionProgressbar.EnableActionProgressbar(currentWeedingTime);
            yield return new WaitForSeconds(currentWeedingTime);

            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.FinishWeedingTrigger);
            potForDeletingWeed.DeleteWeed();
            weedPlanterToAddPotIntoList.AddPotInPlantingWeedList(potForDeletingWeed);
            playerBusyness.SetPlayerFree();
            selectedTableEffect.ActivateEffectWithDelay();

            weedKiller.IncreaseProgress();
            
            if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
            {
                educationHandler.CompleteEducationStep();
            }
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
            selectedTableEffect.ActivateEffectWithDelay();
        }
    }
}
