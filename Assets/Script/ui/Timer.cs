using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.ui
{
    public class Timer : MonoBehaviour
    {
        private TextMeshProUGUI _timerText;
        private Image _timerImage;
        private float _time;

        public int startHour = 12;
        public int endHour = 18;
        public int upgradeStartHour = 11;
        public int upgradeEndHour = 20;
        public int timeMultiplier = 600;

        public bool isUpgraded;

        public bool isStarted;
        public bool isEnded;
        public bool isPaused;

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
        }

        private void Update()
        {
            if (isEnded || isPaused) return;
            _time += (Time.deltaTime * timeMultiplier);
            var adjStartTime = isUpgraded ? upgradeStartHour : startHour;
            var adjEndTime = isUpgraded ? upgradeEndHour : endHour;

            var hour = (int)Math.Floor(_time / 3600) + adjStartTime;
            var minute = (int)Math.Floor((_time % 3600) / 60);
            minute = (int)Math.Floor(minute / 10.0) * 10;
            _timerText.text = $"{hour:00}:{minute:00}";

            float totalDegrees = (adjEndTime - adjStartTime) * 360f / 12f;
            float elapsedDegrees = (_time / (3600f * (adjEndTime - adjStartTime))) * totalDegrees;
            _timerImage.fillAmount = elapsedDegrees / 360f;

            if (hour >= adjEndTime) isEnded = true;
        }

        public void Pause(bool pause)
        {
            isPaused = pause;
        }
    }
}