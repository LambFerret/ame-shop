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

        [Header("Info")] public BoilerState currentState;
        public int minute;

        [Header("Game Objects")] public Button putButton;
        public Button getButton;
        public TextMeshProUGUI text;
        public GameObject blockingScreen;
        public SkewerController skewerController;
        public TextMeshProUGUI minuteText;
        private SkewerBehavior _currentSkewer;
        private GameObject _currentSkewerGameObject;
        private RectTransform _minuteRectTransform;

        private void Start()
        {
            currentState = BoilerState.Nothing;
            _minuteRectTransform = minuteText.GetComponent<RectTransform>();
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

            FlipNumber(minute);
        }

        public void StartTimer()
        {
            if (currentState != BoilerState.BeforeStart) return;
            minuteText.text = minute.ToString("0");
            if (minute is 0) return;
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
            var dryTime = minute;
            while (true)
            {
                yield return new WaitForSeconds(1F);
                if (minute > 0)
                {
                    FlipNumber(minute--);
                }

                if (minute == 0)
                {
                    TimerEnded(dryTime);
                    break;
                }
            }
        }

        private void FlipNumber(int value)
        {
            var duration = 0.3f; // Set duration of your animation.
            _minuteRectTransform.DOScale(new Vector3(1, 0, 1), duration / 2).OnComplete(() =>
            {
                minuteText.text = value.ToString("0");
                _minuteRectTransform.DOScale(new Vector3(1, 1, 1), duration / 2);
            });
        }
    }
}