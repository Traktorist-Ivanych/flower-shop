using System.Collections;
using FlowerShop.PickableObjects.Moving;
using FlowerShop.RepairsAndUpgrades;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.PickableObjects
{
    [RequireComponent(typeof(ObjectMoving))]
    public class RepairingAndUpgradingHammer : MonoBehaviour, IPickableObject
    {
        [Inject] private readonly PlayerPickableObjectHandler playerPickableObjectHandler;
        [Inject] private readonly PlayerBusyness playerBusyness;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly RepairsAndUpgradesSettings repairsAndUpgradesSettings;

        [HideInInspector, SerializeField] private ObjectMoving objectMoving;
        
        public IUpgradableTable UpgradableTable { get; set; }

        private void OnValidate()
        {
            objectMoving = GetComponent<ObjectMoving>();
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

        public IEnumerator ImproveTable()
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartBuildsTrigger);
            UpgradableTable.HideUpgradeIndicator();
            yield return new WaitForSeconds(repairsAndUpgradesSettings.TableUpgradeTime);
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.FinishBuildsTrigger);
            UpgradableTable.UpgradeTable();
            playerBusyness.SetPlayerFree();
        }

        public void LoadInPlayerHands()
        {
            objectMoving.SetParentAndParentPositionAndRotationOnLoad(playerComponents.PlayerHandsForLittleObjectTransform);
            playerPickableObjectHandler.CurrentPickableObject = this;
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.LoadToHoldLittleObject);
        }
    }
}
