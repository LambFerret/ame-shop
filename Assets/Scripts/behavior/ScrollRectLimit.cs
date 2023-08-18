using UnityEngine;
using UnityEngine.UI;

namespace behavior
{
    public class ScrollRectLimit : MonoBehaviour
    {
        public float lowerLimit = -6380f;
        public float higherLimit = -1280f;
        private ScrollRect _scrollRect;

        private void Start()
        {
            _scrollRect = GetComponent<ScrollRect>();
        }

        private void Update()
        {
            if (_scrollRect.content.localPosition.x < lowerLimit)
            {
                var newPos = _scrollRect.content.localPosition;
                newPos.x = lowerLimit;
                _scrollRect.content.localPosition = newPos;
            }

            if (_scrollRect.content.localPosition.x > higherLimit)
            {
                var newPos = _scrollRect.content.localPosition;
                newPos.x = higherLimit;
                _scrollRect.content.localPosition = newPos;
            }
        }
    }
}