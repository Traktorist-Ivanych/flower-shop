using FlowerShop.ComputerPages;
using FlowerShop.Effects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Sounds;
using FlowerShop.Tables;
using PlayerControl;
using Saves;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.Achievements
{
    public abstract class Achievement : MonoBehaviour, ISavableObject
    {
        [Inject] private protected readonly AchievementsTextSettings achievementsTextSettings;
        [Inject] private protected readonly AchievementsSettings achievementsSettings;
        [Inject] private readonly ComputerMainPageCanvasLiaison computerMainPageCanvasLiaison;
        [Inject] private readonly ComputerTable computerTable;
        [Inject] private readonly PlayerMoney playerMoney;
        [Inject] private readonly SoundsHandler soundsHandler;
        [Inject] private readonly TopPlayerFlowerShop topPlayerFlowerShop;

        private protected delegate void OnAwardReceived();
        private protected OnAwardReceived OnAwardReceivedEvent;

        [SerializeField, HideInInspector] private protected TextMeshProUGUI achievementText;
        [SerializeField, HideInInspector] private protected TextMeshProUGUI achievementDescription;
        [SerializeField, HideInInspector] private protected Scrollbar achievementScrollbar;
        [SerializeField, HideInInspector] private protected Button achievementButton;
        [SerializeField, HideInInspector] private protected CanvasElementScaler achievementButtonScaler;

        [SerializeField, HideInInspector] private LocalizeStringEvent localizeAchievementNameStringEvent;
        [SerializeField, HideInInspector] private LocalizeStringEvent localizeAchievementDescriptionStringEvent;

        [SerializeField] private LocalizedString localizeAchievementText;
        [SerializeField] private LocalizedString localizeAchievementDescription;
        [SerializeField] private MeshRenderer achievementModel;
        [field: Tooltip("0 - LowMoneyReward;\n1 - MediumMoneyReward;\n2 - HighMoneyReward.")]
        [SerializeField, Range(0,2)] private int moneyRewardIndex;

        private protected int achievementMaxProgress;
        private protected int achievementProgress;
        private protected bool isAchievementDone;
        private protected bool isAwardReceived;

        [field: SerializeField] public string UniqueKey { get; private set; }
        
        private void OnValidate()
        {
            achievementScrollbar = GetComponentInChildren<Scrollbar>();
            achievementButton = GetComponentInChildren<Button>();
            achievementButtonScaler = GetComponentInChildren<CanvasElementScaler>();

            LocalizeStringEvent[] texts = GetComponentsInChildren<LocalizeStringEvent>();

            foreach (LocalizeStringEvent text in texts)
            {
                if (text.gameObject.name == "AchievementText")
                {
                    localizeAchievementNameStringEvent = text;
                }
                else if (text.gameObject.name == "AchievementDescription")
                {
                    localizeAchievementDescriptionStringEvent = text;
                }
                else
                {
                    Debug.LogWarning("AchievementText not found!");
                }
            }
        }

        private void Awake()
        {
            localizeAchievementNameStringEvent.StringReference = localizeAchievementText;
            localizeAchievementDescriptionStringEvent.StringReference = localizeAchievementDescription;

            Load();
        }

        private protected virtual void OnEnable()
        {
            achievementButton.onClick.AddListener(OnAchievementButtonClick);
        }

        private protected virtual void OnDisable()
        {
            achievementButton.onClick.RemoveListener(OnAchievementButtonClick);
        }

        public void SetProgress(int targetProgress)
        {
            if (achievementProgress < achievementMaxProgress)
            {
                achievementProgress = targetProgress;
                UpdateScrollbar();
            
                if (achievementProgress >= achievementMaxProgress)
                {
                    CompleteAchievement();
                }

                Save();
            }
        }
        
        public void IncreaseProgress()
        {
            if (achievementProgress < achievementMaxProgress)
            {
                IncreaseAchievementProgress();
                UpdateScrollbar();
                
                if (achievementProgress >= achievementMaxProgress)
                {
                    CompleteAchievement();
                }
            }
        }

        public void DecreaseAchievementProgress()
        {
            if (achievementProgress < achievementMaxProgress)
            {
                achievementProgress--;
                UpdateScrollbar();
                
                Save();
            }
        }

        public void Load()
        {
            AchievementForSaving achievementForLoading = SavesHandler.Load<AchievementForSaving>(UniqueKey);

            if (achievementForLoading.IsValuesSaved)
            {
                achievementProgress = achievementForLoading.AchievementProgress;
                isAchievementDone = achievementForLoading.IsAchievementDone;
                isAwardReceived = achievementForLoading.IsAwardReceived;

                if (isAchievementDone)
                {
                    if (isAwardReceived)
                    {
                        achievementButton.image.sprite = achievementsSettings.AwardReceived;
                        achievementModel.enabled = true;
                    }
                    else
                    {
                        achievementButton.image.sprite = achievementsSettings.AwardAwaitingReceipt;
                        achievementButtonScaler.ActivateEffect();
                        ShowIndicators();
                    }
                }
            }
        }

        public void Save()
        {
            AchievementForSaving achievementForSaving = new(achievementProgress, isAchievementDone, isAwardReceived);
            SavesHandler.Save(UniqueKey, achievementForSaving);
        }

        private void OnAchievementButtonClick()
        {
            if (isAchievementDone && !isAwardReceived)
            {
                ReceiveAward();
            }
        }

        private void IncreaseAchievementProgress()
        {
            achievementProgress++;
            
            Save();
        }

        private protected void UpdateScrollbar()
        {
            achievementScrollbar.size = (float)achievementProgress / achievementMaxProgress;
        }

        private void CompleteAchievement()
        {
            achievementButtonScaler.ActivateEffect();
            isAchievementDone = true;
            achievementButton.image.sprite = achievementsSettings.AwardAwaitingReceipt;
            ShowIndicators();
            
            Save();
        }

        private void ReceiveAward()
        {
            achievementButton.image.sprite = achievementsSettings.AwardReceived;
            isAwardReceived = true;
            achievementModel.enabled = true;
            achievementButtonScaler.DeactivateEffect();

            int currentMoneyReward = 0;
            switch (moneyRewardIndex)
            {
                case 0:
                    currentMoneyReward = achievementsSettings.LowMoneyReward;
                    break;
                case 1:
                    currentMoneyReward = achievementsSettings.MediumMoneyReward;
                    break;
                case 2:
                    currentMoneyReward = achievementsSettings.HighMoneyReward;
                    break;
            }
            playerMoney.AddPlayerMoney(currentMoneyReward);
            soundsHandler.PlayAddMoneyAudio();

            HideIndicators();

            topPlayerFlowerShop.IncreaseProgress();
            OnAwardReceivedEvent?.Invoke();


            Save();
        }

        private void ShowIndicators()
        {
            computerTable.ShowIndicator();
            computerMainPageCanvasLiaison.ShowAchievementsIndicator();
        }

        private void HideIndicators()
        {
            computerTable.HideIndicator();
            computerMainPageCanvasLiaison.HideAchievementsIndicator();
        }
    }
}
