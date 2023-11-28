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

        private LittlePickableObjectMoving hammerMoving;
        private IUpgradableTable upgradableTable;

        public IUpgradableTable UpgradableTable
        {
            set => upgradableTable = value;
        }

        private void Start()
        {
            hammerMoving = GetComponent<LittlePickableObjectMoving>();
        }

        public void TakeInPlayerHands()
        {
            playerPickableObjectHandler.CurrentPickableObject = this;
            hammerMoving.TakeLittlePickableObjectInPlayerHands();
        }

        public void PutOnTable(Transform targetTransform)
        {
            hammerMoving.PutLittlePickableObjectOnTable(targetTransform);
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
