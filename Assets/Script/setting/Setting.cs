using System;
using System.Collections;
using Script.persistence;
using Script.persistence.data;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Script.setting
{
    public class Setting : MonoBehaviour, IDataPersistence
    {
        public Slider volumeSlider;
        public Dropdown languageDropdown;
        public Button resetGameButton;

        private bool _isChanging;

        private void Awake()
        {
            volumeSlider = GameObject.Find("Volume").transform.Find("Slider").GetComponent<Slider>();
            languageDropdown = GameObject.Find("Language").transform.Find("Dropdown").GetComponent<Dropdown>();
            resetGameButton = GameObject.Find("Reset").transform.Find("Button").GetComponent<Button>();
        }

        private void Start()
        {
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
            languageDropdown.onValueChanged.AddListener(ChangeLocale);
            resetGameButton.onClick.AddListener(OnResetGameClicked);
        }

        private void OnDestroy()
        {
            volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
            languageDropdown.onValueChanged.RemoveListener(ChangeLocale);
            resetGameButton.onClick.RemoveListener(OnResetGameClicked);
        }

        private static void OnVolumeChanged(float value)
        {
            AudioListener.volume = value;
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

        private void OnEnable()
        {
            DataPersistenceManager.Instance.LoadGame();
        }

        public void SaveAndConfirm()
        {
            DataPersistenceManager.Instance.SaveGame();
            Close();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void LoadData(GameData data)
        {
            // Assuming data.volume is a float that represents the volume
            volumeSlider.value = data.volume;

            // Assuming data.language is an int that represents the language index
            languageDropdown.value = data.language;

            // Update the AudioListener volume and selected locale
            AudioListener.volume = data.volume;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[data.language];
        }

        public void SaveData(GameData data)
        {
            // Save volume and language values to GameData
            Debug.Log("ㅂㅗㄹ륨 " + volumeSlider.value + " 언어 " + languageDropdown.value);
            data.volume = volumeSlider.value;
            data.language = languageDropdown.value;
        }
    }
}