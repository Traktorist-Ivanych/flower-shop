using FlowerShop.Saves.SaveData;
using Saves;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.Sounds
{
    public class SoundSliderSettings : MonoBehaviour, ISavableObject
    {
        [Inject] private readonly SoundSettings soundSettings;

        [SerializeField] private string parameterName;
        [SerializeField] private AudioMixer audioMixer;

        [HideInInspector, SerializeField] private Slider soundSlider;

        private float sliderVolume;

        [field: SerializeField] public string UniqueKey { get; private set; }

        private void OnValidate()
        {
            soundSlider = GetComponent<Slider>();
        }

        private void Start()
        {
            Load();
        }

        private void OnEnable()
        {
            soundSlider.onValueChanged.AddListener(OnSliderChange);
        }

        private void OnDisable()
        {
            soundSlider.onValueChanged.RemoveListener(OnSliderChange);
        }

        public void Load()
        {
            FloatProgressForSaving floatProgressForLoading = SavesHandler.Load<FloatProgressForSaving>(UniqueKey);

            if (floatProgressForLoading.IsValuesSaved)
            {
                sliderVolume = floatProgressForLoading.Progress;
                soundSlider.value = sliderVolume;
            }
        }

        public void Save()
        {
            FloatProgressForSaving floatProgressForSaving = new (sliderVolume);
            
            SavesHandler.Save(UniqueKey, floatProgressForSaving);
        }

        private void OnSliderChange(float sliderValue)
        {
            sliderVolume = sliderValue;
            OnSliderChangeMain();

            Save();
        }

        private void OnSliderChangeMain()
        {
            float audioMixerVolume = Mathf.Log10(sliderVolume) * soundSettings.SoundSliderSettingsMultiplier;
            audioMixer.SetFloat(parameterName, audioMixerVolume);
        }
    }
}
