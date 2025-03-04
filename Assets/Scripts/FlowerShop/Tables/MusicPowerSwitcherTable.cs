﻿using System.Collections;
using DG.Tweening;
using FlowerShop.Achievements;
using FlowerShop.Help;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Settings;
using FlowerShop.Sounds;
using FlowerShop.Tables.Abstract;
using PlayerControl;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    public class MusicPowerSwitcherTable : Table, ISavableObject
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly HelpCanvasLiaison helpCanvasLiaison;
        [Inject] private readonly HelpTexts helpTexts;
        [Inject] private readonly LetThereBeSound letThereBeSound;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly SoundSettings soundSettings;

        [SerializeField] private Transform musicPowerSwitcherTransform;
        [SerializeField] private MusicSongsSwitcherTable musicSongsSwitcherTable;
        [SerializeField] private Vector3 powerSwitcherOffRotation;
        [SerializeField] private Vector3 powerSwitcherOnRotation;
        [SerializeField] private AudioSource musicAudioSource;

        [SerializeField] private ParticleSystem[] musicEffects;

        [field: SerializeField] public string UniqueKey { get; private set; }
        public bool IsMusicPowerOn { get; private set; }

        private void Awake()
        {
            Load();
        }

        private void Start()
        {
            if (IsMusicPowerOn)
            {
                musicPowerSwitcherTransform.DOLocalRotate(
                    endValue: powerSwitcherOnRotation,
                    duration: actionsWithTransformSettings.MovingPickableObjectTimeDelay,
                    mode: RotateMode.Fast);
                
                TurnMusicOn();
            }
        }

        private protected override void TryInteractWithTable()
        {
            base.TryInteractWithTable();

            if (CanPlayerUseMusicPowerSwitcherTable())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(
                    () => StartCoroutine(UseMusicPowerSwitcherTable()));
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
            else if (!playerPickableObjectHandler.IsPickableObjectNull)
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.HandsFull);
            }
        }

        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerUseMusicPowerSwitcherTable() || CanPlayerUseTableInfoCanvas();
        }

        public void SetAudioClip(AudioClip audioClip)
        {
            musicAudioSource.clip = audioClip;
            musicAudioSource.Play();
        }

        public void StartPlayingMusicAudioSource(AudioClip currentSong)
        {
            musicAudioSource.clip = currentSong;
            musicAudioSource.Play();
            musicAudioSource.DOFade(
                endValue: soundSettings.MaxMusicSoundVolume,
                duration: soundSettings.IncreasingVolumeTime);

            foreach (ParticleSystem musicEffect in musicEffects)
            {
                musicEffect.Play();
            }
        }
        
        public void StopPlayingMusicAudioSource()
        {
            musicAudioSource.DOFade(
                    endValue: soundSettings.VolumeOnStartPlaying, 
                    duration: soundSettings.DecreasingVolumeTime).
                OnComplete(musicAudioSource.Stop);
            
            foreach (ParticleSystem musicEffect in musicEffects)
            {
                musicEffect.Stop();
            }
        }

        public void Load()
        {
            MusicPowerSwitcherTableForSaving musicPowerSwitcherTableForSaving =
                SavesHandler.Load<MusicPowerSwitcherTableForSaving>(UniqueKey);

            if (musicPowerSwitcherTableForSaving.IsValuesSaved)
            {
                IsMusicPowerOn = musicPowerSwitcherTableForSaving.IsMusicPowerOn;
            }
        }

        public void Save()
        {
            MusicPowerSwitcherTableForSaving musicPowerSwitcherTableForSaving = new(IsMusicPowerOn);
            SavesHandler.Save(UniqueKey, musicPowerSwitcherTableForSaving);
        }

        private bool CanPlayerUseMusicPowerSwitcherTable()
        {
            return playerBusyness.IsPlayerFree && playerPickableObjectHandler.IsPickableObjectNull;
        }

        private IEnumerator UseMusicPowerSwitcherTable()
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartCrossingTrigger);

            yield return new WaitForSeconds(actionsWithTransformSettings.MovingPickableObjectTimeDelay);

            Vector3 targetRotation = IsMusicPowerOn ? powerSwitcherOffRotation : powerSwitcherOnRotation;

            musicPowerSwitcherTransform.DOLocalRotate(
                    endValue: targetRotation, 
                    duration: actionsWithTransformSettings.MovingPickableObjectTimeDelay, 
                    mode: RotateMode.Fast).
                OnComplete(OnMusicPowerSwitcherRotationComplete);
        }

        private bool CanPlayerUseTableInfoCanvas()
        {
            return playerBusyness.IsPlayerFree && playerPickableObjectHandler.CurrentPickableObject is InfoBook;
        }

        private void UseTableInfoCanvas()
        {
            tableInfoCanvasLiaison.ShowCanvas(tableInfo, growingRoom);
        }

        private void OnMusicPowerSwitcherRotationComplete()
        {
            if (IsMusicPowerOn)
            {
                IsMusicPowerOn = false;
                StopPlayingMusicAudioSource();
                musicSongsSwitcherTable.TurnReadingNeedleOff();
                
                SavesHandler.DeletePlayerPrefsKey(UniqueKey);
            }
            else
            {
                IsMusicPowerOn = true;
                TurnMusicOn();
                letThereBeSound.IncreaseProgress();
                
                Save();
            }
            
            playerBusyness.SetPlayerFree();
            selectedTableEffect.ActivateEffectWithDelay();
        }

        private void TurnMusicOn()
        {
            StartPlayingMusicAudioSource(musicSongsSwitcherTable.GetCurrentSong());
            musicSongsSwitcherTable.TurnReadingNeedleOn();
        }
    }
}