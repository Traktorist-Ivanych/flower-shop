using System.Collections;
using FlowerShop.Effects;
using FlowerShop.RepairsAndUpgrades;
using FlowerShop.Sounds;
using FlowerShop.Tables.Interfaces;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables.BaseComponents
{
    public class BreakableTableBaseComponent : MonoBehaviour, IBreakableTable
    {
        [Inject] private readonly RepairsAndUpgradesTable repairsAndUpgradesTable;
        [Inject] private readonly RepairsAndUpgradesSettings repairsAndUpgradesSettings;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly PlayerBusyness playerBusyness;
        [Inject] private readonly SelectedTableEffect selectedTableEffect;
        [Inject] private readonly SoundsHandler soundsHandler;

        [SerializeField] private ParticleSystem[] brokenEffects;

        public int ActionsBeforeBrokenQuantity { get; private set; }

        public bool IsTableBroken { get; private set; }

        public void UseBreakableTable()
        {
            ActionsBeforeBrokenQuantity--;
            CheckIfTableBroken();
        }

        public void CheckIfTableBroken()
        {
            if (ActionsBeforeBrokenQuantity <= 0)
            {
                repairsAndUpgradesTable.IncreaseTablesThatNeedRepairQuantity();
                selectedTableEffect.TryToRecalculateEffect();
                IsTableBroken = true;

                soundsHandler.StartPlayingBrokenTableAudio();
                
                foreach (ParticleSystem brokenEffect in brokenEffects)
                {
                    brokenEffect.Play();
                }
            }
        }

        public void FixBreakableFlowerTable(int minQuantity, int maxQuantity)
        {
            SetActionsBeforeBrokenQuantity(minQuantity, maxQuantity);
            StartCoroutine(FixBreakableFlowerTableProcess());
        }

        public void SetActionsBeforeBrokenQuantity(int minQuantity, int maxQuantity)
        {
            ActionsBeforeBrokenQuantity = Random.Range(minQuantity, maxQuantity);
        }

        public void LoadActionsBeforeBrokenQuantity(int loadedActionsBeforeBrokenQuantity)
        {
            ActionsBeforeBrokenQuantity = loadedActionsBeforeBrokenQuantity;
        }

        private IEnumerator FixBreakableFlowerTableProcess()
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartBuildsTrigger);
            
            yield return new WaitForSeconds(repairsAndUpgradesSettings.TableRepairTime);

            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.FinishBuildsTrigger);
            IsTableBroken = false;
            playerBusyness.SetPlayerFree();
            soundsHandler.StopPlayingBrokenTableAudio();
            selectedTableEffect.ActivateEffectWithoutDelay();
            repairsAndUpgradesTable.DecreaseTablesThatNeedRepairQuantity();
            
            foreach (ParticleSystem brokenEffect in brokenEffects)
            {
                brokenEffect.Stop();
            }
        }
    }
}
