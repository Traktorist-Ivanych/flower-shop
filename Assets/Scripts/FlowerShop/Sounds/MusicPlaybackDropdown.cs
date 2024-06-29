using FlowerShop.Saves.SaveData;
using FlowerShop.Tables;
using Saves;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

namespace FlowerShop.Sounds
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class MusicPlaybackDropdown : MonoBehaviour , ISavableObject
    {
        [Inject] private readonly MusicSongsSwitcherTable musicSongsSwitcherTable;

        [SerializeField] private LocalizedString LocalizedRepeatText;
        [SerializeField] private LocalizedString LocalizedConsistentlyText;
        [SerializeField] private LocalizedString LocalizedRandomOrderText;

        private int dropdownValue;

        [HideInInspector, SerializeField] private TMP_Dropdown musicDropdown;

        [field: SerializeField] public string UniqueKey { get; private set; }

        private void OnValidate()
        {
            musicDropdown = GetComponent<TMP_Dropdown>();
        }

        private void Awake()
        {
            musicDropdown.options[0].text = LocalizedRepeatText.GetLocalizedString();
            musicDropdown.options[1].text = LocalizedConsistentlyText.GetLocalizedString();
            musicDropdown.options[2].text = LocalizedRandomOrderText.GetLocalizedString();
        }

        private void Start()
        {
            Load();
        }

        private void OnEnable()
        {
            musicDropdown.onValueChanged.AddListener(OnDropDownValueChange);
        }

        private void OnDisable()
        {
            musicDropdown.onValueChanged.RemoveListener(OnDropDownValueChange);
        }

        public void Save()
        {
            IntForSaving intForSaving = new(dropdownValue);

            SavesHandler.Save(UniqueKey, intForSaving);
        }

        public void Load()
        {
            IntForSaving intForLoading = SavesHandler.Load<IntForSaving>(UniqueKey);

            if (intForLoading.IsValuesSaved)
            {
                dropdownValue = intForLoading.SavingInt;
                musicDropdown.value = dropdownValue;
            }
        }

        private void OnDropDownValueChange(int currentDropdownValue)
        {
            dropdownValue = currentDropdownValue;

            switch (dropdownValue)
            {
                case 0:
                    musicSongsSwitcherTable.SwitchPlaybackMode(musicSongsSwitcherTable.RepeatPlayback);
                    break;
                case 1:
                    musicSongsSwitcherTable.SwitchPlaybackMode(musicSongsSwitcherTable.SequencePlayback);
                    break;
                case 2:
                    musicSongsSwitcherTable.SwitchPlaybackMode(musicSongsSwitcherTable.RandomPlayback);
                    break;
            }

            Save();
        }
    }
}
