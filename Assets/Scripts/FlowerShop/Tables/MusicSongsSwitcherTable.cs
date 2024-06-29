using System.Collections;
using FlowerShop.Achievements;
using FlowerShop.Help;
using FlowerShop.PickableObjects;
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
        [Inject] private readonly HelpCanvasLiaison helpCanvasLiaison;
        [Inject] private readonly HelpTexts helpTexts;
        [Inject] private readonly MusicLover musicLover;
        [Inject] private readonly PlayerComponents playerComponents;

        public delegate void SwitchSong();
        public event SwitchSong SwitchSongEvent;

        [SerializeField] private MusicPowerSwitcherTable musicPowerSwitcherTable;
        [SerializeField] private AudioClip[] song;

        [HideInInspector, SerializeField] private Animator musicSongsSwitcherTableAnimator;

        private float currentListenSongTime;
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

        private protected override void OnEnable()
        {
            base.OnEnable();

            cyclicalSaver.CyclicalSaverEvent += Save;
        }
        
        private protected override void OnDisable()
        {
            base.OnDisable();

            cyclicalSaver.CyclicalSaverEvent -= Save;
        }

        private void Update()
        {
            if (musicPowerSwitcherTable.IsMusicPowerOn)
            {
                if (!isSongsListened[currentSongNumber])
                {
                    listenSongsTimes[currentSongNumber] += Time.deltaTime;

                    if (listenSongsTimes[currentSongNumber] >= song[currentSongNumber].length - 5)
                    {
                        isSongsListened[currentSongNumber] = true;
                        musicLover.IncreaseProgress();
                    }
                }

                currentListenSongTime += Time.deltaTime;

                if (currentListenSongTime >= song[currentSongNumber].length)
                {
                    currentListenSongTime = 0;
                    SwitchSongEvent?.Invoke();
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
            else if (CanPlayerUseTableInfoCanvas())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(UseTableInfoCanvas);
            }
            else
            {
                TryToShowHelpCanvas();
            }
        }

        private void TryToShowHelpCanvas()
        {
            if (!playerBusyness.IsPlayerFree)
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.PlayerBusy);
            }
            else if (playerPickableObjectHandler.IsPickableObjectNull)
            {
                if (!musicPowerSwitcherTable.IsMusicPowerOn)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.MusicPowerOff);
                }
            }
            else if (!playerPickableObjectHandler.IsPickableObjectNull)
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.HandsFull);
            }
        }


        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerUseMusicSongsSwitcherTable() || CanPlayerUseTableInfoCanvas();
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

        public void SwitchPlaybackMode(SwitchSong switchSongAction)
        {
            SwitchSongEvent = null;
            SwitchSongEvent += switchSongAction;
        }

        public void RepeatPlayback()
        {

        }

        public void SequencePlayback()
        {
            currentSongNumber++;
            if (currentSongNumber >= song.Length)
            {
                currentSongNumber = 0;
            }

            musicPowerSwitcherTable.SetAudioClip(song[currentSongNumber]);

            Save();
        }

        public void RandomPlayback()
        {
            currentSongNumber += Random.Range(1, song.Length);
            if (currentSongNumber >= song.Length)
            {
                currentSongNumber -= song.Length;
            }

            musicPowerSwitcherTable.SetAudioClip(song[currentSongNumber]);

            Save();
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

        private bool CanPlayerUseTableInfoCanvas()
        {
            return playerBusyness.IsPlayerFree && playerPickableObjectHandler.CurrentPickableObject is InfoBook;
        }

        private void UseTableInfoCanvas()
        {
            tableInfoCanvasLiaison.ShowCanvas(tableInfo, growingRoom);
        }
    }
}