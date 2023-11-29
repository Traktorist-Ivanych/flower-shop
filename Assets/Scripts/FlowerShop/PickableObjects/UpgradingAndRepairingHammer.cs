using System.Collections;
using FlowerShop.PickableObjects.Moving;
using FlowerShop.Upgrades;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.PickableObjects
{
    public class UpgradingAndRepairingHammer : MonoBehaviour, IPickableObject
    {
        [Inject] private readonly PlayerPickableObjectHandler playerPickableObjectHandler;
        [Inject] private readonly PlayerBusyness playerBusyness;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly GameConfiguration gameConfiguration;

        private LittlePickableObjectMovingAndRotating hammerMovingAndRotating;
        private IUpgradableTable upgradableTable;

        public IUpgradableTable UpgradableTable
        {
            set => upgradableTable = value;
        }

        private void Start()
        {
            hammerMovingAndRotating = GetComponent<LittlePickableObjectMovingAndRotating>();
        }

        public void TakeInPlayerHands()
        {
            playerPickableObjectHandler.CurrentPickableObject = this;
            hammerMovingAndRotating.TakeLittlePickableObjectInPlayerHandsWithRotation();
        }

        public void PutOnTable(Transform targetTransform)
        {
            hammerMovingAndRotating.PutLittlePickableObjectOnTableWithRotation(targetTransform);
        }

        public IEnumerator ImproveTable()
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartBuildsTrigger);
            upgradableTable.HideUpgradeIndicator();
            yield return new WaitForSeconds(gameConfiguration.TableImprovementTime);
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.FinishBuildsTrigger);
            upgradableTable.UpgradeTable();
            playerBusyness.SetPlayerFree();
        }
    }
}
