using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.skewer
{
    public class ButtonPressHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private const float TimeToComplete = 1.5f;
        public Slider progressBar;
        public BoilerBehavior boiler;
        private bool _isButtonPressed;
        private float _pressTime;

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
                if (_pressTime >= TimeToComplete) Complete();
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