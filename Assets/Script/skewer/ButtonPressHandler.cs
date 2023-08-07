namespace Script.skewer
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    public class ButtonPressHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Slider progressBar;
        public BoilerBehavior boiler;
        private bool _isButtonPressed;
        private float _pressTime;
        private const float TimeToComplete = 1.5f;

        private void Update()
        {
            if (!boiler.CheckHandIsSkewer())
            {
                progressBar.value = 0;
                return;
            }
            if (_isButtonPressed)
            {
                _pressTime += Time.deltaTime;
                progressBar.value = _pressTime / TimeToComplete;
                if (_pressTime >= TimeToComplete)
                {
                    Complete();
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isButtonPressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isButtonPressed = false;
            _pressTime = 0f;
            progressBar.value = 0f;
        }

        private void Complete()
        {
            _isButtonPressed = false;
            _pressTime = 0f;
            boiler.Complete();
        }
    }
}