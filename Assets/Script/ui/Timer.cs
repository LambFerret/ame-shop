using TMPro;
using UnityEngine;

namespace Script.ui
{
    public class Timer : MonoBehaviour
    {
        public TextMeshProUGUI timerText;

        public void SetTimer(float timer)
        {
            timerText.text = timer.ToString("F2");
        }
    }
}