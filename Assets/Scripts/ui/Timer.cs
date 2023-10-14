using System;
using player;
using player.data;
using setting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ui
{
    public class Timer : MonoBehaviour, IDataPersistence
    {
        public int startHour = 12;
        public int endHour = 18;
        public int upgradeStartHour = 11;
        public int upgradeEndHour = 20;

        // New variable
        public int gameHourLength = 1;

        public bool isUpgraded;

        public bool isStarted;
        public bool isEnded;
        public bool isPaused;
        private float _time;
        private Image _timerImage;
        private TextMeshProUGUI _timerText;
        private bool _dayEndAlert;
        private bool _eveningMusicIsOn;

        private void Awake()
        {
            _timerText = transform.Find("text").GetComponent<TextMeshProUGUI>();
            _timerImage = transform.Find("image").GetComponent<Image>();
        }

        private void Start()
        {
            int adjustedStartHour = isUpgraded ? upgradeStartHour : startHour;
            _timerText.text = $"{adjustedStartHour:00}:00";
            _timerImage.fillAmount = 0;
            _time = 0;
            int rotationDegrees = (12 - adjustedStartHour) * 30;
            _timerImage.rectTransform.Rotate(0, 0, rotationDegrees);
            _dayEndAlert = false;
            _eveningMusicIsOn = false;
        }

        private void Update()
        {
            if (isEnded || isPaused) return;

            _time += Time.deltaTime;
            var adjStartTime = isUpgraded ? upgradeStartHour : startHour;
            var adjEndTime = isUpgraded ? upgradeEndHour : endHour;

            var hour = (int)Math.Floor(_time / gameHourLength) + adjStartTime;
            var totalSecondsInHour = (int)gameHourLength;
            var gameSecondsPassedThisHour = (int)(_time % totalSecondsInHour);
            var minute = gameSecondsPassedThisHour * 60 / totalSecondsInHour;
            minute = (minute / 10) * 10;

            _timerText.text = $"{hour:00}:{minute:00}";

            float totalDegrees = (adjEndTime - adjStartTime) * 360f / 12f;
            float elapsedDegrees = _time / (gameHourLength * (adjEndTime - adjStartTime)) * totalDegrees;
            _timerImage.fillAmount = elapsedDegrees / 360f;

            if (hour == adjEndTime - 1 && minute == 0 && !_dayEndAlert)
            {
                SoundManager.Instance.PlaySFX(SoundManager.SFX.TimeIsRunningOut);
                _dayEndAlert = true;
            }

            if (hour == 5 && minute == 0 && !_eveningMusicIsOn)
            {
                SoundManager.Instance.PlayMusic("CashierNight");
                _eveningMusicIsOn = true;
            }

            if (hour >= adjEndTime) isEnded = true;
        }

        public void Pause(bool pause)
        {
            isPaused = pause;
        }

        public void LoadData(GameData data)
        {
            startHour = (int)data.gameStartTime;
            endHour = (int)data.gameEndTime;
            gameHourLength = (int)data.gameHourLength;
            if (gameHourLength == 0) gameHourLength = 1;
        }

        public void SaveData(GameData data)
        {
        }
    }
}