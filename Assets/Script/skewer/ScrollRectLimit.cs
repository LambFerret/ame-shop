using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.skewer
{
    public class ScrollRectLimit : MonoBehaviour
    {
        private ScrollRect _scrollRect;
        public float lowerLimit = -6380f;
        public float higherLimit = -1280f;

        private void Start()
        {
            _scrollRect = GetComponent<ScrollRect>();
        }

        private void Update()
        {
            if (_scrollRect.content.localPosition.x < lowerLimit)
            {
                Vector3 newPos = _scrollRect.content.localPosition;
                newPos.x = lowerLimit;
                _scrollRect.content.localPosition = newPos;
            }
            if (_scrollRect.content.localPosition.x > higherLimit)
            {
                Vector3 newPos = _scrollRect.content.localPosition;
                newPos.x = higherLimit;
                _scrollRect.content.localPosition = newPos;
            }
        }
    }
}