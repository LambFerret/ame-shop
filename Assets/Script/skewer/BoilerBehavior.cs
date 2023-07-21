using System;
using System.Collections;
using DG.Tweening;
using Script.machine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.skewer
{
    public class BoilerBehavior : MonoBehaviour
    {
        private Skewer _currentSkewer;
        private GameObject _currentSkewerGameObject;

        public TextMeshProUGUI minuteText;
        public TextMeshProUGUI secondText;

        private RectTransform _minuteRectTransform;
        private RectTransform _secondRectTransform;

        public BoilerState currentState;
        public int minute;
        public int second;
        public int concentration;

        public Button putButton;
        public Button getButton;
        public TextMeshProUGUI text;
        public SkewerController skewerController;

        public enum BoilerState
        {
            Nothing,
            BeforeStart,
            Drying,
            Done
        }

        private void Start()
        {
            currentState = BoilerState.Nothing;
            _minuteRectTransform = minuteText.GetComponent<RectTransform>();
            _secondRectTransform = secondText.GetComponent<RectTransform>();
            putButton.onClick.AddListener(() => ReceiveSkewerFromController());
            getButton.onClick.AddListener(() => GiveSkewerToController(skewerController));
        }

        private void Update()
        {
            if (_currentSkewer == null) return;
            text.text = _currentSkewer.ReadFirstIngredients();
        }

        public void ReceiveSkewerFromController()
        {
            Skewer skewer;
            GameObject skewerGameObject;

            skewerController.GiveSkewerToBoiler(out skewer, out skewerGameObject);

            if (skewer == null || skewerGameObject == null)
            {
                Debug.Log("No skewer to receive.");
                return;
            }

            _currentSkewer = skewer;
            _currentSkewerGameObject = skewerGameObject;
            _currentSkewerGameObject.GetComponent<SkewerBehavior>().SetSkewerFocused(false);
            currentState = BoilerState.BeforeStart;
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
                {
                    minute--;
                }
                else
                {
                    minute = 0;
                }
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
                {
                    second -= 10;
                }
                else
                {
                    second = 0;
                }
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
            StartCoroutine(TimerCoroutine());
        }

        public void ReceiveSkewerFromController(Skewer skewer, GameObject skewerGameObject)
        {
            if (_currentSkewer != null || _currentSkewerGameObject != null)
            {
                Debug.Log("Already boiling a skewer.");
                return;
            }

            if (currentState != BoilerState.Nothing)
            {
                Debug.Log("Boiler is busy.");
                return;
            }

            _currentSkewer = skewer;
            _currentSkewerGameObject = skewerGameObject;
            // possibly move skewerGameObject to a location representative of the boiler
        }

        public void GiveSkewerToController(SkewerController controller)
        {
            if (_currentSkewer == null || _currentSkewerGameObject == null)
            {
                Debug.Log("No skewer to give.");
                return;
            }

            controller.ReceiveSkewerFromBoiler(_currentSkewer, _currentSkewerGameObject);
            _currentSkewer = null;
            _currentSkewerGameObject = null;
        }

        public void TimerEnded(int dryTime)
        {
            Debug.Log("this Timer ended.");
            _currentSkewer.AddDryTime(dryTime);
            switch (concentration)
            {
                case < 30:
                    _currentSkewer.AddSecondIngredient(SecondIngredient.None);
                    break;
                case >= 30 and < 100:
                    _currentSkewer.AddSecondIngredient(SecondIngredient.NormalSugar);
                    break;
                default:
                    _currentSkewer.AddSecondIngredient(SecondIngredient.NormalSugar);
                    Debug.Log("too much sugar");

                    break;
            }

            currentState = BoilerState.Done;
        }

        private IEnumerator TimerCoroutine()
        {
            int dryTime = minute * 60 + second;
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
            float duration = 0.3f; // Set duration of your animation.
            RectTransform rectTransform = isMinute ? _minuteRectTransform : _secondRectTransform;

            // Scale down
            rectTransform.DOScale(new Vector3(1, 0, 1), duration / 2).OnComplete(() =>
            {
                if (isMinute)
                {
                    minuteText.text = value.ToString("0");
                }
                else
                {
                    secondText.text = value.ToString("00");
                }

                rectTransform.DOScale(new Vector3(1, 1, 1), duration / 2);
            });
        }
    }
}