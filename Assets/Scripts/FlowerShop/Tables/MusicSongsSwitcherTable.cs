using System.Collections;
using FlowerShop.Achievements;
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
        [Inject] private readonly CyclicalSaver cyclicalSaver;
        [Inject] private readonly MusicLover musicLover;
        [Inject] private readonly PlayerComponents playerComponents;

        [SerializeField] private MusicPowerSwitcherTable musicPowerSwitcherTable;
        [SerializeField] private AudioClip[] song;

        [HideInInspector, SerializeField] private Animator musicSongsSwitcherTableAnimator;

        private float[] listenSongsTimes;
        private bool[] isSongsListened;
        
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
            listenSongsTimes = new float[song.Length];
            isSongsListened = new bool[song.Length];
            
            Load();
        }

        private void OnEnable()
        {
            cyclicalSaver.CyclicalSaverEvent += Save;
        }
        
        private void OnDisable()
        {
            cyclicalSaver.CyclicalSaverEvent -= Save;
        }

        private void Update()
        {
            if (musicPowerSwitcherTable.IsMusicPowerOn && !isSongsListened[currentSongNumber])
            {
                listenSongsTimes[currentSongNumber] += Time.deltaTime;

                if (listenSongsTimes[currentSongNumber] >= song[currentSongNumber].length)
                {
                    isSongsListened[currentSongNumber] = true;
                    musicLover.IncreaseProgress();
                }
            }
        }

        private protected override void TryInteractWithTable()
        {
            base.TryInteractWithTable();

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
            MusicSongsSwitcherTableForSaving musicSongsSwitcherTableForLoading =
                SavesHandler.Load<MusicSongsSwitcherTableForSaving>(UniqueKey);

            if (musicSongsSwitcherTableForLoading.IsValuesSaved)
            {
                currentSongNumber = musicSongsSwitcherTableForLoading.CurrentSongNumber;
                listenSongsTimes = musicSongsSwitcherTableForLoading.ListenSongsTimes;
                isSongsListened = musicSongsSwitcherTableForLoading.IsSongsListened;
            }
        }

        public void Save()
        {
            MusicSongsSwitcherTableForSaving musicSongsSwitcherTableForSaving =
                new(currentSongNumber, listenSongsTimes, isSongsListened);
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