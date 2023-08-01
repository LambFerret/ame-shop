using System.Collections;
using DG.Tweening;
using Script.setting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.skewer
{
    public class DryerBehavior : MonoBehaviour
    {
        public enum BoilerState
        {
            Nothing,
            BeforeStart,
            Drying,
            Done
        }

        public TextMeshProUGUI minuteText;
        public TextMeshProUGUI secondText;

        public BoilerState currentState;
        public int minute;
        public int second;
        public int concentration;

        public Button putButton;
        public Button getButton;
        public TextMeshProUGUI text;
        public GameObject blockingScreen;
        public SkewerController skewerController;
        private SkewerBehavior _currentSkewer;
        private GameObject _currentSkewerGameObject;

        private RectTransform _minuteRectTransform;
        private RectTransform _secondRectTransform;

        private void Start()
        {
            currentState = BoilerState.Nothing;
            _minuteRectTransform = minuteText.GetComponent<RectTransform>();
            _secondRectTransform = secondText.GetComponent<RectTransform>();
            putButton.onClick.AddListener(ReceiveSkewerFromController);
            getButton.onClick.AddListener(GiveSkewerToController);
        }

        private void Update()
        {
            if (_currentSkewer is null) return;
            text.text = _currentSkewer.GetFirstIngredientText();
        }

        private void ReceiveSkewerFromController()
        {
            skewerController.GiveSkewerToBoiler(out GameObject skewerGameObject);
            if (skewerGameObject == null) return;

            _currentSkewerGameObject = skewerGameObject;
            _currentSkewer = skewerGameObject.GetComponent<SkewerBehavior>();
            _currentSkewerGameObject.transform.SetParent(transform);
            currentState = BoilerState.BeforeStart;
        }

        private void GiveSkewerToController()
        {
            if (_currentSkewer == null || _currentSkewerGameObject == null)
            {
                Debug.Log("No skewer to give.");
                return;
            }

            if (!skewerController.ReceiveSkewerFromBoiler(_currentSkewerGameObject)) return;
            _currentSkewer = null;
            _currentSkewerGameObject = null;
            text.text = "EMPTY";
        }

        public void ClickMinute(bool isUp)
        {
            if (currentState == BoilerState.Drying) return;
            if (isUp)
            {
                minute++;
            }
            else
            {
                if (minute is > 0 and < 10)
                    minute--;
                else
                    minute = 0;
            }

            FlipNumber(true, minute);
        }

        public void ClickSecond(bool isUp)
        {
            if (currentState == BoilerState.Drying) return;
            if (isUp)
            {
                second += 10;
            }
            else
            {
                if (second > 10)
                    second -= 10;
                else
                    second = 0;
            }

            FlipNumber(false, second);
        }

        public void StartTimer()
        {
            if (currentState != BoilerState.BeforeStart) return;
            minuteText.text = minute.ToString("0");
            secondText.text = second.ToString("00");

            if (minute is 0 && second is 0) return;
            currentState = BoilerState.Drying;
            blockingScreen.SetActive(true);
            StartCoroutine(TimerCoroutine());
        }

        private void TimerEnded(int dryTime)
        {
            _currentSkewer.AddDryTime(dryTime);
            blockingScreen.SetActive(false);
            currentState = BoilerState.Done;
        }

        private IEnumerator TimerCoroutine()
        {
            var dryTime = minute * 60 + second;
            while (true)
            {
                yield return new WaitForSeconds(2);
                if (second == 0 && minute > 0)
                {
                    minute--;
                    second = 50;
                    FlipNumber(true, minute);
                    FlipNumber(false, second);
                }
                else
                {
                    second -= 10;
                    FlipNumber(false, second);
                }

                if (minute == 0 && second == 0)
                {
                    TimerEnded(dryTime);
                    break;
                }
            }
        }

        private void FlipNumber(bool isMinute, int value)
        {
            var duration = 0.3f; // Set duration of your animation.
            var rectTransform = isMinute ? _minuteRectTransform : _secondRectTransform;

            // Scale down
            rectTransform.DOScale(new Vector3(1, 0, 1), duration / 2).OnComplete(() =>
            {
                if (isMinute)
                    minuteText.text = value.ToString("0");
                else
                    secondText.text = value.ToString("00");

                rectTransform.DOScale(new Vector3(1, 1, 1), duration / 2);
            });
        }
    }
}