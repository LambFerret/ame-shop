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

        public int timeMultiplier = 1200;
        public bool isStarted;
        public bool isEnded;

        private void Awake()
        {
            _timerText = transform.Find("text").GetComponent<TextMeshProUGUI>();
            _timerImage = transform.Find("image").GetComponent<Image>();
        }

        private void Start()
        {
            _timerText.text = "09:00";
            _timerImage.fillAmount = 0;
        }

        private void Update()
        {
            if (isEnded) return;
            var time = (int)Math.Floor(Time.time * timeMultiplier);
            var hour = (time / 3600) + 9;
            var minute = (time % 3600) / 60;
            _timerText.text = $"{hour:00}:{minute:00}";
            _timerImage.fillAmount = (float)time / (21 - 9) / 3600;
            if (hour >= 21) isEnded = true;
        }
    }
}