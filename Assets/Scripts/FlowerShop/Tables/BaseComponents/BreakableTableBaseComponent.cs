using System.Collections;
using FlowerShop.RepairsAndUpgrades;
using FlowerShop.Tables.Interfaces;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables.BaseComponents
{
    public class BreakableTableBaseComponent : MonoBehaviour, IBreakableTable
    {
        [Inject] private readonly RepairsAndUpgradesSettings repairsAndUpgradesSettings;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly PlayerBusyness playerBusyness;

        [SerializeField] private MeshRenderer breakdownIndicatorRenderer;
        [SerializeField] private ParticleSystem[] brokenEffects;

        private int actionsBeforeBrokenQuantity;

        public bool IsTableBroken { get; private set; }

        public void HideBreakdownIndicator()
        {
            breakdownIndicatorRenderer.enabled = false;
        }

        public void ShowBreakdownIndicator()
        {
            breakdownIndicatorRenderer.enabled = true;
        }

        public void UseBreakableTable()
        {
            actionsBeforeBrokenQuantity--;

            if (actionsBeforeBrokenQuantity <= 0)
            {
                IsTableBroken = true;

                foreach (ParticleSystem brokenEffect in brokenEffects)
                {
                    brokenEffect.Play();
                }
            }
        }

        public void FixBreakableFlowerTable(int minQuantity, int maxQuantity)
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartBuildsTrigger);
            breakdownIndicatorRenderer.enabled = false;
            SetActionsBeforeBrokenQuantity(minQuantity, maxQuantity);

            StartCoroutine(FixBreakableFlowerTableProcess());
        }

        public void SetActionsBeforeBrokenQuantity(int minQuantity, int maxQuantity)
        {
            actionsBeforeBrokenQuantity = Random.Range(minQuantity, maxQuantity);
        }

        private IEnumerator FixBreakableFlowerTableProcess()
        {
            yield return new WaitForSeconds(repairsAndUpgradesSettings.TableRepairTime);

            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.FinishBuildsTrigger);
            IsTableBroken = false;
            playerBusyness.SetPlayerFree();

            foreach (ParticleSystem brokenEffect in brokenEffects)
            {
                brokenEffect.Stop();
            }
        }
    }
}
