using System.Collections;
using FlowerShop.Effects;
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
        [Inject] private readonly ActionProgressbar playerActionPtogressbar;
        [Inject] private readonly PlayerPickableObjectHandler playerPickableObjectHandler;
        [Inject] private readonly PlayerBusyness playerBusyness;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly RepairsAndUpgradesSettings repairsAndUpgradesSettings;
        [Inject] private readonly SelectedTableEffect selectedTableEffect;

        [HideInInspector, SerializeField] private ObjectMoving objectMoving;
        
        public IUpgradableTable UpgradableTable { get; set; }

        private void OnValidate()
        {
            objectMoving = GetComponent<ObjectMoving>();
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

        public IEnumerator ImproveTable()
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartBuildsTrigger);
            playerActionPtogressbar.EnableActionProgressbar(repairsAndUpgradesSettings.TableUpgradeTime);
            yield return new WaitForSeconds(repairsAndUpgradesSettings.TableUpgradeTime);
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.FinishBuildsTrigger);
            UpgradableTable.UpgradeTableFinish();
            playerBusyness.SetPlayerFree();
            selectedTableEffect.ActivateEffectWithDelay();
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
