using System.Collections;
using FlowerShop.Saves.SaveData;
using FlowerShop.Settings;
using FlowerShop.Tables.Abstract;
using PlayerControl;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    [RequireComponent(typeof(Animator))]
    public class MusicSongsSwitcherTable : Table, ISavableObject
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly PlayerComponents playerComponents;

        [SerializeField] private MusicPowerSwitcherTable musicPowerSwitcherTable;
        [SerializeField] private AudioClip[] song;

        [HideInInspector, SerializeField] private Animator musicSongsSwitcherTableAnimator;
        
        private int currentSongNumber;
        private static readonly int PowerOn = Animator.StringToHash("PowerOn");
        private static readonly int PowerOff = Animator.StringToHash("PowerOff");
        private static readonly int Switch = Animator.StringToHash("Switch");

        [field: SerializeField] public string UniqueKey { get; private set; }

        private void OnValidate()
        {
            musicSongsSwitcherTableAnimator = GetComponent<Animator>();
        }

        private void Awake()
        {
            Load();
        }

        public override void ExecuteClickableAbility()
        {
            base.ExecuteClickableAbility();

            if (CanPlayerUseMusicSongsSwitcherTable())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(
                    () => StartCoroutine(UseMusicSongsSwitcherTable()));
            }
        }
        
        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerUseMusicSongsSwitcherTable();
        }

        public AudioClip GetCurrentSong()
        {
            return song[currentSongNumber];
        }

        public void TurnReadingNeedleOn()
        {
            musicSongsSwitcherTableAnimator.SetTrigger(PowerOn);
        }
        
        public void TurnReadingNeedleOff()
        {
            musicSongsSwitcherTableAnimator.SetTrigger(PowerOff);
        }

        public void Load()
        {
            MusicSongsSwitcherTableForSaving musicSongsSwitcherTableForSaving =
                SavesHandler.Load<MusicSongsSwitcherTableForSaving>(UniqueKey);

            if (musicSongsSwitcherTableForSaving.IsValuesSaved)
            {
                currentSongNumber = musicSongsSwitcherTableForSaving.CurrentSongNumber;
            }
        }

        public void Save()
        {
            MusicSongsSwitcherTableForSaving musicSongsSwitcherTableForSaving = new(currentSongNumber);
            SavesHandler.Save(UniqueKey, musicSongsSwitcherTableForSaving);
        }

        private bool CanPlayerUseMusicSongsSwitcherTable()
        {
            return playerBusyness.IsPlayerFree && playerPickableObjectHandler.IsPickableObjectNull &&
                   musicPowerSwitcherTable.IsMusicPowerOn;
        }

        private IEnumerator UseMusicSongsSwitcherTable()
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartCrossingTrigger);

            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);
            
            musicSongsSwitcherTableAnimator.SetTrigger(Switch);
        }

        private void StopPlayingMusicAudioSource()
        {
            musicPowerSwitcherTable.StopPlayingMusicAudioSource();
        }
        
        private void StartPlayingMusicAudioSource()
        {
            currentSongNumber++;
            if (currentSongNumber >= song.Length)
            {
                currentSongNumber = 0;
            }
            musicPowerSwitcherTable.StartPlayingMusicAudioSource(song[currentSongNumber]);
            
            Save();
        }

        private void SetPlayerFree()
        {
            playerBusyness.SetPlayerFree();
            selectedTableEffect.ActivateEffectWithDelay();
        }
    }
}