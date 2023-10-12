using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace skewer
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
            putButton.onClick.AddListener(HandleSkewer);
        }

        private void Update()
        {
            if (_currentSkewer is null) return;
            text.text = _currentSkewer.GetIngredientText();
        }

        private void HandleSkewer()
        {
            if (currentState == BoilerState.Nothing)
            {
                ReceiveSkewerFromController();
            }
            else if (currentState == BoilerState.Done)
            {
                GiveSkewerToController();
            }
        }

        private void ReceiveSkewerFromController()
        {
            skewerController.GiveSkewerToBoiler(out GameObject skewerGameObject);
            if (skewerGameObject == null) return;

            _currentSkewerGameObject = skewerGameObject;
            _currentSkewer = skewerGameObject.GetComponent<SkewerBehavior>();
            _currentSkewerGameObject.transform.SetParent(transform);
            _currentSkewerGameObject.transform.SetSiblingIndex(0);
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

        public void ClickMinute(int amount)
        {
            if (currentState == BoilerState.Drying) return;
            minute += amount;
            if (minute < 0) minute = 0;
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
                if (minute > 0) FlipNumber(--minute);

                if (minute == 0)
                {
                    TimerEnded(dryTime);
                    break;
                }
            }
        }

        private void FlipNumber(int value)
        {
            const float duration = 0.2f; // Set duration of your animation.

            _minuteRectTransform.DOScale(new Vector3(1, 0, 1), duration / 2).OnComplete(() =>
            {
                minuteText.text = value.ToString("0");
                _minuteRectTransform.DOScale(new Vector3(1, 1, 1), duration / 2);
            });
        }
    }
}