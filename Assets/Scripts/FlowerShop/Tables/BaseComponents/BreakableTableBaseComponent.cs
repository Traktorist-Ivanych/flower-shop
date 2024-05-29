using System.Collections;
using FlowerShop.Achievements;
using FlowerShop.Education;
using FlowerShop.Effects;
using FlowerShop.RepairsAndUpgrades;
using FlowerShop.Sounds;
using PlayerControl;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables.BaseComponents
{
    public class BreakableTableBaseComponent : MonoBehaviour
    {
        [Inject] private readonly ActionProgressbar playerActionProgressbar;
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly Handyman handyman;
        [Inject] private readonly RepairsAndUpgradesTable repairsAndUpgradesTable;
        [Inject] private readonly RepairsAndUpgradesSettings repairsAndUpgradesSettings;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly PlayerBusyness playerBusyness;
        [Inject] private readonly SelectedTableEffect selectedTableEffect;
        [Inject] private readonly SoundsHandler soundsHandler;

        [SerializeField] private ParticleSystem[] brokenEffects;
        
        private ISavableObject savingTable;

        public int ActionsBeforeBrokenQuantity { get; private set; }

        public bool IsTableBroken { get; private set; }

        private void Start()
        {
            savingTable = GetComponent<ISavableObject>();
        }

        public void UseBreakableTable()
        {
            ActionsBeforeBrokenQuantity--;
            CheckIfTableBroken();
        }

        public void CheckIfTableBroken()
        {
            if (ActionsBeforeBrokenQuantity <= 0)
            {
                BrokenTable();
            }
        }

        public void BrokenTable()
        {
            ActionsBeforeBrokenQuantity = 0;
            repairsAndUpgradesTable.IncreaseTablesThatNeedRepairQuantity();
            selectedTableEffect.TryToRecalculateEffect();
            IsTableBroken = true;

            soundsHandler.StartPlayingBrokenTableAudio();
                
            foreach (ParticleSystem brokenEffect in brokenEffects)
            {
                brokenEffect.Play();
            }
        }

        public IEnumerator FixBreakableFlowerTable(int minQuantity, int maxQuantity)
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartBuildsTrigger);

            playerActionProgressbar.EnableActionProgressbar(repairsAndUpgradesSettings.TableRepairTime);
            yield return new WaitForSeconds(repairsAndUpgradesSettings.TableRepairTime);
            
            SetActionsBeforeBrokenQuantity(minQuantity, maxQuantity);
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
            
            if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
            {
                educationHandler.CompleteEducationStep();
            }
            
            handyman.IncreaseProgress();
            
            savingTable.Save();
        }

        public void SetActionsBeforeBrokenQuantity(int minQuantity, int maxQuantity)
        {
            ActionsBeforeBrokenQuantity = Random.Range(minQuantity, maxQuantity);
        }

        public void LoadActionsBeforeBrokenQuantity(int loadedActionsBeforeBrokenQuantity)
        {
            ActionsBeforeBrokenQuantity = loadedActionsBeforeBrokenQuantity;
        }
    }
}
