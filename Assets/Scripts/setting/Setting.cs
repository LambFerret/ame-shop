using System.Collections;
using player;
using player.data;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace setting
{
    public class Setting : MonoBehaviour, IDataPersistence
    {
        public Slider musicVolumeSlider;
        public Slider sfxVolumeSlider;
        public Dropdown languageDropdown;
        public Button resetGameButton;

        private bool _isChanging;

        private void Awake()
        {
            musicVolumeSlider = GameObject.Find("BGM").transform.Find("Slider").GetComponent<Slider>();
            sfxVolumeSlider = GameObject.Find("SFX").transform.Find("Slider").GetComponent<Slider>();
            languageDropdown = GameObject.Find("Language").transform.Find("Dropdown").GetComponent<Dropdown>();
            resetGameButton = GameObject.Find("Reset").transform.Find("Button").GetComponent<Button>();
        }

        private void Start()
        {
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            sfxVolumeSlider.onValueChanged.AddListener(OnEffectVolumeChanged);
            languageDropdown.onValueChanged.AddListener(ChangeLocale);
            resetGameButton.onClick.AddListener(OnResetGameClicked);
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            DataPersistenceManager.Instance.LoadGame();
        }

        private void OnDestroy()
        {
            musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
            sfxVolumeSlider.onValueChanged.RemoveListener(OnEffectVolumeChanged);
            languageDropdown.onValueChanged.RemoveListener(ChangeLocale);
            resetGameButton.onClick.RemoveListener(OnResetGameClicked);
        }

        public void LoadData(GameData data)
        {
            musicVolumeSlider.value = data.musicVolume;
            sfxVolumeSlider.value = data.sfxVolume;

            languageDropdown.value = data.language;

            SoundManager.Instance.MusicVolume(data.musicVolume);
            SoundManager.Instance.SFXVolume(data.sfxVolume);

            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[data.language];
        }


        public void SaveData(GameData data)
        {
            data.musicVolume = musicVolumeSlider.value;
            data.sfxVolume = sfxVolumeSlider.value;
            data.language = languageDropdown.value;
        }

        private static void OnMusicVolumeChanged(float value)
        {
            SoundManager.Instance.MusicVolume(value);
        }

        private float previousVolumeValue = -1;  // Initialize with a value that's out of the expected range

        private void OnEffectVolumeChanged(float value)
        {
            SoundManager.Instance.SFXVolume(value);

            if (previousVolumeValue == -1) // Initial case
            {
                previousVolumeValue = value;
                return;
            }

            float difference = Mathf.Abs(value - previousVolumeValue);
            if (difference >= 0.1f)  // Adjust the threshold as needed
            {
                SoundManager.Instance.PlaySFX(SoundManager.SFX.UIClick);
                previousVolumeValue = value;  // Update the stored value
            }
        }

        private static void OnResetGameClicked()
        {
            DataPersistenceManager.Instance.NewGame();
        }

        private void ChangeLocale(int index)
        {
            if (_isChanging) return;
            StartCoroutine(ChangeRoutine(index));
        }

        private IEnumerator ChangeRoutine(int index)
        {
            _isChanging = true;

            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

            _isChanging = false;
        }

        public void Open()
        {
            gameObject.SetActive(true);
            SoundManager.Instance.PlaySFX(SoundManager.SFX.UIClick);
        }

        public void SaveAndConfirm()
        {
            SoundManager.Instance.PlaySFX(SoundManager.SFX.UIClick);
            DataPersistenceManager.Instance.SaveGame();
            Close();
        }

        public void Close()
        {
            SoundManager.Instance.PlaySFX(SoundManager.SFX.PanelClose);
            gameObject.SetActive(false);
        }
    }
}